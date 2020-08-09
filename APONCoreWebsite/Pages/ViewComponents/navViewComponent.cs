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

        IUserInfoService UIS;
        public navViewComponent(IUserInfoService uis)
        {
            UIS = uis;
        }

        public UserReturnToken URT { get; set; }



        public IViewComponentResult Invoke()
        {
            URT = UIS.getUser();

            int authLevel = int.Parse(UIS.getUser().AuthLevel);

            return View(authLevel);

        }
    }
}
