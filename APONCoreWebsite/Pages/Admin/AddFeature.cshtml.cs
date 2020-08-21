using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.Shared;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages.Admin
{
    public class AddFeatureModel : ViewModelBase
    {

        public AddFeatureModel(IAuthService authService,  IMetaTagService imts, ITagService ts, IDataService ds) : base(authService, imts, ds)
        {
            this.tagService = ts;
         
        }

        [BindProperty]
        public bool FeatureSaved { get; set; }

        [BindProperty]
        public bool AttemptedSave { get; set; }

  
        public ITagService tagService { get; set; }



        public _tagConsoleModel TCM { get; set; }

        [BindProperty(SupportsGet = true)]
        public int FeatureID { get; set; }

        public NewsWithTags Feature { get; set; }

        public bool PageNotFound { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            AttemptedSave = false;
            TCM = new _tagConsoleModel(tagService);
            TCM.Tags = await tagService.GetTags(false);

            if (int.Parse(myAuthService.getUser().AuthLevel) < 4)
            {
                if (FeatureID == 0)
                {
                    Feature = new NewsWithTags();
                    Feature.News = new News();
                    Feature.tags = new List<Tag>();
                    Feature.News.PostDate = DateTime.Now;
                }
                else
                {
                    string result = await DS.GetAsync("news/GetNewsItemWTags/" + FeatureID);

                    if (result.ToLower().Contains("page not found"))
                    {
                        PageNotFound = true;
                        return Page();
                    }
                    Feature = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsWithTags>(result);

                    ProcessTagsForTCM();
                }
                //Otherwise Get the feature to be populated in the form.


                if (FeatureID == 0)
                {
                    return Page();
                }

            }

            return Page();
        }


        private void ProcessTagsForTCM()
        {

            string tags = "";
            string indicies = "";
            for (int i = 0; i < Feature.tags.Count; i++)
            {

                int index = TCM.Tags.FindIndex(a => a.TagID == Feature.tags[i].TagID);

                if (!string.IsNullOrEmpty(TCM.Tags[index].Name))
                {
                    if (i == 0)
                    {
                        tags = Feature.tags[i].TagID.ToString();
                        if (index != -1)
                        {
                            indicies = index.ToString();
                        }

                    }
                    else
                    {
                        tags += "," + Feature.tags[i].TagID.ToString();
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

        public async Task<IActionResult> OnPostFeatureAsync()
        {
            AttemptedSave = true;

            Feature = new NewsWithTags();
            Feature.tags = new List<Tag>();
            Feature.News = new News();


            Feature.News.Headline = Request.Form["headline"];
            Feature.News.LongText = Request.Form["tinymcetextarea"];
            Feature.News.Text = Request.Form["shortDescription"];
            Feature.News.PostDate = DateTime.Parse(Request.Form["myDatePicker"]);
            Feature.News.TempTweet = Request.Form["addToTweet"];
            Feature.News.ImageURL = Request.Form["smallImageInput"];
            Feature.News.SplashImageURL = Request.Form["splashImageInput"];

            Feature.News.AuthorID = myAuthService.getUserID();
            //If the ID is 0, add them.
            Feature.News.Status = 4;


            //AddTags
            string Tags = Request.Form["selectedTagIDs"];
            if (Tags.Length > 0)
            {
                if (Tags.Substring(0, 1) == ",")
                {
                    Tags = Tags.Substring(1, Tags.Length - 1);
                }
                string splitter = ",";
                string[] taglist = Tags.Split(splitter);

                for (int i = 0; i < taglist.Length; i++)
                {
                   
                    if (taglist[i] != "")
                    {
                        Tag t = new Tag();
                        t.TagID = int.Parse(taglist[i]);
                        Feature.tags.Add(t);
                    }
                }
            }

            HttpResponseMessage Response;

            if (FeatureID == 0)
            {
                Response = await DS.PostAsync(Feature, "news/addnews");
            }
            else
            {
                Feature.News.NewsID = FeatureID;
                Response = await DS.PutAsync(Feature, "news/UpdateNews");
            }

            if (Response.Content.ToString().ToLower().Contains("found"))
            {
                FeatureSaved = false;

            }
            else
            {
                AttemptedSave = true;
                FeatureSaved = true;
            }


            TCM = new _tagConsoleModel(tagService);
            TCM.Tags = await tagService.GetTags(false);
            ProcessTagsForTCM();
            //IF the ID is not 0, update them
            FeatureSaved = true;
            return Page();
        }


    }
}