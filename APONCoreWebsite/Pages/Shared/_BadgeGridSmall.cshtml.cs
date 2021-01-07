using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages.Shared
{
    public class _BadgeGridSmallModel : PageModel
    {
        public List<Badge> BadgeURLS { get; set; }
        public _BadgeGridSmallModel(List<Badge> badgeURLS)
        {
            BadgeURLS = badgeURLS;
        }
        public void OnGet()
        {
        }
    }
}
