using Map.EndPoints.Service.Interfaces;
using Map.EndPoints.Service.Models;
using Services.Core;
using Services.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Map.EndPoints.Service.SubServices
{
    internal class AreaService : AbstractSubService, IAreaService
    {
        /// <summary>Initializes a new instance of the <see cref="AreaService" /> class.</summary>
        /// <param name="apiService">The API service.</param>
        /// <param name="site">The site.</param>
        /// <param name="root">The root.</param>
        public AreaService(IApiCaller apiService, ApiSite site, string root) : base(apiService, site, root)
        {
        }

        /// <summary>Gets all asynchronous.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<List<Area>> GetAllAsync()
        {
            var param = new RequestParameters { Path = $"{RootUrl}/GetAll" };
            var response = await ApiService.GetAsync(param).ConfigureAwait(false);
            return await ParseAsync<List<Area>>(response).ConfigureAwait(false);
        }
    }
}
