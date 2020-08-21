using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.ReturnModels;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages.Admin
{
    public class EditorModel : ViewModelBase
    {
       

        public List<SmallSeries> SeriesForUser { get; set; }
        public EditorModel(IAuthService authService, IMetaTagService imts, IDataService ds): base(authService, imts, ds)
        {
           
        }


        public async Task<IActionResult> OnGetAsync()
        {

            int userID = myAuthService.getUserID();

            string Response = await DS.GetAsync("series/GetSeriesForUser/" + userID);
            SeriesForUser = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SmallSeries>>(Response);


            return Page();

        }
    }
}