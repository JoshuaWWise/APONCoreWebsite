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
    public class EditorModel : ViewModelBase
    {
        public EditorModel(IAuthService authService, IMetaTagService imts): base(authService, imts)
        {

        }


        public void OnGet()
        {

        }
    }
}