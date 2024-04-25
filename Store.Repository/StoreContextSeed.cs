using Microsoft.Extensions.Logging;
using Store.Data.Context;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDbContext context,ILoggerFactory loggerFactory)
        {
            try
            {
                if(context.ProductBrands!=null&&!context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Store.Repository/SeedData/brands.json");
                    var brands = JsonSerializer.Deserialize < List<ProductBrand>>(brandsData);
                    if (brands != null)
                    {
                        foreach (var brand in brands)
                        {
                            await context.ProductBrands.AddAsync(brand);
                        }
                       
                    }
                }
                if (context.ProductTypes != null && !context.ProductTypes.Any())
                {
                    var typesData = File.ReadAllText("../Store.Repository/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                    if (types != null)
                    {
                        foreach (var type in types)
                        {
                            await context.ProductTypes.AddAsync(type);
                        }
                       
                    }
                }
                if (context.Products!= null && !context.Products.Any())
                {
                    var productData = File.ReadAllText("../Store.Repository/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productData);
                    if (products != null)
                    {
                        foreach (var product in products)
                        {
                            await context.Products.AddAsync(product);
                        } 
                      
                    }
                } 
                if (context.DeliveryMethods!= null && !context.DeliveryMethods.Any())
                {
                    var deliveryMethodsData = File.ReadAllText("../Store.Repository/SeedData/delivery.json");
                    var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);
                    if (deliveryMethods != null)
                    {
                        foreach (var delivery in deliveryMethods)
                        {
                            await context.DeliveryMethods.AddAsync(delivery);
                        } 
                      
                    }
                }
                await context.SaveChangesAsync();
            }
            catch(Exception e) {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(e.Message);
            }
        }
    }
}
