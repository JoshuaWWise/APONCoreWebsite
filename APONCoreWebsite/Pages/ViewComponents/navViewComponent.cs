using APONCoreLibrary.Models;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APONCoreWebsite.Pages.ViewComponents
{
    public class navViewComponent : ViewComponent
    {

        IAuthService myAuthService;
        public navViewComponent(IAuthService mas)
        {
            myAuthService = mas;
        }

        public UserReturnToken URT { get; set; }



        public IViewComponentResult Invoke()
        {
            URT = myAuthService.getUser();

     

            return View(myAuthService.getUserAuthLevel());

        }
    }
}
