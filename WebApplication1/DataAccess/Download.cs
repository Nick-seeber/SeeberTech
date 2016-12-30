using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace DataAccess
{
    public class Download
    {
        public string Connect(string downloadLink, string method, string name, string type)
        {
            string url = "http://nickstechtips.com:9091/transmission/rpc?method=";
            string TvDirectory = "D:/TvShows";
            string MovieDirectory = "D:/Movies/Transfer";
            string DownloadDirectory = string.Empty;
            switch (type)
            {
                case ("Movies"):
                    {
                        DownloadDirectory = MovieDirectory;
                        break;
                    }
                case ("TvShow"):
                    {
                        DownloadDirectory = TvDirectory;
                        break;
                    }
                default:
                    {
                        DownloadDirectory = "D:/DownloadAPIPathBroken";
                        break;
                    }
            }
            url = url + method + "&filename=" + downloadLink + "&download-dir=" + DownloadDirectory;
            string IdKey = GetCurrentID(url);
            RestRequest request = CreateRequest(IdKey);
            IRestResponse Response = SendRequest(request, url);
            string Status = Response.ResponseStatus.ToString();
            return Status;
        }

        public RestRequest CreateRequest(string IdKey)
        {
            var Request = new RestRequest(Method.GET);
            Request.AddHeader("postman-token", "6eb5ff0d-0c5c-78bb-dd30-5f85fe85a074");
            Request.AddHeader("cache-control", "no-cache");
            Request.AddHeader("x-transmission-session-id", IdKey);
            return Request;
        }

        public IRestResponse SendRequest(RestRequest Request, string url)
        {
            RestClient Client = new RestClient(url);
            IRestResponse Response = Client.Execute(Request);
            return Response;
        }

        public string GetNewID(IRestResponse Response)
        {
            string response = Response.Content;
            string[] responseArray;
            responseArray = response.Split('>');
            string IdKey = "";
            foreach (string item in responseArray)
            {
                if (item.Length > 1 && item.Contains("X-Transmission-Session-Id:"))
                {
                    string[] pullID = item.Split(':');
                    IdKey = CleanID(pullID);
                }
            }
            return IdKey;
        }

        public string CleanID(string[] pullID)
        {
            string CleanID = "";
            string[] CleaningArray;
            CleaningArray = pullID[1].Split('<');
            CleanID = CleaningArray[0];
            return CleanID;
        }

        public string GetCurrentID(string url)
        {
            RestRequest request = CreateRequest("");
            RestClient Client = new RestClient(url);
            IRestResponse Response = Client.Execute(request);
            string IdKey = string.Empty;
            if (Response.StatusCode == HttpStatusCode.Conflict)
            {
                IdKey = GetNewID(Response);
            }
            return IdKey;
        }
    }
}
