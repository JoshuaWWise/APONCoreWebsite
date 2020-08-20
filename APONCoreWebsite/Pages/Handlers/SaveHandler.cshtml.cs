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

namespace APONCoreWebsite.Pages.Handlers
{
    public class SaveHandlerModel : ViewModelBase
    {
        [BindProperty]
        public int ID { get; set; }
        public SaveHandlerModel(IAuthService authService, IMetaTagService imts, IDataService ds, IUserInfoService iuis) : base(authService, imts, ds, iuis)
        {

        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostSplashNewsAsync()
        {
            SplashNews SN = new SplashNews();
            SN.EndDate = DateTime.Parse(Request.Form["EndDate"]);
            SN.StartDate = DateTime.Parse(Request.Form["StartDate"]);
            SN.SplashNewsID = int.Parse(Request.Form["currentSplashID"]);
            SN.LinkID = int.Parse(Request.Form["currentSplashLinkID"]);
            SN.PostURL = Request.Form["currentSplashPostURL"];
            SN.Headline = Request.Form["headline"];
            SN.ImageURL = Request.Form["imgURL"];
          

            HttpResponseMessage result = await DS.PutAsync(SN, "SplashNews/UpdateSplashnews");
          
            return new JsonResult(result.StatusCode.ToString());


        }

        public async Task<IActionResult> OnPostDelSplashNewsAsync()
        {
            if (ID == 0)
            {
                return new JsonResult(null);
            }

            HttpResponseMessage result = await DS.DeleteAsync("SplashNews/DeleteSplashNews/" + ID);

            return new JsonResult(result.StatusCode.ToString());
        }




    }
}