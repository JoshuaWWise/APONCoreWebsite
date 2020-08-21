using APONCoreLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APONCoreWebsite.Services
{
    public interface IDataService
    {


        public Task<HttpResponseMessage> PostAsync(object T, string Endpoint);

        public Task<HttpResponseMessage> PutAsync(object T, string Endpoint);

        public Task<HttpResponseMessage> DeleteAsync(string Endpoint);

        public Task<HttpResponseMessage> PostImageAsync(IFormFile T, string Endpoint);

        public Task<string> GetAsync(string Endpoint);

    

    }

    public class DataService : IDataService
    {
        public HttpClient http { get; set; }
        private IConfiguration configuration { get; set; }

        private IAuthService myAuthService;
      

     

        public DataService(HttpClient httpClient, IConfiguration Configuration, IAuthService IAS)
        {
            http = httpClient;
            configuration = Configuration;
            myAuthService = IAS;
        }

        public async Task<string> GetAsync(string Endpoint)
        {

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", myAuthService.getToken());

            Uri Uri = new Uri(configuration.GetValue<string>("BaseUrl") + Endpoint);


            return await http.GetStringAsync(Uri);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string Endpoint)
        {

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", myAuthService.getToken());

            Uri Uri = new Uri(configuration.GetValue<string>("BaseUrl") + Endpoint);

            return await http.DeleteAsync(Uri);
           
        }

        public async Task<HttpResponseMessage> PostAsync(object T, string Endpoint)
        {
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", myAuthService.getToken());

            Uri Uri = new Uri(configuration.GetValue<string>("BaseUrl") + Endpoint);

            //Add JWT Token Header to Post Requests and Get Requests
            return await http.SendAsync(new HttpRequestMessage(HttpMethod.Post, Uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(T), Encoding.UTF8, "application/json")
            });

        }

        public async Task<HttpResponseMessage> PutAsync(object T, string Endpoint)
        {
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", myAuthService.getToken());

            Uri Uri = new Uri(configuration.GetValue<string>("BaseUrl") + Endpoint);

            //Add JWT Token Header to Post Requests and Get Requests
            return await http.SendAsync(new HttpRequestMessage(HttpMethod.Put, Uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(T), Encoding.UTF8, "application/json")
            });

        }

        public async Task<HttpResponseMessage> PostImageAsync(IFormFile T, string Endpoint)
        {
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", myAuthService.getToken());

            Uri Uri = new Uri(configuration.GetValue<string>("BaseUrl") + Endpoint);

            MultipartFormDataContent form = new MultipartFormDataContent();
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await T.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();

            }

            ByteArrayContent byteArrayContent = new ByteArrayContent(imageBytes);

            MultipartFormDataContent multipartContent = new MultipartFormDataContent();
            multipartContent.Add(byteArrayContent, T.ContentType, T.FileName);
            multipartContent.Headers.Add("Authoriazation", myAuthService.getToken());



            //Add JWT Token Header to Post Requests and Get Requests
            return await http.SendAsync(new HttpRequestMessage(HttpMethod.Post, Uri)
            {
                Content = multipartContent
                //Content = new FormUrlEncodedContent(new[]
                //{

                //})
            });

        }



    }
}
