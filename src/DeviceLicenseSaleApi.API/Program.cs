using DeviceLicenseSaleApi.Extensions;

namespace DeviceLicenseSaleApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddPresentationServices(builder.Configuration);

            var app = builder.Build();

            app.UsePresentationMiddleware();

            app.Run();
        }
    }
}
