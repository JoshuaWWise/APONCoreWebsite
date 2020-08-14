using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    public class FeatureModel : ViewModelBase
    {
        [BindProperty(SupportsGet = true)]
        public int FeatureID { get; set; }

        private IDataService DS { get; set; }

        public NewsWithTags Feature { get; set; }

        public int CurrentUserID {get; set;}

        public IUserInfoService UIS { get; set; }

        public FeatureModel(IAuthService authService, IMetaTagService imts, IDataService ds, IUserInfoService uis) : base(authService, imts)
        {
            this.DS = ds;
            this.UIS = uis;
        }
    
        public async Task<IActionResult> OnGetAsync()
        {
            if (FeatureID == 0)
            {
                return Page();
            }
            CurrentUserID = UIS.getUser().UserID;
            string response = await DS.GetAsync("News/GetNewsItemWTags/" + FeatureID);
            Feature = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsWithTags>(response);
            Feature.News.LongText = Feature.News.LongText.Replace("<img", "<img style='width:100%'");
            return Page();
        }
    }
}