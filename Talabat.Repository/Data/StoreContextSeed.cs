using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
    {
        //Read the data from the json file and add it to the database
        public static async Task SeedAsync(StoreContext context)
        {
            if(!context.ProductBrands.Any())
            {
                //Brands data 
                var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                if (Brands!.Count > 0)
                {
                    foreach (var brand in Brands)
                    {
                        await context.Set<ProductBrand>().AddAsync(brand);
                    }
                    await context.SaveChangesAsync();
                }
            }
            if (!context.ProductTypes.Any())
            {
                //Types data
                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                if (Types!.Count > 0)
                {
                    foreach (var type in Types)
                    {
                        await context.Set<ProductType>().AddAsync(type);
                    }
                    await context.SaveChangesAsync();
                }
            }
            if (!context.Products.Any())
            {
                //Products data
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (Products!.Count > 0)
                {
                    foreach (var product in Products)
                    {
                        await context.Set<Product>().AddAsync(product);
                    }
                    await context.SaveChangesAsync();
                }
            }
            if (!context.DeliveryMethods.Any())
            {
                //Products data
                var DeliveryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodsData);
                if (DeliveryMethods!.Count > 0)
                {
                    foreach (var deliveryMethod in DeliveryMethods)
                    {
                        await context.Set<DeliveryMethod>().AddAsync(deliveryMethod);
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
