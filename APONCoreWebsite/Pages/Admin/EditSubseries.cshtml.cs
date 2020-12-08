using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using APONCoreLibrary.Models;
using APONCoreWebsite.Services;
using APONCoreLibrary.ReturnModels;
using APONCoreWebsite.Pages.Shared;
using APONCoreWebsite.Pages.ViewModels;
using System.Net.Http;

namespace APONCoreWebsite.Pages.Admin
{
    public class EditSubseriesModel : ViewModelBase
    {

        public List<SubseriesPageInfo> subseriesList { get; set; }

        ISeriesService SS { get; set; }
        public ITagService tagService { get; set; }

        public string ErrorMessage { get; set; }
        public EditSubseriesModel(IAuthService authService, IMetaTagService imts, ITagService ts, IDataService ds, ISeriesService ss) : base(authService, imts, ds)
        {
            this.tagService = ts;

           

            SS = ss;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            ErrorMessage = "";
            //Get list of all subseries and their episodes
            string response = await DS.GetAsync("Series/GetAllSubseries");
            subseriesList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SubseriesPageInfo>>(response);

            return Page();


        }

        public async Task<IActionResult> OnPostAddSubseriesAsync()
        {
            string title = Request.Form["addSSTitle"];
            HttpResponseMessage message = await DS.PostAsync(title, "Series/AddSubseries");

           string response = await DS.GetAsync("Series/GetAllSubseries");
            subseriesList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SubseriesPageInfo>>(response);

            return Page();


        }
        public async Task<IActionResult> OnPostEpisodeAsync()
        {
            ErrorMessage = "";
            int ssID = int.Parse(Request.Form["subseriesID"]);
            string response = await DS.GetAsync("Series/GetSubseries/" + ssID.ToString());
            Subseries S = Newtonsoft.Json.JsonConvert.DeserializeObject<Subseries>(response);
            S.ImageURL = Request.Form["ssImageURL"];
            S.Title = Request.Form["ssTitle"];
            S.Description = Request.Form["ssDescription"];

            SubseriesUpdateInfo SUI = new SubseriesUpdateInfo();
            SUI.Subseries = S;
            SUI.EpisodeIDList = Request.Form["epListIDs"];
            if (SUI.EpisodeIDList.Trim().Length > 0)
            {
                SUI.EpisodeIDList = SUI.EpisodeIDList.Trim().Replace(" ", "");

                try
                {
                    char[] c = { ',' };
                    string[] slist = SUI.EpisodeIDList.Split(c);
                    for (int i = 0; i < slist.Length; i++)
                    {
                        int j = int.Parse(slist[i]);
                    }

                }
                catch (Exception e)
                {
                    ErrorMessage = "Unable to read Episode ID list properly. Please make sure you only enter Episode IDs and commas";
                    response = await DS.GetAsync("Series/GetAllSubseries");
                    subseriesList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SubseriesPageInfo>>(response);

                    return Page();
                }
            }

            HttpResponseMessage message = await DS.PutAsync(SUI, "Series/UpdateSubseries");

            response = await DS.GetAsync("Series/GetAllSubseries");
            subseriesList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SubseriesPageInfo>>(response);

            return Page();
        }
    }
}