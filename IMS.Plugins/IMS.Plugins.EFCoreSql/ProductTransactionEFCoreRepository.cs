using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSql;

public class ProductTransactionEFCoreRepository : IProductTransactionRepository
{
    private readonly IProductRepository productRepository;
    private readonly IInventoryTransactionRepository inventoryTransactionRepository;
    private readonly IInventoryRepository inventoryRepository;
    private readonly IMSContext db;

    public ProductTransactionEFCoreRepository(IProductRepository productRepository, 
        IInventoryTransactionRepository inventoryTransactionRepository,
        IInventoryRepository inventoryRepository,
        IMSContext db)
    {
        this.productRepository = productRepository;
        this.inventoryTransactionRepository = inventoryTransactionRepository;
        this.inventoryRepository = inventoryRepository;
        this.db = db;
    }
    public async Task ProduceAsync(string productionNumber, Product product, int quantity, string doneBy)
    {
        
        var prod = await this.productRepository.GetProductByIdAsync(product.ProductId);
        if (prod != null)
        {
            foreach (var pi in prod.ProductInventories)
            {
                if (pi.Inventory != null)
                {
                    //add inventory transaction
                    await this.inventoryTransactionRepository.ProduceAsync(productionNumber, 
                        pi.Inventory,
                        pi.InventoryQuantity * quantity,
                        doneBy,
                        -1);
                    //decrease inventories
                    var inv = await this.inventoryRepository.GetInventoryByIdAsync(pi.InventoryId);
                    inv.Quantity -= pi.InventoryQuantity * quantity;
                    await this.inventoryRepository.UpdateInventoryAsync(inv);
                }
            }
        }
        
        //add product transaction
        this.db.ProductTransactions.Add(new ProductTransaction
        {
            ProductionNumber = productionNumber,
            ProductId = product.ProductId,
            QuantityBefore = product.Quantity,
            ActivityType = ProductTransactionType.ProduceProduct,
            QuantityAfter = product.Quantity + quantity,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy
        });

        await this.db.SaveChangesAsync();
    }

    public async Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double unitPrice, string doneBy)
    {
        this.db.ProductTransactions.Add(new ProductTransaction
        {
            ActivityType = ProductTransactionType.SellProduct,
            SONumber = salesOrderNumber,
            ProductId = product.ProductId,
            QuantityBefore = product.Quantity,
            QuantityAfter = product.Quantity - quantity,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy,
            UnitPrice = unitPrice
        });

        await this.db.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo,
        ProductTransactionType? transactionType)
    {
        var query = from pt in this.db.ProductTransactions
            join prod in this.db.Products on pt.ProductId equals prod.ProductId
            where
                (string.IsNullOrWhiteSpace(productName) ||
                 prod.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0) &&
                (!dateFrom.HasValue || pt.TransactionDate >= dateFrom.Value.Date) &&
                (!dateTo.HasValue || pt.TransactionDate <= dateTo.Value.Date) &&
                (!transactionType.HasValue || pt.ActivityType == transactionType)
            select pt;
        return await query.Include(x => x.Product).ToListAsync();
    }
}