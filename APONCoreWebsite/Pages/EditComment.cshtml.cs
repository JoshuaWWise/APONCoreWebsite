using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using System.Net.Http;

namespace APONCoreWebsite.Pages
{
    public class EditCommentModel : ViewModelBase
    {
        [BindProperty(SupportsGet = true)]
        public int ForumPostID { get; set; }
        public ForumPost FP { get; set; }
        public string ReturnURL { get; set; }

        public string Mode { get; set; }
        public EditCommentModel(IDataService ds, IAuthService authService, IMetaTagService imts) : base(authService, imts, ds)
        {

        }

        public async Task<IActionResult> OnGetAsync()
        {
            Mode = "Edit";
            ReturnURL = Request.Headers["Referer"].ToString();
            string result = await DS.GetAsync("Forum/GetForumPost/" + ForumPostID);
            FP = Newtonsoft.Json.JsonConvert.DeserializeObject<ForumPost>(result);

            return Page();
        }

        public async Task<IActionResult> OnPostCommentAsync()
        {
            string result = await DS.GetAsync("Forum/GetForumPost/" + ForumPostID);
            FP = Newtonsoft.Json.JsonConvert.DeserializeObject<ForumPost>(result);

            FP.Text = Request.Form["tinyMCETextArea"];

            HttpResponseMessage Response = await DS.PutAsync(FP, "Forum/UpdateForumPost");

            ReturnURL = Request.Form["returnURL"];
            Mode = "Saved";
            return Page();
        }
    }
}
