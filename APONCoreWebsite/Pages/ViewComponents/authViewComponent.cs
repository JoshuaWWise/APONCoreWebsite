using APONCoreLibrary.Models;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APONCoreWebsite.Pages.ViewComponents
{
    public class authViewComponent : ViewComponent
    {

        IUserInfoService UIS;
        public authViewComponent(IUserInfoService uis)
        {
            UIS = uis;
        }

       
        public UserReturnToken URT { get; set; }

        public IViewComponentResult Invoke()
        {
            URT = UIS.getUser();


            return View(URT);

        }

        public void OnPost(FormCollection form)
        {

            string jwt = form["jwt"];

        }

    }
}
