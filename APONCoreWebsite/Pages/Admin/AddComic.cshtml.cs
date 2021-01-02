using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.Shared;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using System.Net.Http;

namespace APONCoreWebsite.Pages.Admin
{
    public class AddComicModel : ViewModelBase
    {
        public ITagService tagService { get; set; }

        public string ResponseMessage { get; set; }
        public bool EditorLoggedIn { get; set; }
        public ComicStrip CS { get; set; }

        public int SavedStripID { get; set; }
        bool Saved { get; set; }

        public AddComicModel(IAuthService authService, IMetaTagService imts, ITagService ts, IDataService ds) : base(authService, imts, ds)
        {
            this.tagService = ts;

        }

        public async Task<IActionResult> OnGet()
        {
            string response = await DS.GetAsync("user/IsEditorLoggedIn");
            EditorLoggedIn = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(response);
            SavedStripID = -1;
            return Page();
        }

        public async Task<IActionResult> OnPostComicAsync()
        {
            string response = await DS.GetAsync("user/IsEditorLoggedIn");
            EditorLoggedIn = Newtonsoft.Json.JsonConvert.DeserializeObject<bool>(response);

            if (EditorLoggedIn)
            {
                ComicStrip CS = new ComicStrip();
                CS.DefaultHeight = int.Parse(Request.Form["DefaultHeight"]);
                CS.DefaultWidth = int.Parse(Request.Form["DefaultWidth"]);
                CS.Description = Request.Form["Description"];
                CS.Name = Request.Form["Name"];
                CS.Enabled = (int.Parse(Request.Form["enabledStatus"]) != 0);
                CS.ImageURL = Request.Form["imgURLtextbox"];

                HttpResponseMessage message = await DS.PostAsync(CS, "comic/AddComic");
                if (message.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ResponseMessage = "There was a problem uploading the comic.";
                }
                else
                {
                    string newEpID = await DS.GetAsync("comic/GetLatestComicID");
                    SavedStripID = Newtonsoft.Json.JsonConvert.DeserializeObject<int>(newEpID);

                }
            }

            return Page();
        }
    }
}
