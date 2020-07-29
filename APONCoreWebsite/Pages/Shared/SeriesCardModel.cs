using APONCoreLibrary.Models;
using Microsoft.AspNetCore.Components;


namespace APONCoreWebsite.Pages.Shared
{
    public class SeriesCardModel
    {
        [Parameter] public Series Series { get; set; }

        public SeriesCardModel(Series series)
        {
            this.Series = series;
        }
    }
}
