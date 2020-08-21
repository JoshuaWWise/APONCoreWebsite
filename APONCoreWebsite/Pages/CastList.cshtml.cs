using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreLibrary.Models.ReturnModels;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    public class CastListModel : ViewModelBase

    {
        [BindProperty(SupportsGet =true)]
        public int SeriesID { get; set; }

        public APONCoreLibrary.Models.Series series { get; set; }

        public List<SeriesCastType> Types { get; set; }

        public CastWithSocialMedia CastList { get; set; }

        public CastListModel(IDataService ds, IAuthService authService, IUserInfoService iuis, IMetaTagService imts) : base(authService, imts, ds, iuis)
        {


        }
        public async Task<IActionResult> OnGetAsync()
        {
            string Response = await DS.GetAsync("cast/GetCastForSeries_TypeSafe/" + SeriesID);

            CastList = Newtonsoft.Json.JsonConvert.DeserializeObject<CastWithSocialMedia>(Response);


            string SeriesResponse = await DS.GetAsync("series/" + SeriesID);
            series = Newtonsoft.Json.JsonConvert.DeserializeObject<APONCoreLibrary.Models.Series>(SeriesResponse);

            return Page();
        }
    }
}