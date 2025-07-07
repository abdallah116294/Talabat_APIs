namespace Talabat_APIs.Extensions
{
    public static class AddSwaggerExtensions
    {
        public static WebApplication UserSwaggerMiddleWares(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
