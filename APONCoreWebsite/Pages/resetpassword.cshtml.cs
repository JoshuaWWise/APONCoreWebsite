using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using APONCoreLibrary.Models;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages
{
    public class ResetPasswordModel :    ViewModelBase
    {
     
        [BindProperty(SupportsGet =true)]
        public string ID { get; set; }


        public ResetPasswordModel(IAuthService authService, IMetaTagService imts, ITagService ts, IDataService ds) : base(authService, imts, ds)
        {


        }

        public   void OnGet()
        {
    
        }
    }
}