using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages.Shared
{
    public class authBadgeModel : PageModel
    {
        public readonly ISession session;

        public authBadgeModel(IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor.HttpContext.Session;
        }

        public bool LoggedIn { get; set; }

        public int? UserID { get; set; }
        public string UserName { get; set; }
      
        public string UserImageURL { get; set; }

        public int? UserAuthLevel { get; set; }
        public void OnGet()
        {

            try
            {
                UserID = session.GetInt32("UserID");
                UserName = session.GetString("UserName");
                UserImageURL = session.GetString("UserImageURL");
                UserAuthLevel = session.GetInt32("UserAuthLevel");
                if (string.IsNullOrEmpty(UserName))
                {
                    LoggedIn = false;
                }
                else
                {
                    LoggedIn = true;
                }
            }
            catch (Exception ex)
            {
                LoggedIn = false;
            }
        }
    }
}