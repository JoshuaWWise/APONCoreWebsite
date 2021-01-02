using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreLibrary.Models.RequestModels;
using APONCoreLibrary.ReturnModels;
using APONCoreWebsite.Models;
using APONCoreWebsite.Pages.Shared;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;

namespace APONCoreWebsite.Pages.Series
{
    [BindProperties]
    public class EpisodeModel : ViewModelBase
    {
        [BindProperty(SupportsGet = true)]
        public int EpisodeID { get; set; }
        public EpisodePageData EpisodeData { get; set; }

        public Episode Episode { get; set; }


        public List<Tag> Tags { get; set; }

        public string SeriesName { get; set; }

        public UserBriefWithBadges Author { get; set; }

        public string FileURL { get; set; }

        public ISeriesService SS { get; set; }

        public int CurrentUserID { get; set; }

        [BindProperty]
        public APONCoreLibrary.Models.Series epSeries { get; set; }

        public Shared._ForumViewModel fvm { get; set; }
        public EpisodeModel(IDataService ds, IAuthService authService, IMetaTagService imts, ISeriesService ss) : base(authService, imts, ds)
        {
            SS = ss;

        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (EpisodeID == 0)
            {
                return Page();
            }
            string result = "";
            try
            {
                result = await DS.GetAsync("Episodes/GetEpisodePageData/" + EpisodeID);

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("404"))
                {
                    Response.Redirect("/");
                    Episode = new Episode();
                    Episode.Title = "No Episode Found";
                    Episode.EpisodeID = -1;
                    return Page();
                }
            }
            EpisodeData = Newtonsoft.Json.JsonConvert.DeserializeObject<EpisodePageData>(result);

            if (EpisodeData.EpWithTag.episode.Local)
            {
                FileURL = "https://media.allportsopen.org/Podcasts/" + EpisodeData.Series.Folder + EpisodeData.EpWithTag.episode.FileURL;
            }
            else
            {
                FileURL = EpisodeData.EpWithTag.episode.FileURL;
            }

            //
            IMTS.setParams("https://www.allportsopen.com/Series/Episode/" + EpisodeData.EpWithTag.episode.EpisodeID, "article", EpisodeData.EpWithTag.episode.Title, EpisodeData.EpWithTag.episode.Description, EpisodeData.EpWithTag.episode.EpImageURL);

            Episode = EpisodeData.EpWithTag.episode;
            Tags = EpisodeData.EpWithTag.tags;
            SeriesName = EpisodeData.Series.Name;
            Author = EpisodeData.Author;

            //TODO: add current user ID to forum post page and compare with forum post info

            fvm = new Shared._ForumViewModel();
            fvm.Forum = EpisodeData.Forum;
            fvm.ForumLUI = EpisodeData.ForumLUI;
            fvm.Posts = new List<_Forum__PostModel>();
            foreach (ForumPostInfo fpi in EpisodeData.Posts)
            {
                fpi.CurrentUserID = myAuthService.getUserID();
                _Forum__PostModel f = new _Forum__PostModel(fpi, myAuthService.getUserID());
                fvm.Posts.Add(f);

            }
            CurrentUserID = myAuthService.getUserID();
            fvm.CurrentUserID = myAuthService.getUserID();
            fvm.ShowTop = false;

            metaData md = new metaData();
            md.description = Episode.Description;
            md.image = Episode.EpImageURL;
            md.title = Episode.Title;
            md.type = "article";
            md.URL = "https://www.allportsopen.com/series/Episode/" + Episode.EpisodeID;

            epSeries = await SS.GetSeriesByID(Episode.SeriesID);
            return Page();
        }
    }
}