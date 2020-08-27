using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreLibrary.ReturnModels;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    public class SubseriesModel : ViewModelBase
    {

        public SubseriesModel(IAuthService authService, IDataService ds, IMetaTagService imts) : base(authService, imts, ds)
        {

        }

        [BindProperty(SupportsGet = true)]
        public int SubseriesID { get; set; }

        public int EpCount { get; set; }

        public SubseriesPageInfo SubSeries { get; set; }
    
        public async Task<IActionResult> OnGetAsync()
        {
            string response = await DS.GetAsync("Series/GetSubseriesPageInfo/" + SubseriesID);

            this.SubSeries = Newtonsoft.Json.JsonConvert.DeserializeObject<SubseriesPageInfo>(response);

            this.EpCount = this.SubSeries.Episodes.Count;

            return Page();
        }
    }
}