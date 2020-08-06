using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace APONCoreWebsite.Controllers
{
    public class PostsController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }
    }
}
