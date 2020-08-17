using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.Shared;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    public class AddEpisodeModel : ViewModelBase
    {

        [BindProperty]
        public bool EpisodeSaved { get; set; }

        [BindProperty]
        public bool AttemptedSave { get; set; }

        public IDataService DS { get; set; }
        public ITagService tagService { get; set; }

        public IUserInfoService IUIS;

        public _tagConsoleModel TCM { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SeriesID { get; set; }

        public APONCoreLibrary.Models.Series EpSeries { get; set; }

        public EpisodeWithTags Episode { get; set; }

        public bool Affiliate { get; set; }

        public bool PageNotFound { get; set; }
        public AddEpisodeModel(IAuthService authService, IUserInfoService iuis, IMetaTagService imts, ITagService ts, IDataService ds) : base(authService, imts)
        {
            this.tagService = ts;
            this.IUIS = iuis;
            this.DS = ds;
            Episode = new EpisodeWithTags();
            Episode.episode = new Episode();
        }


        public async Task<IActionResult> OnGetAsync()
        {
            if (int.Parse(IUIS.getUser().AuthLevel) < 4)
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
    }
}