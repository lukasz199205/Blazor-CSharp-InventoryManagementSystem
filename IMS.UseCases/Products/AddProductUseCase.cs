using IMS.CoreBusiness;
using IMS.UseCases.Inventories.PluginInterfaces;

namespace IMS.UseCases.Products;

public class AddProductUseCase : IAddProductUseCase
{
    private readonly IProductRepository productRepository;

    public AddProductUseCase(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }
    public async Task ExecuteAsync(Product product)
    {
        if(product == null) return;
        // if (!await productRepository.ExistsAsync(product))
        // {
            await this.productRepository.AddProductAsync(product);
        // }
    }
}