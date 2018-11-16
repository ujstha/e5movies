using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using e5movies.Models;

namespace e5movies.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string name, string password)
        {
            if ("admin".Equals(name) && "123456aA".Equals(password))
            {
                Session["user"] = new admin() { Login = name, Name = "Ujjawal" };
                return RedirectToAction("Index", "Movies");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            //or Session["user"] = null;
            return RedirectToAction("Index", "Movies");
        }
    }
}