using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Threading.Tasks;
using Talabat.Repository.Data;

namespace Talabat_APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            // Add services to the container.

            #region Configure Service 
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion

            var app = builder.Build();
            #region Update DataBase
            //Create obj from StoreContext to use it in the application
           // StoreContext storeContext = new StoreContext();
           //  await  storeContext.Database.MigrateAsync(); // This will apply any pending migrations to the database
             using  var Scope = app.Services.CreateScope();//Group of services that can be used to resolve dependencies
             var Serives = Scope.ServiceProvider; // Get the service provider from the scope
             var DbContext = Serives.GetRequiredService<StoreContext>(); // Get the StoreContext from the service provider
             await DbContext.Database.MigrateAsync(); // Apply any pending migrations to the database
             
            #endregion

            #region Configur - Http rquest pipline 
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
