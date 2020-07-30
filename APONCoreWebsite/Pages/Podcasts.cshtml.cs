using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    public class PodcastModel : ViewModelBase
    {

        private HttpClient _client;


        public List<Series> Series { get; set; }


        public PodcastModel(HttpClient client, IAuthService authService): base (authService)
        {
            _client = client;
        }

        public async Task<IActionResult> OnGetAsync()
        {

            var result = await _client.GetStringAsync("https://api.allportsopen.org/api/Series");
            Series = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Series>>(result);

            return Page();
        }
    }
}