using IMS.CoreBusiness;
using IMS.UseCases.Inventories.PluginInterfaces;

namespace IMS.UseCases.Inventories;

public class ViewInventoriesByNameUseCase
{
    private readonly IInventoryRepository _inventoryRepository;

    public ViewInventoriesByNameUseCase(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }
    
    public async Task<IEnumerable<Inventory>> ExecuteAsync(string name = "")
    {
        return await _inventoryRepository.GetInventoriesByNameAsync(name);
    }
}