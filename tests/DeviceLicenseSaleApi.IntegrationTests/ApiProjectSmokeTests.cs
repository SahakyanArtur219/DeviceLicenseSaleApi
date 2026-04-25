using Xunit;

namespace DeviceLicenseSaleApi.IntegrationTests
{
    public class ApiProjectSmokeTests
    {
        [Fact]
        public void ApiAssembly_Should_Be_Loadable()
        {
            var assembly = typeof(DeviceLicenseSaleApi.Program).Assembly;

            Assert.NotNull(assembly);
        }
    }
}
