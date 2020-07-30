using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    public class SeriesModel : ViewModelBase
    {
        protected IDataService DS { get; set; }
        public SeriesModel(IAuthService authService, IDataService ds): base(authService)
        {
            DS = ds;

        }

        [BindProperty(SupportsGet = true)]
        public string SeriesName { get; set; }

        public int EpCount { get; set; }

        public int PageNum { get; set; }

        [BindProperty]
        public Series currentSeries { get; set; }
        [BindProperty]
        public Series Series { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {

            //Get the Series information as well as episodes information. 
            string result = await DS.GetAsync("Series/GetSeriesByName/" + SeriesName);
            currentSeries = Newtonsoft.Json.JsonConvert.DeserializeObject<Series>(result);

            //Figure out Paging, 

            return Page();
        }
    }
}