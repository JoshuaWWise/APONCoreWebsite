using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileProviders;

namespace APONCoreWebsite.Pages.Handlers
{
    public class SaveHandlerModel : ViewModelBase
    {
        [BindProperty]
        public int ID { get; set; }
        public SaveHandlerModel(IAuthService authService, IMetaTagService imts, IDataService ds) : base(authService, imts, ds)
        {

        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostSplashNewsAsync()
        {
            SplashNews SN = new SplashNews();
            SN.EndDate = DateTime.Parse(Request.Form["EndDate"]);
            SN.StartDate = DateTime.Parse(Request.Form["StartDate"]);
            SN.SplashNewsID = int.Parse(Request.Form["currentSplashID"]);
            SN.LinkID = int.Parse(Request.Form["currentSplashLinkID"]);
            SN.PostURL = Request.Form["currentSplashPostURL"];
            SN.Headline = Request.Form["headline"];
            SN.ImageURL = Request.Form["imgURL"];
          

            HttpResponseMessage result = await DS.PutAsync(SN, "SplashNews/UpdateSplashnews");
          
            return new JsonResult(result.StatusCode.ToString());


        }

        public async Task<IActionResult> OnPostDelSplashNewsAsync()
        {
            if (ID == 0)
            {
                return new JsonResult(null);
            }

            HttpResponseMessage result = await DS.DeleteAsync("SplashNews/DeleteSplashNews/" + ID);

            return new JsonResult(result.StatusCode.ToString());
        }

        public async Task<IActionResult> OnPostNewEpisodeAsync()
        {
            bool epIsAffiliate = false;
            IFormFile UploadFile;
            EpisodeWithTags Episode = new EpisodeWithTags();

            List<Tag> Tags = new List<Tag>();

            Episode E = new Episode();
            E.SeriesID = int.Parse(Request.Form["seriesID"]);
            if (!string.IsNullOrEmpty(Request.Form["affiliateChckbx"]) && Request.Form["affiliateChckbx"] == "on")
            {
                epIsAffiliate = true;
                E.Local = false;
                E.FileURL = Request.Form["affiliateFileURL"];
                E.NonLocalType = 1;
            }
            else
           
            {
                UploadFile = Request.Form.Files[0];
                string s = UploadFile.Name;
                E.FileURL = UploadFile.FileName;
                E.Local = true;
                E.NonLocalType = 3;

                HttpResponseMessage UploadResponse = await DS.PostImageAsync(UploadFile, "Episodes/ULE/" + E.SeriesID);
                if (UploadResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                  //RETURN Failure
                    return Page();
                }
            }
      

          

            E.Description = Request.Form["shortDescription"];
            E.EpImageURL = Request.Form["imgURLtextbox"];
            E.Keywords = Request.Form["keywords"];
            E.PostedByUserID = myAuthService.getUserID();
           
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



            return new JsonResult(message.StatusCode.ToString());

        }


    }
}