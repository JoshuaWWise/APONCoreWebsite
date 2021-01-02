using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using APONCoreLibrary.Models;
using APONCoreLibrary.ReturnModels;
namespace APONCoreWebsite.Pages
{
    public class TestModel : ViewModelBase
    {
        public ForumWithPosts FWP { get; set; }
        public TestModel(IDataService ds, IAuthService authService, IMetaTagService imts) : base(authService, imts, ds)
        {
            FWP = new ForumWithPosts();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            int ForumID = 1112;
            string result = await DS.GetAsync("Forum/GetForumAndPostsByID/" + ForumID);
            FWP = Newtonsoft.Json.JsonConvert.DeserializeObject<ForumWithPosts>(result);

            FWP.CurrentUserID = myAuthService.getUserID();


            return Page();

        }
    }
}
