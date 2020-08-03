using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace APONCoreWebsite.Pages
{
    public class LoginModel : ViewModelBase
    {
        HttpClient http { get; set; }
     

       // IAuthService AuthService { get; set; }

        IConfiguration configuration { get; set; }
  

        public LoginModel(HttpClient client,  IConfiguration Configuration, IAuthService authService): base (authService)
        {
            http = client;
          
            configuration = Configuration;
            //AuthService = authService;
        
        }
        [BindProperty]
        public SignUpUser LoginUser { get; set; }
        [BindProperty]
        public bool LogInSuccessful { get; set; }

        [BindProperty]
        public DateTime TokenExpDate { get; set; }

        [BindProperty]
        public bool UserSaved { get; set; }
       
        public UserReturnToken SavedURT { get; set; }

        public string LoginMessage { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            LoginUser.UserName = Request.Form["loginUsername"];
            LoginUser.Password = Request.Form["loginPassword"];

          UserReturnToken URT = await  myAuthService.Login(LoginUser);

            if (string.IsNullOrEmpty(URT.Message))
            {
                //User was logged in
                SavedURT = URT;
                LogInSuccessful = true;
                TokenExpDate = DateTime.Now.AddMilliseconds(URT.Expiration);
            }
            else
            {
                //User was not logged in, display message
                LoginMessage = URT.Message;
            }





            return Page();
        }
    }
}