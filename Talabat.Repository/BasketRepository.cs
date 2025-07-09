using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;

namespace Talabat.Repository
{  
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer connectionMultiplexer)
        {
           _database = connectionMultiplexer.GetDatabase(); // Get the database from the connection multiplexer
        }
        public Task<bool> DeleteBasketAsync(string basketId)
        {
            return _database.KeyDeleteAsync(basketId); // Delete the basket from Redis using the basketId        }
        }
        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
           var basket = await _database.StringGetAsync(basketId); // Get the basket from Redis using the basketId 
           return basket.IsNull?null:  JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var jsonBasket = JsonSerializer.Serialize(basket); // Serialize the basket to JSON
            var created = await _database.StringSetAsync(basket.Id, jsonBasket, TimeSpan.FromDays(1)); // Set the basket in Redis with a 30-day expiration time
            // Return the task to await the completion of the operation
            if (created==false) return null; // If the operation was successful, return the basket

            return await GetBasketAsync(basket.Id); // Otherwise, retrieve the basket again from Redis
        }
    }
}
