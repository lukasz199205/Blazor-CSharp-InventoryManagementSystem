﻿using IMS.CoreBusiness;
using IMS.Plugins.EFCoreSql;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSql;

public class InventoryTransactionEFCoreRepository : IInventoryTransactionRepository
{
    private readonly IMSContext db;

    public InventoryTransactionEFCoreRepository(IMSContext db)
    {
        this.db = db;
    }

    public async Task PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
    {
        this.db.InventoryTransactions.Add(new InventoryTransaction
        {
            PONumber = poNumber,
            InventoryId = inventory.InventoryId,
            QuantityBefore = inventory.Quantity,
            ActivityType = InventoryTransactionType.PurchaseInventory,
            QuantityAfter = inventory.Quantity + quantity,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy,
            UnitPrice = price
        });
        
        await this.db.SaveChangesAsync();
    }

    public async Task ProduceAsync(string productionNumber, Inventory inventory, int quantityToConsume, string doneBy,
        double price)
    {
        this.db.InventoryTransactions.Add(new InventoryTransaction
        {
            ProductionNumber = productionNumber,
            InventoryId = inventory.InventoryId,
            QuantityBefore = inventory.Quantity,
            ActivityType = InventoryTransactionType.ProduceProduct,
            QuantityAfter = inventory.Quantity - quantityToConsume,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy,
            UnitPrice = price
        });

        await this.db.SaveChangesAsync();
    }

    public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName,
        DateTime? dateFrom, DateTime? dateTo,
        InventoryTransactionType? transactionType)
    {
        var query = from it in this.db.InventoryTransactions
            join inv in this.db.Inventories on it.InventoryId equals inv.InventoryId
            where
                (string.IsNullOrWhiteSpace(inventoryName) ||
                 inv.InventoryName.ToLower().IndexOf(inventoryName.ToLower()) >= 0) &&
                (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date) &&
                (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date) &&
                (!transactionType.HasValue || it.ActivityType == transactionType)
            select it;
        return await query.Include(x => x.Inventory).ToListAsync();
    }
}