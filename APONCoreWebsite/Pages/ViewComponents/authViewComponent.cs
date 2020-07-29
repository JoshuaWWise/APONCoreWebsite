using APONCoreLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APONCoreWebsite.Pages.ViewComponents
{
    public class authViewComponent: ViewComponent
    {
    
        public authViewComponent()
        {
        
        }

        public bool LoggedIn { get; set; }

        public int UserID { get; set; }
        public string UserName { get; set; }

        public string UserImageURL { get; set; }

        public int? UserAuthLevel { get; set; }

        public IViewComponentResult Invoke()
        {

            UserID = HttpContext.Session.GetInt32("UserID") ?? -1;
            //UserName = session.GetString("UserName");
            //UserImageURL = session.GetString("UserImageURL");
            //UserAuthLevel = session.GetInt32("UserAuthLevel");
           

            //UserID = -1;
            UserName = "Josh";
            UserImageURL = "https://i.pinimg.com/originals/79/b3/9c/79b39c5356a4999120b39e33c2f71e16.gif";
            UserAuthLevel = 1;
            if (UserID == -1)
            {
                UserImageURL = "http://media.allportsopen.org/images/siteImages/login.png";
            }
            UserReturnToken URT = new UserReturnToken("", 0, "", UserAuthLevel.ToString(), UserName, UserID, UserImageURL,  1);
            if (string.IsNullOrEmpty(UserName))
            {
                LoggedIn = false;
                return null;
            }
            else
            {
                return View(URT);
            }
        }
      
    }
}
