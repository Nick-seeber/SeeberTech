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
using System.IO;

namespace WebApplication1.Controllers
{
    public class currentmoviesController : Controller
    {
        private NTTDB db = new NTTDB();

        // GET: currentmovies
        public async Task<ActionResult> Index()
        {
            return View("Index", await db.current_movies.ToListAsync());
        }

        // GET: currentmovies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            current_movies current_movies = await db.current_movies.FindAsync(id);
            if (current_movies == null)
            {
                return HttpNotFound();
            }
            return View(current_movies);
        }

        // GET: currentmovies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: currentmovies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,FilePath")] current_movies current_movies)
        {
            if (ModelState.IsValid)
            {
                db.current_movies.Add(current_movies);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(current_movies);
        }

        // GET: currentmovies/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            current_movies current_movies = await db.current_movies.FindAsync(id);
            if (current_movies == null)
            {
                return HttpNotFound();
            }
            return View(current_movies);
        }

        // POST: currentmovies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,FilePath")] current_movies current_movies)
        {
            if (ModelState.IsValid)
            {
                db.Entry(current_movies).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(current_movies);
        }

        // GET: currentmovies/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            current_movies current_movies = await db.current_movies.FindAsync(id);
            if (current_movies == null)
            {
                return HttpNotFound();
            }
            return View(current_movies);
        }

        // POST: currentmovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            current_movies current_movies = await db.current_movies.FindAsync(id);
            db.current_movies.Remove(current_movies);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Update()
        {
            if (User.IsInRole("Master"))
            {
                string moviedir = @"D:\Movies\Transfer";
                string parentDirectory = @"D:\Movies";
                List<current_movies> currentMediaList = new List<current_movies>();
                string[] currentMedia = Directory.GetDirectories(moviedir);
                string[] parentDir = Directory.GetDirectories(parentDirectory);
                List<current_movies> currentMoviesList = db.current_movies.ToList();
                foreach (string mediaString in currentMedia)
                {
                    string[] splitMediaString = mediaString.Split('\\');
                    current_movies currentMediaDir = new current_movies();
                    currentMediaDir.Name = splitMediaString[splitMediaString.Count() - 1];
                    currentMediaDir.FilePath = moviedir + currentMediaDir.Name;
                    currentMediaList.Add(currentMediaDir);
                }
                foreach (string mediaString in parentDir)
                {
                    string[] splitMediaString = mediaString.Split('\\');
                    current_movies currentMediaDir = new current_movies();
                    currentMediaDir.Name = splitMediaString[splitMediaString.Count() - 1];
                    if (currentMediaDir.Name != @"Transfer")
                    {
                        currentMediaDir.FilePath = moviedir + @"\" + currentMediaDir.Name;
                        currentMediaList.Add(currentMediaDir);
                    }
                }
                foreach (current_movies movie in currentMediaList)
                {
                    if (!currentMoviesList.Contains(movie))
                    {
                        db.current_movies.Add(movie);
                        db.SaveChanges();
                    }
                }
            }
            return View("Index", await db.current_movies.ToListAsync());
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
