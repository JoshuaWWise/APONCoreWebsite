using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using APONCoreLibrary.Models;
using APONCoreWebsite.Models;
using APONCoreWebsite.Pages.Shared;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
namespace APONCoreWebsite.Pages
{
    public class TagModel : ViewModelBase
    {

        [BindProperty(SupportsGet = true)]
        public int TagID { get; set; }


        public TagBundle TB { get; set; }

        public TagModel(IDataService ds, IAuthService authService, IMetaTagService imts) : base(authService, imts, ds)
        {


        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (TagID == -1)
            {
                TB = new TagBundle();
             
                TB.Name = "Undefined";
                Response.Redirect("/");
                return Page();
            }
            string result = "";
            result = await DS.GetAsync("Tag/GetItemsForTag/" + TagID);
            TB = Newtonsoft.Json.JsonConvert.DeserializeObject<TagBundle>(result);

            return Page();


        }
    }
}