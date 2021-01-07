using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APONCoreLibrary.Models;
using APONCoreLibrary.ReturnModels;
using APONCoreWebsite.Pages.ViewModels;
using APONCoreWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APONCoreWebsite.Pages.Admin
{
    public class EditorModel : ViewModelBase
    {
       

        public List<SmallSeries> SeriesForUser { get; set; }
        public List<ForumPost> ForumPosts { get; set; }

        
        public EditorModel(IAuthService authService, IMetaTagService imts, IDataService ds): base(authService, imts, ds)
        {
            ForumPosts = new List<ForumPost>();
        }


        public async Task<IActionResult> OnGetAsync()
        {
            base.Init();

            try
            {
                
               
                string Response = await DS.GetAsync("series/GetSeriesForUser/" + LoggedInUser.UserID);
                SeriesForUser = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SmallSeries>>(Response);

                if (myAuthService.getUserAuthLevel() < 3)
                {
                    string PostsResponse = await DS.GetAsync("forum/AdminGetLastWeekFP");
                    ForumPosts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ForumPost>>(PostsResponse);
                }
               
            }
            catch(Exception ex)
            {

            }
            if (SeriesForUser == null)
            {
                SeriesForUser = new List<SmallSeries>();
            }
                return Page();

        }
    }
}