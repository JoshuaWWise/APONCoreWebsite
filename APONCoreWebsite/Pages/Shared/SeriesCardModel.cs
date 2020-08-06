using APONCoreLibrary.Models;
using Microsoft.AspNetCore.Components;


namespace APONCoreWebsite.Pages.Shared
{
    public class SeriesCardModel
    {
        [Parameter] public APONCoreLibrary.Models.Series Series { get; set; }

        public SeriesCardModel(APONCoreLibrary.Models.Series series)
        {
            this.Series = series;
        }
    }
}
