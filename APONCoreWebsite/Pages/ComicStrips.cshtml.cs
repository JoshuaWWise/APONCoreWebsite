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
    public class ComicStripsModel : ViewModelBase
    {
       public List<ComicStrip> Strips { get; set; }
        public ComicStripsModel(IAuthService authService, IDataService ds, IMetaTagService imts) : base(authService, imts, ds)
        {

        }

        public async Task<IActionResult> OnGetAsync()
        {
            string ComicListResponse = await DS.GetAsync("comic/GetAllComics");
            Strips = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ComicStrip>>(ComicListResponse);
            return Page();
        }
    }
}
