using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages.Admin
{
    public class TwitterAdminModel : ViewModelBase
    {

        public List<Tweet> Tweets { get; set; }


        public TwitterAdminModel(IAuthService authService, IMetaTagService imts, IDataService ds) : base(authService, imts, ds)
        {

        }

        public async Task<IActionResult> OnGetAsync()
        {
            string Response = await DS.GetAsync("Twitter/GetAll");

            Tweets = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Tweet>>(Response);
            return Page();

        }
    }
}