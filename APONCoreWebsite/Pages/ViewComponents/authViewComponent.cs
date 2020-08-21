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

       

        IAuthService MyAuthService;
        public authViewComponent(IAuthService mas)
        {
         
            MyAuthService = mas;
        }

       
        public UserReturnToken URT { get; set; }

        public IViewComponentResult Invoke()
        {
            URT = MyAuthService.getUser();


            return View(URT);

        }

        public void OnPost(FormCollection form)
        {

            string jwt = form["jwt"];

        }

    }
}
