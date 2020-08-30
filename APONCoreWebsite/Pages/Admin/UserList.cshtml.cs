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

namespace APONCoreWebsite.Pages.Admin
{
    public class UserListModel : ViewModelBase
    {
public        List<User> Users { get; set; }

  

        public UserListModel(IAuthService authService, IMetaTagService imts, ITagService ts, IDataService ds) : base(authService, imts, ds)
        {
            

        }

        public async Task<IActionResult> OnGetAsync()
        {
          
            if (UserAuthLevel > 2)
            {
                return Page();
            }

            string response = await DS.GetAsync("User/GetAllUsersList");
            Users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(response);

            return Page();

        }
    }
}