using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    public class _metaTagsModel : PageModel
    {
        public IMetaTagService IMTS { get; set; }
        public _metaTagsModel(IMetaTagService imts)
        {
            IMTS = imts;
        }

        public void OnGet()
        {

        }
    }
}