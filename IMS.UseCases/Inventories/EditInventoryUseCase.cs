using IMS.CoreBusiness;
using IMS.UseCases.Inventories.PluginInterfaces;

namespace IMS.UseCases.Inventories;

public class EditInventoryUseCase : IEditInventoryUseCase
{
    private readonly IInventoryRepository inventoryRepository;

    public EditInventoryUseCase(IInventoryRepository inventoryRepository)
    {
        this.inventoryRepository = inventoryRepository;
    }
    public async Task ExecuteAsync(Inventory inventory)
    {
        await this.inventoryRepository.UpdateInventoryAsync(inventory);
    }
}