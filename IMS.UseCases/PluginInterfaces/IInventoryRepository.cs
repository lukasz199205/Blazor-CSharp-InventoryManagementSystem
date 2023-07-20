using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.PluginInterfaces;

public interface IInventoryRepository
{
    Task<IEnumerable<Inventory>> GetInventoriesByNameAsync(string name);

    Task AddInventoryAsync(Inventory inventory);
}