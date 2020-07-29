using APONCoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace APONCoreWebsite.Services
{
    public interface IDataService
    {


        public Task<HttpResponseMessage> PostAsync(object T, string Endpoint);

    }

    public class DataService : IDataService
    {
        public HttpClient http { get; set; }
        private IConfiguration configuration { get; set; }

        public DataService(HttpClient httpClient, IConfiguration Configuration)
        {
            http = httpClient;
            configuration = Configuration;
        }



        public async Task<HttpResponseMessage> PostAsync(object T, string Endpoint)
        {
            Uri Uri = new Uri(configuration.GetValue<string>("BaseUrl") + Endpoint);

            //Add JWT Token Header to Post Requests and Get Requests
            return await http.SendAsync(new HttpRequestMessage(HttpMethod.Post, Uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(T), Encoding.UTF8, "application/json")
            });






        }
    }
}
