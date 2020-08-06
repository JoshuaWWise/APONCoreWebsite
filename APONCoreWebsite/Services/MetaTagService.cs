using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APONCoreWebsite.Services
{
    public interface IMetaTagService
    {
        public List<string> getMetaData();

        public void setParams(string url, string type, string title, string desc, string img);
    }

    public class MetaTagService : IMetaTagService
    {

        public string URL { get; set; }
        public string type { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string image { get; set; }

        public void setParams(string url, string type, string title, string desc, string img)
        {
            URL = url;
            this.type = type;
            this.title = title;
            description = desc;
            image = img;
        }


        public List<string> getMetaData()
        {
            List<string> data = new List<string>();

            data.Add(URL);
            data.Add(type);
            data.Add(title);
            data.Add(description);
            data.Add(image);


            return data;
        }


    }
}
