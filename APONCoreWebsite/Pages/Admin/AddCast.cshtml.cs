using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages.Admin
{
    public class AddCastModel : ViewModelBase
    {
        public AddCastModel(IAuthService authService, IMetaTagService imts, IDataService ds) : base(authService, imts, ds)
        {

        }

        public void OnGet()
        {

        }
    }
}