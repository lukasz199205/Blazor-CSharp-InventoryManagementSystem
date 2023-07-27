using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Activities;

public class PurchaseInventoryUseCase : IPurchaseInventoryUseCase
{
    private readonly IInventoryTransactionRepository inventoryTransactionRepository;
    private readonly IInventoryRepository inventoryRepository;

    public PurchaseInventoryUseCase(IInventoryTransactionRepository inventoryTransactionRepository, IInventoryRepository inventoryRepository)
    {
        this.inventoryTransactionRepository = inventoryTransactionRepository;
        this.inventoryRepository = inventoryRepository;
    }

    public async Task ExecuteAsync(string poNumber, Inventory inventory, int quantity, string doneBy)
    {
        //insert record in transaction table
        inventoryTransactionRepository.PurchaseAsync(poNumber, inventory, quantity, doneBy, inventory.Price);

        //increase quantity
        inventory.Quantity += quantity;
        await this.inventoryRepository.UpdateInventoryAsync(inventory);
    }
}