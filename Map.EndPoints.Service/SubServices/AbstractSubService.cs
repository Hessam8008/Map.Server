namespace Map.EndPoints.Service.SubServices
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Services.Core;
    using Services.Core.Interfaces;

    internal abstract class AbstractSubService
    {
        protected readonly IApiCaller ApiService;

        protected readonly string RootUrl;

        protected readonly ApiSite Site;

        protected AbstractSubService(IApiCaller apiService, ApiSite site, string root)
        {
            this.ApiService = apiService;
            this.Site = site;
            this.RootUrl = site.UrlAddress + "/" + root;
        }

        protected T Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected async Task ParseAsync(HttpResponseMessage message)
        {
            await CheckResponse(message).ConfigureAwait(false);
        }

        protected async Task<T> ParseAsync<T>(HttpResponseMessage message)
        {
            await CheckResponse(message).ConfigureAwait(false);
            var json = await message.Content.ReadAsStringAsync().ConfigureAwait(false);
            return this.Parse<T>(json);
        }

        private static async Task CheckResponse(HttpResponseMessage msg)
        {
            if (!msg.IsSuccessStatusCode)
            {
                var reason = msg.ReasonPhrase;
                var code = msg.StatusCode;
                throw new ApiException(code, reason);
            }
        }
    }
}