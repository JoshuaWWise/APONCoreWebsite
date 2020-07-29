using APONCoreLibrary.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APONCoreWebsite.Services
{
    public interface IAuthService
    {
     
        public Task<UserReturnToken> Login(SignUpUser LoginUser);

        public void Logout();

        public bool SignUp(string email, string username, string password);

        public bool GetPassworResetLink(ResetPW resetPW);

        public bool HandleAuthentication(UserReturnToken URT);

        public bool AutoLogin();

        public int getUserAuthLevel();

        public void autoLogout();

        public string InitiateLogin(string UserName);

        public void SaveBrowserUserData();

        //TODO: Add to User Service, GetUserByName, GetUserByID, GetAllUsersList, SaveUserTheme

    }
    public class AuthService : IAuthService
    {
        ISession Session { get; set; }
        HttpClient http { get; set; }

        IDataService DS { get; set; }
        public AuthService(HttpContext context, HttpClient client, IDataService ds)
        {
            Session = context.Session;
            http = client;
            DS = ds;
        }
        public bool AutoLogin()
        {
            return false;
        }

        public void autoLogout()
        {
            throw new NotImplementedException();
        }

        public bool GetPassworResetLink(ResetPW resetPW)
        {
            throw new NotImplementedException();
        }

        public int getUserAuthLevel()
        {
            throw new NotImplementedException();
        }

        public bool HandleAuthentication(UserReturnToken URT)
        {
            throw new NotImplementedException();
        }

        public string InitiateLogin(string UserName)
        {
            throw new NotImplementedException();
        }

        public async Task<UserReturnToken> Login(SignUpUser LoginUser)
        {
            
            var result = await http.GetAsync("User/InitiateLogin/" + LoginUser.UserName);
            UserReturnToken URT = new UserReturnToken();
            URT.UserID = -1;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {


                var LoginResult = await DS.PostAsync(LoginUser, "user/authenticate");


                if (LoginResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var responseJSON = await LoginResult.Content.ReadAsStringAsync();

                   URT = Newtonsoft.Json.JsonConvert.DeserializeObject<UserReturnToken>(responseJSON);
                    if (!string.IsNullOrEmpty(URT.Message))
                    {
                        return URT;

                    }
                    else
                    {


                        Session.SetInt32("UserID", URT.UserID);
                        Session.SetString("UserImageURL", URT.imageURL);
                        Session.SetString("UserName", URT.UserName);
                        Session.SetInt32("UserAuthLevel", int.Parse(URT.AuthLevel));
                        Session.SetInt32("UserTheme", URT.Theme);
                        Session.SetString("JWTToken", URT.Token);
                        //Response.Redirect("/");
                    }

                }



            }
            else
            {
                URT.Message = "No user with that name found";

            }






            return URT;
        }

        public void Logout()
        {
          
            Session.Remove("UserID");
            Session.Remove("UserImageURL");
            Session.Remove("UserName");
            Session.Remove("UserAuthLevel");
            Session.Remove("UserTheme");
            Session.Remove("JWTToken");
            Session.Remove("JWTExp");
        }

        public void SaveBrowserUserData()
        {
            throw new NotImplementedException();
        }

        public bool SignUp(string email, string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
