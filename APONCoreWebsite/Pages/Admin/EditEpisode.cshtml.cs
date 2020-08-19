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
using System.Security.Policy;
using Microsoft.AspNetCore.Http;

namespace APONCoreWebsite.Pages.Admin
{
    public class EditEpisodeModel : ViewModelBase
    {

        public IDataService DS { get; set; }
        public ITagService tagService { get; set; }

        public IUserInfoService IUIS;

        public _tagConsoleModel TCM { get; set; }

        [BindProperty(SupportsGet = true)]
        public int EpisodeID { get; set; }

        public APONCoreLibrary.Models.Series EpSeries { get; set; }

        public EpisodeWithTags Episode { get; set; }

        [BindProperty]
        public string SaveStatus { get; set; }



        public string ResponseMessage { get; set; }
        public EditEpisodeModel(IAuthService authService, IUserInfoService iuis, IMetaTagService imts, ITagService ts, IDataService ds) : base(authService, imts)
        {
            this.tagService = ts;
            this.IUIS = iuis;
            this.DS = ds;
            Episode = new EpisodeWithTags();
            Episode.episode = new Episode();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            //Get Episode
            string response = await DS.GetAsync("Episodes/GetEpisodePageData/" + EpisodeID);
            EpisodePageData EPD = Newtonsoft.Json.JsonConvert.DeserializeObject<EpisodePageData>(response);

            Episode = EPD.EpWithTag;
            SaveStatus = "NotSaved";

            //Populate TCM

            TCM = new _tagConsoleModel(tagService)
            {
                Tags = await tagService.GetTags(false)
            };

            ProcessTagsForTCM();

            return Page();

        }

        private void ProcessTagsForTCM()
        {

            string tags = "";
            string indicies = "";
            for (int i = 0; i < Episode.tags.Count; i++)
            {

                int index = TCM.Tags.FindIndex(a => a.TagID == Episode.tags[i].TagID);

                if (!string.IsNullOrEmpty(TCM.Tags[index].Name))
                {
                    if (i == 0)
                    {
                        tags = Episode.tags[i].TagID.ToString();
                        if (index != -1)
                        {
                            indicies = index.ToString();
                        }

                    }
                    else
                    {
                        tags += "," + Episode.tags[i].TagID.ToString();
                        if (index != -1)
                        {
                            indicies += "," + index.ToString();
                        }
                    }
                }

            }

            TCM.SelectedIDs = tags;
            TCM.SelectedIndicies = indicies;
        }

        public async Task<IActionResult> OnPostEpisodeAsync()
        {
            string response = await DS.GetAsync("Episodes/GetEpisodePageData/" + EpisodeID);
            EpisodePageData EPD = Newtonsoft.Json.JsonConvert.DeserializeObject<EpisodePageData>(response);


            //Get Episode from Form
            Episode E = EPD.EpWithTag.episode;
            E.EpImageURL = Request.Form["imageURL"];
            E.FileURL = Request.Form["fileURL"];
            E.Title = Request.Form["EpTitle"];
            E.ShowDate = DateTime.Parse(Request.Form["showDate"]);
            E.WebDescription = Request.Form["tinyMCETextArea"];
            E.Description = Request.Form["shortDescription"];
            E.EpisodeID = EpisodeID;

            List<Tag> Tags = new List<Tag>();

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

            HttpResponseMessage message = await DS.PutAsync(Episode, "Episodes/UpdateEpisode");

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



            //Refresh TCM

            TCM = new _tagConsoleModel(tagService)
            {
                Tags = await tagService.GetTags(false)
            };
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteEpisodeAsync()
        {
            HttpResponseMessage message = await DS.DeleteAsync("Episodes/" + EpisodeID);


            JsonResult result = new JsonResult(message.StatusCode.ToString());
            return result;
        }
    }
}