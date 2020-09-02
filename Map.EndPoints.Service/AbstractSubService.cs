namespace Map.EndPoints.Service
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Services.Core;
    using Services.Core.Interfaces;

    internal abstract class AbstractSubService
    {
        protected readonly string RootUrl;
        protected readonly IApiCaller ApiService;
        protected readonly ApiSite Site;

        protected AbstractSubService(IApiCaller apiService, ApiSite site, string root)
        {
            this.ApiService = apiService;
            this.Site = site;
            this.RootUrl = site.UrlAddress + "/" + root;
        }

        protected T Parse<T>(string json) => JsonConvert.DeserializeObject<T>(json);

        protected async Task ParseAsync(HttpResponseMessage message)
            => await CheckResponse(message).ConfigureAwait(false);

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
                //var content = msg.Content;
                //var details = "N/A";
                //if (content != null)
                //    details = await msg.Content.ReadAsStringAsync()
                //                  .ConfigureAwait(false);

                //if ((int)msg.StatusCode == 499)// خطای داخلی هندل شده توسط سرویس دهنده
                //{
                //    var handledError = JsonConvert.DeserializeObject<HandledServerError>(details);
                //    if (handledError != null)
                //        throw new ApiException(handledError.Code, handledError.Message);
                //}
                //else // خطای هندل نشده توسط سرویس دهنده
                throw new ApiException(code, reason);
            }
        }
    }
}