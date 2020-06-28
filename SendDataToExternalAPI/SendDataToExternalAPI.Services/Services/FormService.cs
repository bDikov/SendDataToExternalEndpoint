using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using SendDataToExternalAPI.Services.ModelsDTO;
using SendDataToExternalAPI.Services.Services.Contracts;

namespace SendDataToExternalAPI.Services.Services
{
    public class FormService : IFormService
    {
        public Task<bool> SendForm(FormDTO data)
        {
         
            var externalApiURL = String.Format("https://us-central1-randomfails.cloudfunctions.net/submitEmail");

            var request = WebRequest.Create(externalApiURL);
            request.Method = "POST";
            request.ContentType = "application/json";

            var dataToSend = JsonSerializer.Serialize(data);
           
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(dataToSend);
                streamWriter.Flush();
                streamWriter.Close();

                try
                {
                    var response = request.GetResponse();
                    return Task.FromResult(true);
                }
                catch (Exception)
                {

                    return Task.FromResult(false);
                }

            }
          
        }
    }
}
