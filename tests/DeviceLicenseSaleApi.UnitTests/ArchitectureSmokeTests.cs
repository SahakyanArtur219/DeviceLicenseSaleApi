using Xunit;

namespace DeviceLicenseSaleApi.UnitTests
{
    public class ArchitectureSmokeTests
    {
        [Fact]
        public void ApplicationAssembly_Should_Be_Loadable()
        {
            var assembly = typeof(DeviceLicenseSaleApi.Services.AuthService).Assembly;

            Assert.NotNull(assembly);
        }
    }
}
