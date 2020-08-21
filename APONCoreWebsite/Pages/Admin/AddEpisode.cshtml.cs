using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.Shared;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    [DisableRequestSizeLimit]
    public class AddEpisodeModel : ViewModelBase
    {

        [BindProperty]
        public string EpisodeSaved { get; set; }

        [BindProperty]
        public bool AttemptedSave { get; set; }


        public ITagService tagService { get; set; }

   

        public _tagConsoleModel TCM { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SeriesID { get; set; }

        public APONCoreLibrary.Models.Series EpSeries { get; set; }

        public EpisodeWithTags Episode { get; set; }

        public bool Affiliate { get; set; }

        public bool PageNotFound { get; set; }

        [BindProperty]
        public IFormFile UploadFile { get; set; }

        public string ResponseMessage { get; set; }
        public AddEpisodeModel(IAuthService authService,  IMetaTagService imts, ITagService ts, IDataService ds) : base(authService, imts, ds)
        {
            this.tagService = ts;
          
          
            Episode = new EpisodeWithTags();
            Episode.episode = new Episode();
        }


        public async Task<IActionResult> OnGetAsync()
        {
            AttemptedSave = false;
            EpisodeSaved = "False";
            if (int.Parse(myAuthService.getUser().AuthLevel) < 4)
            {

                TCM = new _tagConsoleModel(tagService)
                {
                    Tags = await tagService.GetTags(false)
                };

                string Response = await DS.GetAsync("Series/" + SeriesID);

                if (Response.ToLower().Contains("could not find"))
                {
                    PageNotFound = true;
                    return Page();
                }

                EpSeries = Newtonsoft.Json.JsonConvert.DeserializeObject<APONCoreLibrary.Models.Series>(Response);

                int dayOfTheWeek = (int)DateTime.Now.DayOfWeek;
                DateTime D;

                if (EpSeries.ShowDate > dayOfTheWeek)
                {
                    D = DateTime.Now.AddDays(EpSeries.ShowDate - dayOfTheWeek);
                }
                else if (EpSeries.ShowDate < dayOfTheWeek)
                {
                    D = DateTime.Now.AddDays((EpSeries.ShowDate + 7) - dayOfTheWeek);
                }
                else
                {
                    D = DateTime.Now;
                    
                }
              
                Episode.episode.ShowDate = new DateTime(D.Year, D.Month, D.Day, EpSeries.DefaultHour - 2, 0, 0);
                Episode.episode.EpImageURL = EpSeries.ImageURL;
                Episode.episode.SplashImageURL = EpSeries.SplashImageURL;
                Episode.episode.Keywords = EpSeries.Keywords;
            }
            return Page();
        }



    
        
        public async Task<IActionResult> OnPostEpisodeAsync()
        {
            string s = UploadFile.Name;
            AttemptedSave = true;
            EpisodeSaved = "False";
            HttpResponseMessage UploadResponse = await DS.PostImageAsync(UploadFile, "Episodes/ULE/" + SeriesID);
            if (UploadResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ResponseMessage = "Error Uploading file: " + UploadResponse.StatusCode.ToString();
                AttemptedSave = true;
                return Page();
            }

            Episode = new EpisodeWithTags();
          
            List<Tag> Tags = new List<Tag>();

            Episode E = new Episode();

            E.FileURL = UploadFile.FileName;
            E.Description = Request.Form["shortDescription"];
            E.EpImageURL = Request.Form["imgURLtextbox"];
            E.Keywords = Request.Form["keywords"];
            E.PostedByUserID = myAuthService.getUserID();
            E.SeriesID = SeriesID;
            E.ShowDate = DateTime.Parse(Request.Form["myDatePicker"]);
            E.Size = Request.Form["epSize"];
            E.SplashImageURL = Request.Form["splashImageInput"];
            E.Time = Request.Form["epTime"];
            E.Title = Request.Form["episodeTitle"];
            E.TrackNumber = int.Parse(Request.Form["trackNumber"]);
            E.WebDescription = Request.Form["tinymcetextarea"];

            string TagList = Request.Form["selectedTagIDs"];
            if (TagList.Length > 0)
            {
                if (TagList.Substring(0, 1) == ",")
                {
                    TagList = TagList.Substring(1, TagList.Length - 1);
                }
                string splitter = ",";
                string[] taglistarray = TagList.Split(splitter);

                for (int i = 0; i < taglistarray.Length; i++)
                {

                    if (taglistarray[i] != "")
                    {
                        Tag t = new Tag();
                        t.TagID = int.Parse(taglistarray[i]);
                        Tags.Add(t);
                    }
                }
            }

            Episode.episode = E;
            Episode.tags = Tags;

            HttpResponseMessage message = await DS.PostAsync(Episode, "Episodes/AddEpisode");

            if (message.StatusCode != System.Net.HttpStatusCode.OK)
            {
                ResponseMessage = "There was a problem uploading the episode.";
            }

            //Populate to restore page

            string Response = await DS.GetAsync("Series/" + SeriesID);

        

            EpSeries = Newtonsoft.Json.JsonConvert.DeserializeObject<APONCoreLibrary.Models.Series>(Response);
            AttemptedSave = true;
            EpisodeSaved = "True";

        
            return Page();
        }
    }
}