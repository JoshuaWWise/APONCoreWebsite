using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.ReturnModels;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    public class AboutUsModel : ViewModelBase
    {
        [BindProperty]
        public AboutUsPageInfo Info { get; set; }
        public AboutUsModel(IAuthService authService, IUserInfoService iuis, IMetaTagService imts,  IDataService ds) : base(authService, imts, ds, iuis)
        {
            

        }
        public async Task<IActionResult> OnGetAsync()
        {

            string Response = await DS.GetAsync("AboutUs/GetAboutUsPageInfo");
            Info = Newtonsoft.Json.JsonConvert.DeserializeObject<AboutUsPageInfo>(Response);

            IMTS.setParams("https://www.allportsopen.com/AboutUs", "article", "About Us", "The All Ports Open Network Crew", "https://media.allportsopen.org/Images/LogoSmWText.png");

            return Page();
        }
    }
}