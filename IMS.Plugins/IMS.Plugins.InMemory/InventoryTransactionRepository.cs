using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory;

public class InventoryTransactionRepository : IInventoryTransactionRepository
{
    public List<InventoryTransaction> _inventoryTrasactions = new List<InventoryTransaction>();
    
    public void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price)
    {
        this._inventoryTrasactions.Add(new InventoryTransaction
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
    }
}