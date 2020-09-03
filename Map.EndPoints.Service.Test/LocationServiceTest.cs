namespace Map.EndPoints.Service.Test
{
    using System;
    using System.Threading.Tasks;

    using Map.EndPoints.Service.Interfaces;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Services.Core.Interfaces;
    using Services.WebApiCaller;

    [TestClass]
    public class LocationServiceTest
    {
        private IApiCaller apiCaller = new ApiCaller();
        private IApiConfiguration apiConfiguration = new MyConfiguration();
        private IMapService ms;

        public LocationServiceTest()
        {
            this.ms = new MapService(apiCaller, apiConfiguration);
        }

        [TestMethod]
        public async Task GetPath()
        {
            try
            {
                var result = await this.ms.LocationService.GetPathAsync(13, DateTime.Now.AddDays(-1), DateTime.Now);
                Assert.IsTrue(result != null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail(e.Message);
            }

        }
    }
}
