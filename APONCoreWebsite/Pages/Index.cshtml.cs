
using System.Collections.Generic;

using System.Net.Http;

using System.Threading.Tasks;
using APONCoreLibrary.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using APONCoreWebsite.Services;

namespace APONCoreWebsite.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel(ILogger<IndexModel> logger, HttpClient client, IDataService dataService)
        {
            _logger = logger;
            _client = client;
            _DS = dataService;
        }
        private readonly ILogger<IndexModel> _logger;
        private HttpClient _client;
        private IDataService _DS;

        [BindProperty]
        public List<SplashNews> SplashNews { get; set; }
        [BindProperty(SupportsGet = true)]
        public int index { get; set; }
        public int indexSecond { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

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
