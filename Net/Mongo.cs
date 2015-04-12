using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace HAS_Post.Net
{
    //Classs Mongo
    public class Mongo<T>
    {
        //Interface IMongo
        public interface IMongo
        {
            void loadDocuments(List<T> documents);
        }

        const String URL_BASE = "https://api.mongolab.com/api/1/databases/";

        HttpClient client;
        String url;
        String dbName;
        String collectionName;
        String apiKey;
        //IMongo iMongo;

        public Mongo(String apiKey, String dbName, String collectionName)
        {
            client = new HttpClient();
            this.dbName = dbName;
            this.collectionName = collectionName;
            this.apiKey = "apiKey=" + apiKey;
            url = URL_BASE + dbName + "/collections/" + collectionName + "?" + this.apiKey;
        }


        public async void insertDocument(T document)
        {
            JsonSerializerSettings property = new JsonSerializerSettings();

            property.NullValueHandling = NullValueHandling.Ignore;
            String json = JsonConvert.SerializeObject(document, Formatting.None, property);
            HttpStringContent content = new HttpStringContent(json);
            HttpMediaTypeHeaderValue contentType = new HttpMediaTypeHeaderValue("application/json");
            content.Headers.ContentType = contentType;
            await client.PostAsync(new Uri(url), content);
        }

        public async void updateDocument(T document, String id)
        {
            String urlUpdate = URL_BASE + dbName + "/collections/" + collectionName + "/"+ id + "?" + this.apiKey;
            JsonSerializerSettings property = new JsonSerializerSettings();

            property.NullValueHandling = NullValueHandling.Ignore;
            String json = JsonConvert.SerializeObject(document, Formatting.None, property);
            HttpStringContent content = new HttpStringContent(json);
            HttpMediaTypeHeaderValue contentType = new HttpMediaTypeHeaderValue("application/json");
            content.Headers.ContentType = contentType;
            await client.PutAsync(new Uri(url), content);
        }

        public async void findAllDocuments(IMongo iMongo)
        {
            HttpResponseMessage msg = await client.GetAsync(new Uri(url));
            String jsonArray = msg.Content.ToString();
            jsonArray = jsonArray.Replace("$","");
            List<T> data = JsonConvert.DeserializeObject<List<T>>(jsonArray);
            iMongo.loadDocuments(data);
        }

        public async void findOneDocument(IMongo iMongo, String attribute, String value)
        {
            String auxUrl = URL_BASE + dbName + "/collections/" + collectionName + "?" + "q={\"" + attribute + "\": \"" + value + "\"}" + "&" + this.apiKey;
            HttpResponseMessage msg = await client.GetAsync(new Uri(auxUrl));
            String jsonArray = msg.Content.ToString();
            jsonArray = jsonArray.Replace("$", "");
            List<T> data = JsonConvert.DeserializeObject<List<T>>(jsonArray);
            iMongo.loadDocuments(data);
        }
    }
}
