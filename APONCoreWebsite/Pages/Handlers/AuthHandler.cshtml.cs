using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages.Handlers
{
    public class AuthHandlerModel : ViewModelBase
    {

        public AuthHandlerModel(IAuthService ias, IMetaTagService imts, IDataService ds ): base(ias, imts, ds)
        {

        }
        public void OnGet()
        {

        }

        public async Task<string> SignUp(string email, string username, string password)
        {

            SignUpUser SUU = new SignUpUser();
            SUU.Email = email.ToLower();
            SUU.UserName = username;
            SUU.Password = password;

            var signUpResponse = await DS.PostAsync(SUU, "user/SignUp");
            if (signUpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseJSON = await signUpResponse.Content.ReadAsStringAsync();

                UserReturnToken URT = Newtonsoft.Json.JsonConvert.DeserializeObject<UserReturnToken>(responseJSON);
                if (string.IsNullOrEmpty(URT.Message))
                {

                   myAuthService.SaveUserSessionData(URT);
                    return "";
                }

                else
                {
                    return URT.Message;
                }

            }

            return signUpResponse.Content.ToString();

        }
    }
}