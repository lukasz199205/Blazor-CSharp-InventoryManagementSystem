﻿using IMS.CoreBusiness;
using IMS.UseCases.Inventories.PluginInterfaces;

namespace IMS.UseCases.Products;

public class EditProductUseCase : IEditProductUseCase
{
    private readonly IProductRepository productRepository;

    public EditProductUseCase(IProductRepository productRepository)
    {
        this.productRepository = productRepository;
    }

    public async Task ExecuteAsync(Product product)
    {
        await this.productRepository.UpdateProductAsync(product);
    }
}