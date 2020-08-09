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

        public Task<HttpResponseMessage> PostImageAsync(IFormFile T, string Endpoint);

        public Task<string> GetAsync(string Endpoint);

        public void SetAuthToken(string token);

    }

    public class DataService : IDataService
    {
        public HttpClient http { get; set; }
        private IConfiguration configuration { get; set; }

        private string token { get; set; }

        public void SetAuthToken(string token)
        {
            this.token = token;

        }

        public DataService(HttpClient httpClient, IConfiguration Configuration)
        {
            http = httpClient;
            configuration = Configuration;
        }

        public async Task<string> GetAsync(string Endpoint)
        {

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.token);

            Uri Uri = new Uri(configuration.GetValue<string>("BaseUrl") + Endpoint);


            return await http.GetStringAsync(Uri);
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

        public async Task<HttpResponseMessage> PostImageAsync(IFormFile T, string Endpoint)
        {
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
            multipartContent.Headers.Add("Authoriazation", this.token);

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.token);

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
