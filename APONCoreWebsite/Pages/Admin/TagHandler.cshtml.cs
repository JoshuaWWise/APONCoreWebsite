using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using APONCoreLibrary.Models;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using System.Web.Helpers;

namespace APONCoreWebsite.Pages.Admin
{
    public class TagHandlerModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string TagName { get; set; }

        private ITagService TS { get; set; }

        private IAuthService myAuthService;
        public TagHandlerModel(ITagService tagService, IAuthService ias)
        {
            this.TS = tagService;
            myAuthService = ias;
        }

        public async Task<IActionResult> OnPostTagAsync(string newTagName)
        {

            

            List<Tag> tags = await TS.AddTag(newTagName, myAuthService.getUserID());

            return new JsonResult(tags);


          
        }
        public void OnGet()
        {

        }
    }
}