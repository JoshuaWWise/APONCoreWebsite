using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using APONCoreLibrary.Models;
using System.ComponentModel;

namespace APONCoreWebsite.Pages.Admin
{
    public class TagHandlerModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string TagName { get; set; }

        private ITagService TS { get; set; }
        public TagHandlerModel(ITagService tagService)
        {
            this.TS = tagService;
        }

        public async Task<IActionResult> OnPostTagAsync()
        {
           List<Tag> tags = await TS.AddTag(TagName);

            return new JsonResult(tags);


          
        }
        public void OnGet()
        {

        }
    }
}