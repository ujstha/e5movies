using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using e5movies.Models;
using System.IO;
using PagedList;

namespace e5movies.Controllers
{
    public class MoviesController : Controller
    {
        private moviesdbEntities db = new moviesdbEntities();

        // GET: Movies
        public ActionResult Index(string movieGenre, string movieStarring, string searchString)
        {
            var StarringLst = new List<string>();
            var StarringQry = from b in db.Images
                              orderby b.Starring
                              select b.Starring;
            StarringLst.AddRange(StarringQry.Distinct());
            ViewBag.movieStarring = new SelectList(StarringLst);

            var GenreLst = new List<string>();
            var GenreQry = from b in db.Images
                           orderby b.Genre
                           select b.Genre;
            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.movieGenre = new SelectList(GenreLst);

            var movies = from m in db.Images
                         select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }
            if (!String.IsNullOrEmpty(movieStarring))
            {
                movies = movies.Where(x => x.Starring == movieStarring);
            }
            return View(movies);
        }

        [HttpGet]
        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // selects specific movie to view
            var movieToView = (from m in db.Images
                               where m.ImageID == id
                               select m).First();

            //Allows use of id value in view
            ViewBag.ImageID = id.Value;

            //Takes reviews if not 0 sums them to a total and total number to use in view.
            var ratings = db.Reviews.Where(d => d.ImageID.Equals(id.Value)).ToList();

            if (ratings.Count() > 0)
            {
                var ratingSum = ratings.Sum(d => d.rating);
                var ratingCount = ratings.Count();
                var ratingAvg = ratingSum / ratingCount;

                movieToView.RatingAVG = ratingAvg;
                db.SaveChanges();

            }
            else
            {
                movieToView.RatingAVG = 0;
                db.SaveChanges();

            }
            if (movieToView == null)
            {
                return HttpNotFound();
            }
            return View(movieToView);
            // Image imageModel = db.Images.Find(id);
            //imageModel = db.Images.Where(x => x.ImageID == id).FirstOrDefault();

            //return View(imageModel);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Image imageModel)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                string extension = Path.GetExtension(imageModel.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmddhhss") + extension;
                imageModel.ImagePath = "~/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                imageModel.ImageFile.SaveAs(fileName);
                db.Images.Add(imageModel);
                imageModel.RatingAVG = 0;
                db.SaveChanges();

                ModelState.Clear();

                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image imageModel = db.Images.Find(id);
            imageModel = db.Images.Where(x => x.ImageID == id).FirstOrDefault();
            if (imageModel == null)
            {
                return HttpNotFound();
            }
            return View(imageModel);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, Image imageModel, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                string extension = Path.GetExtension(imageModel.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmddhhss") + extension;
                imageModel.ImagePath = "~/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                imageModel.ImageFile.SaveAs(fileName);

                db.Entry(imageModel).State = EntityState.Modified;
                db.SaveChanges();

                ModelState.Clear();

                return RedirectToAction("Index");
            }
            return View(imageModel);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            image = (from m in db.Images
                     where m.ImageID == id
                     select m).First();
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, FormCollection fcNotUsed)
        {
            Image image = db.Images.Find(id);
            db.Images.Remove(image);
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
