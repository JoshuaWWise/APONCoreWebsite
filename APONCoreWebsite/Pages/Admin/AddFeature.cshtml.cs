using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        public AddFeatureModel(IAuthService authService, IUserInfoService iuis, IMetaTagService imts, ITagService ts) : base(authService, imts)
        {
            tagService = ts;
            this.IUIS = iuis;
        }

        public bool FeatureSaved { get; set; }

        public ITagService tagService { get; set; }

        public IUserInfoService IUIS;

        public _tagConsoleModel TCM { get; set; }

        [BindProperty(SupportsGet = true)]
        public int FeatureID { get; set; }

        public News Feature { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (int.Parse(IUIS.getUser().AuthLevel) < 4)
            {
                Feature = new News();

                //Otherwise Get the feature to be populated in the form.
                TCM = new _tagConsoleModel(tagService);
                TCM.Tags = await tagService.GetTags(false);

                if (FeatureID == 0)
                {
                    return Page();
                }

            }

            return Page();
        }

        public async Task<IActionResult> OnPostFeatureAsync()
        {
            News N = new News();
            NewsWithTags NWT = new NewsWithTags();
            NWT.tags = new List<Tag>();

            N.Headline = Request.Form["headline"];
            N.LongText = Request.Form["tinymcetextarea"];
            N.Text = Request.Form["shortDescription"];
            N.PostDate = DateTime.Parse(Request.Form["myDatePicker"]);
            N.TempTweet = Request.Form["addToTweet"];
            N.ImageURL = Request.Form["smallImageInput"];
            N.SplashImageURL = Request.Form["splashImageInput"];

            N.AuthorID = IUIS.getUserID();
            //If the ID is 0, add them.
            N.Status = 2;

            NWT.News = N;

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
                    Tag t = new Tag();
                    t.TagID = int.Parse(taglist[i]);
                    NWT.tags.Add(t);
                }
            }


            TCM = new _tagConsoleModel(tagService);
            TCM.Tags = new List<Tag>();
            //IF the ID is not 0, update them
            FeatureSaved = true;
            return Page();
        }


    }
}