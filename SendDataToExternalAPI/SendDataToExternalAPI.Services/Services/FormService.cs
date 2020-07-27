using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SendDataToExternalAPI.Services.ModelsDTO;
using SendDataToExternalAPI.Services.Services.Contracts;

namespace SendDataToExternalAPI.Services.Services
{
    public class FormService : IFormService
    {
        private readonly IHttpClientFactory _clientFactory;

        public FormService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<bool> SendForm(FormDTO data)
        {

            var httpClient = _clientFactory.CreateClient("HttpClient");
            httpClient.Timeout = System.TimeSpan.FromSeconds(275);
            var dataToSend = JsonSerializer.Serialize(data);

            using (var content = new StringContent(dataToSend, Encoding.UTF8, "application/json"))
            {
                try
                {

                    var result = await httpClient.PostAsync($"https://us-central1-randomfails.cloudfunctions.net/submitEmail", content);

                    // The call was a success

                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }

                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
