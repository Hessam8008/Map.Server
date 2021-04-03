using System;
using System.Collections.Generic;
using System.Text;

namespace Map.EndPoints.Service.Test
{
    using System.Linq;

    using Services.Core;
    using Services.Core.Interfaces;

    internal class TestApiConfiguration : IApiConfiguration
    {
        public void Load()
        {
            /*
             * server: http://10.10.1.34:3344/api
             * local : http://localhost:2348/api
             */
            this.ApiSites = new List<ApiSite> { new ApiSite("Map.Api", "http://localhost:2348/api") };
        }

        public void Save()
        {
            // Ignore
        }

        public ApiSite Find(Func<ApiSite, bool> expression)
        {
            return this.ApiSites.First(expression);
        }

        public ApiSite FindByTitle(string urlTitle)
        {
            return this.ApiSites.First(x => x.Title.Equals(urlTitle));
        }

        public List<ApiSite> ApiSites { get; set; }
    }
}
