using Newtonsoft.Json;
using SM.Models;
using SM.Service.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace SM.Service.Controller
{
    internal class ApiController
    {
        HttpClient client = new HttpClient();

        Uri url;
        String authToken;

        public ApiController()
        {
            url = new Uri(ConfigurationManager.AppSettings["api"]);
            authToken = ConfigurationManager.AppSettings["auth_token"];
        }

        public async Task<List<Module>> GetServicesAsync()
        {
            HttpRequestMessage hrp = new HttpRequestMessage(HttpMethod.Get, url.AbsoluteUri + "Modules");
            hrp.Headers.Add("auth_token", authToken);

            HttpResponseMessage hrm = await client.SendAsync(hrp);
            // TODO Status prüfen, ob 200
            if(hrm.StatusCode == System.Net.HttpStatusCode.OK)
                 return JsonConvert.DeserializeObject<List<Module>>(await hrm.Content.ReadAsStringAsync());

            return null;
        }

        public List<Module> GetModules()
        {
            var getModules = this.GetServicesAsync();
            getModules.Wait();
            return getModules.Result;
        }

        public async Task<Byte[]> GetFileAsync(Module moduleService)
        {
            HttpRequestMessage hrp = new HttpRequestMessage(HttpMethod.Get, url.AbsoluteUri + $"{moduleService.Module_ID}/versions/dl");
            hrp.Headers.Add("auth_token", authToken);

            HttpResponseMessage hrm = await client.SendAsync(hrp);
            if(hrm.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return await hrm.Content.ReadAsByteArrayAsync(); ;
            }

            String error = await hrm.Content.ReadAsStringAsync();
            Console.WriteLine(error);

            return null;
        }
        public Byte[] GetFile(Module moduleService)
        {
            var getModules = this.GetFileAsync(moduleService);
            getModules.Wait();
            return getModules.Result;
        }
    }
}