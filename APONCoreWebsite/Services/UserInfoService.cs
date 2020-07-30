using APONCoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APONCoreWebsite.Services
{
    public interface IUserInfoService
    {
      public  void setUser(UserReturnToken urt);
        public int getUserID();

        public UserReturnToken getUser();

        public void logoutUser();

    }
    public class UserInfoService: IUserInfoService
    {

        public UserInfoService()
        {
            logoutUser();
            

        }
        public UserReturnToken URT { get; set; }

        public void setUser(UserReturnToken urt)
        {
            URT = urt;
        }

        public void logoutUser()
        {
            URT = new UserReturnToken();
            URT.UserID = -1;
            URT.imageURL = "http://media.allportsopen.org/images/siteImages/login.png"; // actual http://media.allportsopen.org/images/siteImages/login.png
            URT.AuthLevel = "5";
            URT.UserName = "";
        }

        public int getUserID()
        {
            return URT.UserID;
        }

        public UserReturnToken getUser()
        {
            return URT;
        }

    }
}
