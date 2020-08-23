using APONCoreLibrary.Models;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APONCoreWebsite.Pages.ViewModels
{
    public abstract class ViewModelBase : PageModel
    {
        public UserReturnToken myURT { get; set; }
        public IAuthService myAuthService { get; set; }

       public _metaTagsModel MetaTagsModel { get; set; }

        public IMetaTagService IMTS { get; set; }

     

        public IDataService DS { get; set; }

        public int UserAuthLevel { get; set; }

        public UserReturnToken LoggedInUser { get; set; }

  string PageMessage { get; set; }

        public ViewModelBase(IAuthService authService, IMetaTagService imts, IDataService ds)
        {
            myAuthService = authService;
            MetaTagsModel = new _metaTagsModel(imts);
            IMTS = imts;
         
            DS = ds;
            UserAuthLevel = authService.getUserAuthLevel();
         
        }

        public void Init()
        {
            this.LoggedInUser = myAuthService.getUser();
        }

        public IActionResult OnPost()
        {
            try
            {
                string s = Request.Form["layoutJWT"].ToString();
                UserReturnToken URT = new UserReturnToken(s);

                myAuthService.AutoLogin(URT);
            }
            catch(Exception)
            {

            }

            return Page();

        }
    }
}
