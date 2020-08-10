using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using System.Net.Http;
using System.Web.Helpers;

namespace APONCoreWebsite.Pages
{
    public class ImageHandlerModel : ViewModelBase
    {
        private IDataService DS { get; set; }
        public ImageHandlerModel(IAuthService authService, IMetaTagService imts, IDataService ds) : base(authService, imts)
        {
            DS = ds;
        }

        [BindProperty, Display(Name = "File")]
        public IFormFile UploadedFile { get; set; }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostNewsImage()
        {
            IFormFile FileData = Request.Form.Files[0];
            string Message = "";

            HttpResponseMessage response = await DS.PostImageAsync(FileData, "News/UploadNewsImage");

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Message = "";
            }
            else
            {
                Message = "Error Uploading File: " + response.Content.ToString();
            }

            JsonResult result = new JsonResult(Message);

            return result;
        }
    }
}