using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using e5movies.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;

namespace MovieApp.Controllers
{
    public class ReviewController : Controller
    {
        private moviesdbEntities db = new moviesdbEntities();

        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            var comment = form["Comment"].ToString();
            var MovieID = int.Parse(form["ImageID"]);
            var rating = int.Parse(form["Rating"]);

            Review movieReview = new Review()
            {
                ImageID = MovieID,
                Comment = comment,
                rating = rating,
                DateReviewed = DateTime.Now
            };

            db.Reviews.Add(movieReview);
            db.SaveChanges();

            return RedirectToAction("Details", "Movies", new { id = MovieID });
        }

    }
}
