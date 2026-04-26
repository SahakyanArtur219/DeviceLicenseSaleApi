using DeviceLicenseSaleApi.Middleware;

namespace DeviceLicenseSaleApi.Extensions
{
    public static class MiddlewareExtensions
    {
        public static WebApplication UsePresentationMiddleware(this WebApplication app)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("VueDevPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
