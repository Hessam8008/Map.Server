namespace Map.EndPoints.Service.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Map.EndPoints.Service.Args;
    using Map.EndPoints.Service.Interfaces;
    using Map.EndPoints.Service.Models;

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
                var result = await this.ms.LocationService.GetLocationsAsync(13, DateTime.Now.AddDays(-1), DateTime.Now);
                Assert.IsTrue(result != null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public async Task BrowseRoute()
        {
            try
            {
                var result = await this.ms.ReportService.BrowseRoute(13, DateTime.Now.AddDays(-1), DateTime.Now);
                Assert.IsTrue(result != null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public async Task InsertLocationAsync()
        {
            try
            {

                var param = new AddLocationArg
                {
                    Angle = 240,
                    Elements = new List<LocationElement> { new LocationElement { Id = 1, Value = 100 } }
                };

                await this.ms.LocationService.CreateAsync(param).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Assert.Fail(e.Message);
            }
        }
    }
}
