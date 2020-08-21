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


        public LoginModel(HttpClient client, IConfiguration Configuration, IAuthService authService, IMetaTagService imts, IDataService ds) : base(authService, imts, ds)
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

        public string RegisterMessage { get; set; }
        public void OnGet()
        {

            IMTS.setParams("https://www.allportsopen.com/Login", "website", "Login", "Login Page", "https://media.allportsopen.org/Images/LogoSmWText.png");

        }

        public async Task<IActionResult> OnPostSignUpAsync()
        {

            LoginUser.UserName = Request.Form["registerUsername"];
            LoginUser.Email = Request.Form["registerEmail"];
            LoginUser.Password = Request.Form["registerPassword1"];
            SignUpUser SUU = new SignUpUser();
            SUU.Email = LoginUser.Email;
            SUU.UserName = LoginUser.UserName;
            SUU.Password = LoginUser.Password;
            UserReturnToken URT = new UserReturnToken();
            var signUpResponse = await DS.PostAsync(SUU, "user/SignUp");
            if (signUpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseJSON = await signUpResponse.Content.ReadAsStringAsync();

                URT = Newtonsoft.Json.JsonConvert.DeserializeObject<UserReturnToken>(responseJSON);
                if (string.IsNullOrEmpty(URT.Message))
                {

                    myAuthService.SaveUserSessionData(URT);

                }

            }

            if (string.IsNullOrEmpty(URT.Message))
            {
                //user is registerd and signed in, all is good.
                LogInSuccessful = true;
            }
            else
            {
                RegisterMessage = URT.Message;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostResetPasswordAsync()
        {
            LoginUser.Email = Request.Form["resetEmail"];
            HttpResponseMessage LoginResult = await DS.PostAsync(null, "user/GetPasswordResetLink/" + LoginUser.Email);


           
            return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            LoginUser.UserName = Request.Form["loginUsername"];
            LoginUser.Password = Request.Form["loginPassword"];

            UserReturnToken URT = new UserReturnToken();

            //-----------------------------------------------------------

            string result = await DS.GetAsync("User/InitiateLogin/" + LoginUser.UserName);
    
            URT.UserID = -1;
            if (result.Contains("200"))
            {


                HttpResponseMessage LoginResult = await DS.PostAsync(LoginUser, "user/authenticate");


                if (LoginResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseJSON = await LoginResult.Content.ReadAsStringAsync();

                    URT = Newtonsoft.Json.JsonConvert.DeserializeObject<UserReturnToken>(responseJSON);
                    if (string.IsNullOrEmpty(URT.Message))
                    {
                  

                       myAuthService.SaveUserSessionData(URT);
                       
                    }

                }



            }
            else
            {
                URT.Message = "No user with that name found";

            }


            //-----------------------------------------------------------

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
