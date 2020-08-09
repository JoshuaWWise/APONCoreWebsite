using APONCoreWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APONCoreWebsite.Services
{
    public interface IMetaTagService
    {
        public metaData getMetaData();

        public void setMetaData(metaData md);

        public void setParams(string url, string type, string title, string desc, string img);
    }

    public class MetaTagService : IMetaTagService
    {

        public MetaTagService()
        {
            MetaData = new metaData();
        }
      public metaData MetaData { get; set; }

        public void setParams(string url, string type, string title, string desc, string img)
        {
            MetaData.URL = url;
            MetaData.type = type;
            MetaData.title = title;
            MetaData.description = desc;
            MetaData.image = img;
       
        }


        public metaData getMetaData()
        {
            return MetaData;
        }

        public void setMetaData(metaData md)
        {
            this.MetaData = md;
        }


    }
}
