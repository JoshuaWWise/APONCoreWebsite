using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreLibrary.Models.ReturnModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages.Shared
{
    public class _Forum__PostModel : PageModel
    {
        public _Forum__PostModel(ForumPostInfo fpi)
        {
            FPI = fpi;
            newPost = new Models.Post();
            newPost.Description = FPI.forumPost.Text;
        }
        public ForumPostInfo FPI { get; set; }

        public Models.Post newPost { get; set; }
        public void OnGet()
        {
           

        }
    }
}