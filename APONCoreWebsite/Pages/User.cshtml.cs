using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages.Shared
{
    public class UserModel : ViewModelBase
    {
        [BindProperty(SupportsGet = true)]
        public int ID { get; set; }

        IAuthService AuthService { get; set; }

        IDataService DS { get; set; }

        IUserInfoService IUS { get; set; }

        [BindProperty]
       public  UserBriefWithBadges UBWB { get; set; }

        [BindProperty]
        public bool isProfileUser { get; set; }

        public UserModel(IAuthService authService, IDataService ds, IUserInfoService userInfoService, IMetaTagService imts) : base(authService, imts)
        {
            AuthService = AuthService;
            DS = ds;
            IUS = userInfoService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string result = await DS.GetAsync("User/GetUserByID/" + ID);
            UBWB = Newtonsoft.Json.JsonConvert.DeserializeObject<UserBriefWithBadges>(result);

            if (UBWB.User.UserID == IUS.getUserID())
            {
                isProfileUser = true;
            }
            return Page();
        }
    }
}