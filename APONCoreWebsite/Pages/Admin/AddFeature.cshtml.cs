using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.Shared;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages.Admin
{
    public class AddFeatureModel : ViewModelBase
    {

        public AddFeatureModel(IAuthService authService, IMetaTagService imts, ITagService ts) : base(authService, imts)
        {
            tagService = ts;
        }

        public ITagService tagService { get; set; }
 
        public _tagConsoleModel TCM { get; set; }

        [BindProperty(SupportsGet = true)]
        public int FeatureID { get; set; }

        public News Feature { get; set; }
        public async Task<IActionResult>  OnGetAsync()
        {
            Feature = new News();

            //Otherwise Get the feature to be populated in the form.
            TCM = new _tagConsoleModel(tagService);
            TCM.Tags =  await tagService.GetTags(false);

            if (FeatureID == 0)
            {
                return Page();
            }



            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //Get Values from form and save them.

           //If the ID is 0, add them.


            //IF the ID is not 0, update them

            return Page();
        }

  
    }
}