using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    public class FeaturesModel : ViewModelBase
    {
        private IDataService DS { get; set; }

        [BindProperty]
       public List<News> News { get; set; }

        
        public FeaturesModel(IAuthService authService, IMetaTagService imts, IDataService ds): base(authService, imts)
        {
            this.DS = ds;

        }

        public async Task<IActionResult> OnGetAsync()
        {
            string response = await DS.GetAsync("news/GetNewsList");
            News = Newtonsoft.Json.JsonConvert.DeserializeObject<List<News>>(response);




            return Page();


        }
    }
}