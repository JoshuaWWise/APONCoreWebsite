
using System.Collections.Generic;

using System.Net.Http;

using System.Threading.Tasks;
using APONCoreLibrary.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using APONCoreWebsite.Services;
using APONCoreWebsite.Pages.ViewModels;

namespace APONCoreWebsite.Pages
{
    public class IndexModel : ViewModelBase
    {
        public IndexModel(ILogger<IndexModel> logger, HttpClient client, IAuthService authService, IMetaTagService imts, IDataService ds, IUserInfoService iuis) : base(authService, imts, ds, iuis)
        {
            _logger = logger;
            _client = client;
           
        }
        private readonly ILogger<IndexModel> _logger;
        private HttpClient _client;
      

        [BindProperty]
        public List<SplashNews> SplashNews { get; set; }
        [BindProperty(SupportsGet = true)]
        public int index { get; set; }
        public int indexSecond { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            IMTS.setParams("https://www.allportsopen.com", "website", "The All Ports Open Network", "A Podcast Network for Geeks", "https://media.allportsopen.org/Images/LogoSmWText.png");
            if (SplashNews == null)
            {
                var result = await _client.GetStringAsync("SplashNews/GetThree");
                SplashNews = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SplashNews>>(result);
            }
            ViewData["SplashNews"] = SplashNews;
            return Page();
        }

        public string getLink()
        {
            return SplashNews[index].PostURL;

        }

        public IActionResult onPost()
        {

            return Page();
        }

        public IActionResult OnPostPreviousNews()
        {
            
          

            if (index - 1 < 0)
            {
                index = SplashNews.Count - 1;
            }
            else
            {
               index--;
            }

            return Page();
        }

        public IActionResult OnPostNextNews()
        {

        

            if (index + 1 >= SplashNews.Count)
            {
               index = 0;
            }
            else
            {
                index++;
            }

            return Page();
        }


    }

}
