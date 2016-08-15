using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.Models;

namespace Forum.Controllers
{
    public class AdminController : Controller
    {
        protected readonly ApplicationDbContext db = new ApplicationDbContext();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            var users = db.Users.ToList();

            return View(users);
        }

        public PartialViewResult Search(string query)
        {
            var result = db.Users.Where(u => u.UserName.ToLower().Contains(query.ToLower())).ToList();

            return PartialView("_UsersResult", result);
        }
    }
}