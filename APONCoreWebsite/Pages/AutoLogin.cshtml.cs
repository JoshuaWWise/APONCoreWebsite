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



 
        public AutoLoginModel( IAuthService authService, IDataService ds, IMetaTagService imts) : base(authService, imts, ds)
        {
      
        }
        public JsonResult OnGet()
        {
            JsonResult js = new JsonResult("This is a value");
            return js;
        }

        public void OnPostLogout()
        {
            string s = myAuthService.Logout();
            myAuthService.Logout();
            Response.Redirect("/");
        }

        public IActionResult OnPostURT([FromBody] object JwtValue)
        {
            if (JwtValue != null)
            {
                string j = JwtValue.ToString();
              
                URT = new UserReturnToken(j);
            
                myAuthService.SaveUserSessionData(URT);
               
            }
            else
            {
                myAuthService.Logout();
            }
            return Page();
        }
    }


}