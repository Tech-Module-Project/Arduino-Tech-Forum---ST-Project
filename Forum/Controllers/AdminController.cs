using System;
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
        protected UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users()
        {
            var users = db.Users.ToList();

            return View("~/Views/Admin/User/Users.cshtml", users);
        }

        public ActionResult SearchUsers(string query)
        {
            var resultUsers = db.Users.Where(u => u.UserName.ToLower().Contains(query.ToLower())).ToList();

            return PartialView("~/Views/Admin/User/_UsersResult.cshtml", resultUsers);

        }

        public ActionResult SearchCategories(string query)
        {

            var resultCategory = db.Categories.Where(c => c.Name.ToLower().Contains(query.ToLower())).ToList();

            return PartialView("~/Views/Admin/Category/_CategoriesResult.cshtml", resultCategory);
        }

        public ActionResult AddAdmin(string id)
        {
            userManager.AddToRole(id, "Admin");

            TempData["NotificationMessage"] = "User has been added to the admin role";
            TempData["NotificationType"] = "success";

            return RedirectToAction("Users");
        }

        public ActionResult RemoveAdmin(string id)
        {
            userManager.RemoveFromRole(id, "Admin");

            TempData["NotificationMessage"] = "User has been removed from the admin role";
            TempData["NotificationType"] = "info";

            return RedirectToAction("Users");
        }

        public ActionResult Categories()
        {
            var categories = db.Categories.ToList();

            return View("~/Views/Admin/Category/Categories.cshtml", categories);
        }
    }
}