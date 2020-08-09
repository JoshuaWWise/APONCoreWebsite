using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    public class AutoLoginModel : ViewModelBase
    {
        UserReturnToken URT { get; set; }



        public IDataService DS { get; set; }
        public IUserInfoService UIS { get; set; }
        public AutoLoginModel(IUserInfoService uis, IAuthService authService, IDataService ds, IMetaTagService imts) : base(authService, imts)
        {
            UIS = uis;
            DS = ds;
        }
        public JsonResult OnGet()
        {
            JsonResult js = new JsonResult("This is a value");
            return js;
        }

        public void OnPostLogout()
        {
            string s = myAuthService.Logout();
            UIS.logoutUser();
            Response.Redirect("/");
        }

        public IActionResult OnPostURT([FromBody] object JwtValue)
        {

            string j = JwtValue.ToString();
            if (j == null)
            {
                return Page();
            }
            URT = new UserReturnToken(j);
            DS.SetAuthToken(URT.Token);
            UIS.setUser(URT);
            
            return Page();
        }
    }


}