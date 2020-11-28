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
    public class EditSeriesModel : ViewModelBase
    {
        public ITagService tagService { get; set; }
  

        [BindProperty(SupportsGet = true)]
        public int SeriesID { get; set; }

        public APONCoreLibrary.Models.Series Series { get; set; }


        [BindProperty]
        public string SaveStatus { get; set; }



        public string ResponseMessage { get; set; }

        public EditSeriesModel(IAuthService authService, IMetaTagService imts, ITagService ts, IDataService ds) : base(authService, imts, ds)
        {
            this.tagService = ts;

            Series = new APONCoreLibrary.Models.Series();
          
        }
        public async Task<IActionResult> OnGetAsync()
        {

            //First, see if the user can edit the series

            string response = await DS.GetAsync("Series/GetEditSeriesForUser/" + SeriesID);
            Series = Newtonsoft.Json.JsonConvert.DeserializeObject<APONCoreLibrary.Models.Series>(response);

            
            SaveStatus = "NotSaved";

            //Populate TCM

         


            return Page();

        }

        public async Task<IActionResult> OnPostSeriesAsync()
        {

            string response = await DS.GetAsync("Series/GetEditSeriesForUser/" + SeriesID);
            Series = Newtonsoft.Json.JsonConvert.DeserializeObject<APONCoreLibrary.Models.Series>(response);


            Series.Name = Request.Form["seriesName"];
            Series.Subtitle = Request.Form["Subtitle"];
            Series.Description = Request.Form["Description"];
            Series.ShortDescription = Request.Form["ShortDescription"];
            Series.Authors = Request.Form["Authors"];
            Series.Keywords = Request.Form["DefaultKeywords"];
            Series.ShowDate = int.Parse(Request.Form["ShowDate"]);
            Series.CategoryList = Request.Form["Categories"];
            Series.ITunes = Request.Form["iTunesLink"];
            Series.Spotify = Request.Form["SpotifyLink"];
            Series.ImageURL = Request.Form["imgURLtextbox"];
            Series.SplashImageURL = Request.Form["splashImageInput"];
            Series.Email = Request.Form["Email"];
            Series.TwitterFeedURL = Request.Form["Twitter"];
            Series.Copyright = Request.Form["Copyright"];




            HttpResponseMessage message = await DS.PutAsync(Series, "Series/UpdateSeries/" + Series.SeriesID);

            switch (message.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    SaveStatus = "Saved";
                    
                    break;
                case System.Net.HttpStatusCode.Forbidden:
                    SaveStatus = "Forbidden";
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    SaveStatus = "Unauthorized";
                    break;
                case System.Net.HttpStatusCode.InternalServerError:
                    SaveStatus = "Error";
                    break;
            }



            ////Refresh TCM

            //TCM = new _tagConsoleModel(tagService)
            //{
            //    Tags = await tagService.GetTags(false)
            //};
            return Page();
        }
    }
}