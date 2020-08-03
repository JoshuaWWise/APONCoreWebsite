using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    public class _SeriesEpisodeListModel : PageModel
    {
        public List<SmallEpisode> Episodes { get; set; }

        public void OnGet()
        {

        }

       
    }
}