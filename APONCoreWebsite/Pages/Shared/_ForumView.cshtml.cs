using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreLibrary.Models.ReturnModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using APONCoreLibrary.ReturnModels;
namespace APONCoreWebsite.Pages.Shared
{
    public class _ForumViewModel : PageModel
    {
       
        public _ForumViewModel()
        {
           
        }
        public bool ShowTop { get; set; }
        public Forum Forum { get; set; }

        public int CurrentUserID { get; set; }

        public List<_Forum__PostModel> Posts { get; set; }

        public LastUpdatedInfo ForumLUI { get; set; }

        public Models.Post NewPost { get; set; }
       
        public void OnGet()
        {
            NewPost = new Models.Post();
            
        }
    }
}