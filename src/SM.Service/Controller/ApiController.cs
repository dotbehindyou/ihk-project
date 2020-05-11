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

        Uri apiUrl;
        String authToken;

        public HttpRequestMessage GetHttpRequest(HttpMethod method, String subUrl)
        {
            HttpRequestMessage hrp = new HttpRequestMessage(method, new Uri(apiUrl, subUrl));
            hrp.Headers.Add("auth_token", authToken);
            return hrp;
        }

        public ApiController()
        {
            apiUrl = new Uri(ConfigurationManager.AppSettings["api"]);
            authToken = ConfigurationManager.AppSettings["auth_token"];
        }

        public async Task<List<Module>> GetModulesAsync()
        {
            HttpRequestMessage hrp = this.GetHttpRequest(HttpMethod.Get, "Modules");

            HttpResponseMessage hrm = await client.SendAsync(hrp);

            if(hrm.StatusCode == System.Net.HttpStatusCode.OK)
                 return JsonConvert.DeserializeObject<List<Module>>(await hrm.Content.ReadAsStringAsync());

            return null;
        }

        public List<Module> GetModules()
        {
            var getModules = this.GetModulesAsync();
            getModules.Wait();
            return getModules.Result;
        }

        public async Task<Boolean> SendStatusAsync(Models.Service service)
        {
            HttpRequestMessage hrp = this.GetHttpRequest(HttpMethod.Put, "Modules");
            service.Module.Status = service.Status.ToString();
            String moduleJson = JsonConvert.SerializeObject(service.Module);
            hrp.Content = new StringContent(moduleJson, Encoding.UTF8, "application/json");

            HttpResponseMessage hrm = await client.SendAsync(hrp);

            return hrm.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public Boolean SendStatus(Models.Service service)
        {
            var getModules = this.SendStatusAsync(service);
            getModules.Wait();
            return getModules.Result;
        }

        public async Task<Boolean> SendRemoveAsync(Models.Service service)
        {
            HttpRequestMessage hrp = this.GetHttpRequest(HttpMethod.Delete, $"Modules/{service.Module.Module_ID}");

            HttpResponseMessage hrm = await client.SendAsync(hrp);

            return hrm.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public Boolean SendRemove(Models.Service service)
        {
            var getModules = this.SendRemoveAsync(service);
            getModules.Wait();
            return getModules.Result;
        }

        public async Task<Byte[]> GetFileAsync(Module moduleService)
        {
            HttpRequestMessage hrp = this.GetHttpRequest(HttpMethod.Get, $"{moduleService.Module_ID}/versions/dl");

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