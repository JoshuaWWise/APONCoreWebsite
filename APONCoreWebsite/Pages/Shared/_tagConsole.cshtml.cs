using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages.Shared
{
    public class _tagConsoleModel : PageModel
    {
        private ITagService TagService { get; set; }

        [BindProperty]
   
        public List<Tag> Tags { get; set; }

        [BindProperty]
        public string SelectedIDs { get; set; }
        [BindProperty]
        public string SelectedIndicies { get; set; }


        public _tagConsoleModel(ITagService tagService)
        {
            TagService = tagService;
        }


        public async Task<IActionResult> OnGetAsync()
        {

            Tags = await TagService.GetTags(false);

         

            return Page();
        }
    }
}