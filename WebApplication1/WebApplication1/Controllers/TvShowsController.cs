using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.ServiceModel.Syndication;
using System.Xml;
using Newtonsoft.Json;
using DataAccess;

namespace WebApplication1.Controllers
{
    public class TvShowsController : Controller
    {
        private NicksTechtipsTvShows db = new NicksTechtipsTvShows();

        // GET: TvShows
        public async Task<ActionResult> Index()
        {
            return View(await db.tv_shows.ToListAsync());
        }

        // GET: TvShows/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tv_shows tv_shows = await db.tv_shows.FindAsync(id);
            if (tv_shows == null)
            {
                return HttpNotFound();
            }
            return View(tv_shows);
        }

        // GET: TvShows/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TvShows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,url,imdb_code,title,title_english,title_long,slug,year,rating,season,episode,runtime,summary,description_full,synopsis,yt_trailer_code,language,mpa_rating,background_image,small_cover_image,medium_cover_image,large_cover_image,state,torrents,date_uploaded,date_updated")] tv_shows tv_shows)
        {
            if (ModelState.IsValid)
            {
                db.tv_shows.Add(tv_shows);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tv_shows);
        }

        public ActionResult UpdateTvShows()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Account/Login");
            }
            string downloadurl = "https://eztv.ag/ezrss.xml";
            int itemCount = 0;
            XmlReader reader = XmlReader.Create(downloadurl);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            List<tv_shows> TvShowList = new List<tv_shows>();
            foreach (SyndicationItem item in feed.Items)
            {
                String Title = item.Title.Text;
                if (Title != null)
                {
                    tv_shows TvShow = new tv_shows()
                    {
                        title = TitleParser(Title),
                        url = item.Links[1].Uri.AbsoluteUri.ToString(),
                    };
                    TvShowList.Add(TvShow);
                }
                itemCount++;
            }
            return View("Index", TvShowList);
        }

        static string TitleParser(string title)
        {
            String newtitle = String.Empty;
            for (int i = 0; i < title.Length; i++)
            {
                newtitle = newtitle + title[i];
                if (i > 20)
                {
                    i = title.Length;
                }
            }


            return newtitle;
        }


        public ActionResult DownloadTvShow(string url, string title)
        {
            Download download = new Download();
            download.Connect(url, "torrent-add", title, "TvShow");

            return Redirect("/tvshows/UpdateTvShows");
        }

        // GET: TvShows/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tv_shows tv_shows = await db.tv_shows.FindAsync(id);
            if (tv_shows == null)
            {
                return HttpNotFound();
            }
            return View(tv_shows);
        }

        // POST: TvShows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,url,imdb_code,title,title_english,title_long,slug,year,rating,season,episode,runtime,summary,description_full,synopsis,yt_trailer_code,language,mpa_rating,background_image,small_cover_image,medium_cover_image,large_cover_image,state,torrents,date_uploaded,date_updated")] tv_shows tv_shows)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tv_shows).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tv_shows);
        }

        // GET: TvShows/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tv_shows tv_shows = await db.tv_shows.FindAsync(id);
            if (tv_shows == null)
            {
                return HttpNotFound();
            }
            return View(tv_shows);
        }

        // POST: TvShows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tv_shows tv_shows = await db.tv_shows.FindAsync(id);
            db.tv_shows.Remove(tv_shows);
            await db.SaveChangesAsync();
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
