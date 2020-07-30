using APONCoreLibrary.Models;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APONCoreWebsite.Pages.ViewModels
{
    public abstract class ViewModelBase : PageModel
    {
        public UserReturnToken myURT { get; set; }
        public IAuthService myAuthService { get; set; }

        public ViewModelBase(IAuthService authService)
        {
            myAuthService = authService;
        }

        public IActionResult OnPost()
        {
            try
            {
                string s = Request.Form["layoutJWT"].ToString();
                UserReturnToken URT = new UserReturnToken(s);

                myAuthService.AutoLogin(URT);
            }
            catch(Exception)
            {

            }

            return Page();

        }
    }
}
