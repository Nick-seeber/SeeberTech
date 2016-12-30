using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WebApplication1.Models;
using DataAccess;


namespace WebApplication1.Controllers
{
    public class MoviesController : Controller
    {
        private NTTDB db = new NTTDB();


        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {

                return View(db.media_info.ToList());
            }
            else
            {
                return Redirect("/Home/Index");

            }
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            media_info media_info = db.media_info.Find(id);
            if (media_info == null)
            {
                return HttpNotFound();
            }
            return View("/Movies/Details", media_info);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,url,imdb_code,title,title_english,title_long,slug,year,rating,runtime,summary,description_full,synopsis,yt_trailer_code,language,mpa_rating,background_image,small_cover_image,medium_cover_image,large_cover_image,state,torrents,date_uploaded")] media_info media_info)
        {
            if (ModelState.IsValid)
            {
                db.media_info.Add(media_info);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(media_info);
        }

        public ActionResult GetNewData([Bind(Include = "url,title,summary,description_full,language,background_image,small_cover_image,medium_cover_image,large_cover_image")]media_info media_info)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Account/Login");
            }


            string url = "https://yts.ag/api/v2/list_movies.json";
            string jsonString;
            string xmlString = string.Empty;
            XmlDocument document = new XmlDocument();
            List<media_info> mediaList = new List<media_info>();
            int count = 0;
            string genres = string.Empty;

            using (WebClient c = new WebClient())
            {
                jsonString = c.DownloadString(url);
            }

            JToken json = JsonConvert.DeserializeObject<JToken>(jsonString);
            List<JToken> MoviesList = json["data"]["movies"].ToList();
            dynamic json2 = JsonConvert.DeserializeObject(jsonString);
            JArray jsonArray = JArray.FromObject(json2.data.movies);
            string jsonmovieString = json2.data.movies.ToString();
            dynamic jsonMovieObject = JsonConvert.DeserializeObject(jsonmovieString);
            JArray jsonMovieArray = JArray.FromObject(jsonMovieObject);
            count = int.Parse(json2.data.limit.Value.ToString());


            for (int i = 0; i <= count - 1; i++)
            {
                media_info media = new media_info()
                {
                    id = int.Parse(json2.data.movies[i].id.ToString()),
                    url = json2.data.movies[i].torrents[0].url.Value.ToString(),
                    title = json2.data.movies[i].title.Value.ToString(),
                    background_image = json2.data.movies[i].background_image.Value.ToString(),
                    small_cover_image = json2.data.movies[i].small_cover_image.Value.ToString(),
                    medium_cover_image = json2.data.movies[i].medium_cover_image.Value.ToString(),
                    large_cover_image = json2.data.movies[i].large_cover_image.Value.ToString(),
                    language = json2.data.movies[i].language.Value.ToString(),
                    summary = json2.data.movies[i].summary.Value.ToString(),
                    description_full = json2.data.movies[i].description_full.Value.ToString(),
                    year = json2.data.movies[i].year.ToString(),
                    imdb_code = json2.data.movies[i].imdb_code.Value.ToString(),
                    title_english = json2.data.movies[i].title_english.Value.ToString(),
                    title_long = json2.data.movies[i].title_long.Value.ToString(),
                    slug = json2.data.movies[i].slug.Value.ToString(),
                    rating = json2.data.movies[i].rating.Value.ToString(),
                    runtime = json2.data.movies[i].runtime.Value.ToString(),

                };

                mediaList.Add(media);

            }
            mediaList = mediaList.OrderBy(x => x.date_uploaded).ToList();
            return View("Index", mediaList);
        }

        public List<current_movies> GetCurrentMovies()
        {
            string moviedir = @"D:\Movies\";
            List<current_movies> currentMediaList = new List<current_movies>();
            string[] currentMedia = Directory.GetDirectories(moviedir);
            foreach (string mediaString in currentMedia)
            {
                string[] splitMediaString = mediaString.Split('\\');
                current_movies currentMediaDir = new current_movies();
                currentMediaDir.Name = splitMediaString[splitMediaString.Count() - 1];
                currentMediaDir.FilePath = moviedir + currentMediaDir.Name;
                currentMediaList.Add(currentMediaDir);
                db.current_movies.Add(currentMediaDir);
                db.SaveChanges();
            }


            return currentMediaList;
        }

        public ActionResult DownloadISO()
        {
            return View(@"\en_sql_server_2016_enterprise_x64_dvd_8701793.iso");
        }



        public ActionResult DownloadMovie(string url, string name)
        {
            DataAccess.Download newDownload = new Download();
            string downloadResult = newDownload.Connect(url, "torrent-add", name, "Movie");
            return Redirect("/Movies/GetNewData");
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            media_info media_info = db.media_info.Find(id);
            if (media_info == null)
            {
                return HttpNotFound();
            }
            return View(media_info);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,url,imdb_code,title,title_english,title_long,slug,year,rating,runtime,summary,description_full,synopsis,yt_trailer_code,language,mpa_rating,background_image,small_cover_image,medium_cover_image,large_cover_image,state,torrents,date_uploaded")] media_info media_info)
        {
            if (ModelState.IsValid)
            {
                db.Entry(media_info).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(media_info);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            media_info media_info = db.media_info.Find(id);
            if (media_info == null)
            {
                return HttpNotFound();
            }
            return View(media_info);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            media_info media_info = db.media_info.Find(id);
            db.media_info.Remove(media_info);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
