using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace APONCoreWebsite.Pages
{
    [BindProperties]
    public class SeriesModel : ViewModelBase
    {
        protected IDataService DS { get; set; }
        public SeriesModel(IAuthService authService, IDataService ds) : base(authService)
        {
            DS = ds;

        }

        [BindProperty(SupportsGet = true)]
        public string SeriesName { get; set; }

        public int EpCount { get; set; }

        public int PageNum { get; set; }


        public SeriesPageData SPD { get; set; }


        public PartialViewResult GetEps(List<SmallEpisode> eps)
        {
            _SeriesEpisodeListModel selm = new _SeriesEpisodeListModel
            {
                Episodes = eps
            };
            ViewData.Model = selm;
            return new PartialViewResult()
            {
                ViewName = "_SeriesEpisodeList",
                ViewData = ViewData,
                TempData = null
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {

            //Get the Series information as well as episodes information. 
            string result = await DS.GetAsync("Series/GetSeriesPageByName/" + SeriesName);

            SPD = Newtonsoft.Json.JsonConvert.DeserializeObject<SeriesPageData>(result);

            EpCount = SPD.Episodes.Count;

         

            //Figure out Paging, 

            return Page();
        }
    }
}