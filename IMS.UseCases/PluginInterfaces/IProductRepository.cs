using IMS.CoreBusiness;

namespace IMS.UseCases.Inventories.PluginInterfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
    Task AddProductAsync(Product product);
}