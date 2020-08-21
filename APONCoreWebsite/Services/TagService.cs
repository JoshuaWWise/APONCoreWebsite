using APONCoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APONCoreWebsite.Services
{

    public interface ITagService
    {
        public Task<bool> LoadTags();

        public Task<List<Tag>> GetTags(bool refreshTags);

        public  Task<List<Tag>> AddTag(string tagName, int UserID);
    }
    public class TagService : ITagService
    {
        public IDataService DS { get; set; }
      
   
        public TagService(IDataService ds)
        {
            this.DS = ds;
          
        }

   
        public List<Tag> RawTags { get; set; }

        public async Task<bool> LoadTags()
        {
            //Use dataservice to get all tags. 
            string result = await DS.GetAsync("Tag/GetAll");
            RawTags = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Tag>>(result);


            return true;
        }

        public async Task<List<Tag>> GetTags(bool refreshTags)
        {
            if (RawTags == null || refreshTags)
            {
                await LoadTags();
            }



            return RawTags;



        }

        public async Task<List<Tag>> AddTag(string tagName, int UserID)
        {
            //Add Tag info including current user id
            Tag t = new Tag();
            t.Name = tagName;
            t.CreatedBy = UserID;
            

            HttpResponseMessage httpResponseMessage = await DS.PostAsync(t, "Tag/AddTag");

            //if the message said the tag was added, refresh the tag list. 

            //if not, just return the tag list as it is.

            await LoadTags();

            return RawTags;

        }

      
    }
}
