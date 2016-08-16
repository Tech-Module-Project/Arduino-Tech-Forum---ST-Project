﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Forum.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Forum.Controllers
{
    public class AdminController : Controller
    {
        protected static readonly ApplicationDbContext db = new ApplicationDbContext();
        protected  UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            var users = db.Users.ToList();

            return View(users);
        }

        public ActionResult Search(string query)
        {
            var result = db.Users.Where(u => u.UserName.ToLower().Contains(query.ToLower())).ToList();

            return PartialView("_UsersResult", result);
        }

        public ActionResult AddAdmin(string id)
        {
            userManager.AddToRole(id, "Admin");

            return RedirectToAction("Users");
        }

        public ActionResult RemoveAdmin(string id)
        {
            userManager.RemoveFromRole(id, "Admin");

            return RedirectToAction("Users");
        }
    }
}