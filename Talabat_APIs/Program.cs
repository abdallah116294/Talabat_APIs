using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Data.Common;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat_APIs.Errors;
using Talabat_APIs.Extensions;
using Talabat_APIs.Helpers;
using Talabat_APIs.MiddelWares;

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
            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });
            builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("Identity"));
            });
            builder.Services.AddApplicationServices();
            //Extension method to add application services for Authentication, Authorization, and Identity
            builder.Services.AddIdentityServices();
            //builder.Services.AddIdentity<AppUser, IdentityRole>()
            //    .AddEntityFrameworkStores<AppIdentityDbContext>();
            //builder.Services.AddAuthentication();
            //   builder.Services.AddScoped<IGenericRepositort<Product>, GenericRepository<Product>>(); // Register the generic repository for Product entity
            //Make it generic to work with any entity type
            //builder.Services.AddScoped(typeof(IGenericRepositort<>), typeof(GenericRepository<>)); // Register the generic repository for any entity type
            ////builder.Services.AddAutoMapper(M=>M.AddProfile(new MappingProfiles())); // Register AutoMapper with the mapping profiles
            //builder.Services.AddAutoMapper(typeof(MappingProfiles));
            //builder.Services.Configure<ApiBehaviorOptions>(optins =>
            //{
            //    // Disable the default model state validation behavior
            //    optins.InvalidModelStateResponseFactory=(actionContext)=> 
            //    {
            //        var errors=actionContext.ModelState.Where(e => e.Value.Errors.Count > 0)
            //            .SelectMany(x => x.Value.Errors)
            //            .Select(x => x.ErrorMessage)
            //            .ToArray(); // Get the error messages from the model state
            //        var ValidationErrors = new ApiVialidationErrorResponse()
            //        {
            //            Errors = errors
            //        };
            //        return new BadRequestObjectResult(ValidationErrors);
            //    };
            //});
            #endregion

            var app = builder.Build();
            #region Update DataBase
            //Create obj from StoreContext to use it in the application
            // StoreContext storeContext = new StoreContext();
            //  await  storeContext.Database.MigrateAsync(); // This will apply any pending migrations to the database
            //use try and catch to handle any exceptions that may occur during the migration process
            using var Scope = app.Services.CreateScope();//Group of services that can be used to resolve dependencies
            var Serives = Scope.ServiceProvider; // Get the service provider from the scope
           
            //create objec from LoggerFactor 
             var LoggerFactory = Serives.GetRequiredService<ILoggerFactory>(); // Get the logger factory from the service provider
            try
            {
                 var DbContext = Serives.GetRequiredService<StoreContext>(); // Get the StoreContext from the service provider
                 await DbContext.Database.MigrateAsync(); // Apply any pending migrations to the database
                var IdentityDbContext = Serives.GetRequiredService<AppIdentityDbContext>(); // Get the AppIdentityDbContext from the service provider
                await IdentityDbContext.Database.MigrateAsync(); // Apply any pending migrations to the identity database
                var userManager = Serives.GetRequiredService<UserManager<AppUser>>();
                await AppIdentitiyDbContextSeed.SeedUser(userManager);
                await StoreContextSeed.SeedAsync(DbContext) ;
            }
            catch (Exception ex)
            {
                var logger =LoggerFactory.CreateLogger<Program>(); // Create a logger for the Program class
                logger.LogError(ex, "An error occurred during migration"); // Log the error message    
            }

            #endregion
            #region DataSeeding 
          //  StoreContextSeed.SeedAsync(StoreContext);
            #endregion
            #region Configur - Http rquest pipline 
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleWare>();
                //User Swagger Extensions 
                app.UserSwaggerMiddleWares();
            }
            app.UseStaticFiles();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
