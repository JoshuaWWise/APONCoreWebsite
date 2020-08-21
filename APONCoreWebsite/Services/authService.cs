using APONCoreLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APONCoreWebsite.Services
{
    public interface IAuthService
    {
     
   

        public string Logout();

        public UserReturnToken getUser();

        public string getToken();

      


        public int getUserID();



        public void SaveUserSessionData(UserReturnToken URT);

        public void AutoLogin(UserReturnToken urt);

        public int getUserAuthLevel();

        public void autoLogout();



        public void SaveBrowserUserData();

     

        //TODO: Add to User Service, GetUserByName, GetUserByID, GetAllUsersList, SaveUserTheme

    }
    public class AuthService : IAuthService
    {
        ISession Session { get; set; }
        HttpClient http { get; set; }

   

       
        public AuthService(HttpContext context, HttpClient client)
        {
            Session = context.Session;
            http = client;
        
           
        }
        public void AutoLogin(UserReturnToken urt)
        {
            SaveUserSessionData(urt);
           // UIS.setUser(urt);
        }

        public void autoLogout()
        {
            throw new NotImplementedException();
        }

        public int getUserID()
        {
            int UID = Session.GetInt32("UserID") ?? -1;
            return UID;
        }

        public string getToken()
        {

            string token = Session.GetString("JWTToken");
            return token;
        }
        public UserReturnToken getUser()
        {

            UserReturnToken URT = new UserReturnToken();
      
            URT = new UserReturnToken(Session.GetString("JWTToken"), 10000, "", Session.GetInt32("UserAuthLevel").ToString(), Session.GetString("UserName"),
                Session.GetInt32("UserID") ?? -1, Session.GetString("UserImageURL"), Session.GetInt32("UserTheme") ?? 0);

            return URT;
        }
  

        public int getUserAuthLevel()
        {
            UserReturnToken urt = getUser();
            int authLevel = 5;

            if (urt.UserID != -1)
            {
                authLevel = int.Parse(urt.AuthLevel);
            }

            return authLevel;
            
        }

     


 

     


        public void SaveUserSessionData(UserReturnToken URT)
        {
            Session.SetInt32("UserID", URT.UserID);
            Session.SetString("UserImageURL", URT.imageURL);
            Session.SetString("UserName", URT.UserName);
            Session.SetInt32("UserAuthLevel", int.Parse(URT.AuthLevel));
            Session.SetInt32("UserTheme", URT.Theme);
            Session.SetString("JWTToken", URT.Token);
          
            

        }

        public string Logout()
        {
      

            try
            {
                Session.SetInt32("UserID", -1);
                Session.SetString("UserImageURL", "http://media.allportsopen.org/images/siteImages/login.png");
                Session.SetString("UserName", "Guest");
                Session.SetInt32("UserAuthLevel", 5);
                Session.SetInt32("UserTheme", 0);
                Session.SetString("JWTToken", "");
              
            }catch(Exception ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        public void SaveBrowserUserData()
        {
            throw new NotImplementedException();
        }

       
    }
}
