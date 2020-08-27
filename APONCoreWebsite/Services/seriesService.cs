using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APONCoreLibrary.Models;

namespace APONCoreWebsite.Services
{
    public interface ISeriesService
    {
        public Task<List<Series>> GetSeriesList(bool ForceRefresh);

        public Task<Series> GetSeriesByID(int id);

    }
    public class seriesService : ISeriesService
    {
        private List<Series> SavedSeriesList { get; set; }

        private DateTime LastSeriesUpdate { get; set; }

        private int SeriesCheckIntrval { get; set; }
   private HttpClient httpClient { get; set; }

        public seriesService(HttpClient client)
        {
            SeriesCheckIntrval = 6;
            LastSeriesUpdate = DateTime.Now.AddDays(-10);
            httpClient = client;
            SavedSeriesList = new List<Series>();
        }

        public async Task<List<Series>> GetSeriesList(bool ForceRefresh)
        {
            if (ForceRefresh || (LastSeriesUpdate.AddHours(SeriesCheckIntrval) < DateTime.Now))
            {
                var Response = await httpClient.GetStringAsync("Series");
                SavedSeriesList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Series>>(Response);
                LastSeriesUpdate = DateTime.Now;
            }

            return SavedSeriesList;
        }

        public async Task<Series> GetSeriesByID(int id)
        {
            if (SavedSeriesList.Count == 0)
            {
               await GetSeriesList(false);
            }

            Series S = SavedSeriesList.Where(x => x.SeriesID == id).FirstOrDefault();

            return S;
        }
    }
}
