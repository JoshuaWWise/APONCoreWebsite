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
    public class EditSplashNewsModel : ViewModelBase
    {
        public List<SplashNews> SplashNewsList { get; set; }

        public EditSplashNewsModel(IAuthService authService, IMetaTagService imts, IDataService ds, IUserInfoService iuis) : base(authService, imts, ds, iuis)
        {

        }
        public async Task<IActionResult> OnGetAsync()
        {
            string Response = await DS.GetAsync("SplashNews");

            SplashNewsList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SplashNews>>(Response);

            return Page();
        }
    }
}