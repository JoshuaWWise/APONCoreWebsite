using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages.Admin
{
    public class RefreshSeriesModel : ViewModelBase
    {
        public ISeriesService SeriesService { get; set; }
        public RefreshSeriesModel(IAuthService authService, IMetaTagService imts, IDataService ds, ISeriesService serser) : base(authService, imts, ds)
        {
            SeriesService = serser;
           
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await SeriesService.GetSeriesList(true);

            return Page();
        }
    }
}
