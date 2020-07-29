using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    public class SeriesModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string SeriesName { get; set; }

        public int EpCount { get; set; }

        public int PageNum { get; set; }


        [BindProperty]
        public Series Series { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            
            //Get the Series information as well as episodes information. 


            //Figure out Paging, 

            return Page();
        }
    }
}