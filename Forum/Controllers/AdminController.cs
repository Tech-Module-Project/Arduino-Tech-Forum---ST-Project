using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Forum.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Forum.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private  ApplicationDbContext db = new ApplicationDbContext();

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
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            userManager.AddToRole(id, "Admin");

            TempData["NotificationMessage"] = "User has been added to the admin role";
            TempData["NotificationType"] = "success";

            return RedirectToAction("Users");
        }

        public ActionResult RemoveAdmin(string id)
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            userManager.RemoveFromRole(id, "Admin");

            TempData["NotificationMessage"] = "User has been removed from the admin role";
            TempData["NotificationType"] = "info";

            return RedirectToAction("Users");
        }

        public ActionResult BanUser(string id)
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            userManager.AddToRole(id, "Banned");

            TempData["NotificationMessage"] = "User has been successfuly banned";
            TempData["NotificationType"] = "success";

            return RedirectToAction("Users");
        }

        public ActionResult UnbanUser(string id)
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            userManager.RemoveFromRole(id, "Banned");

            TempData["NotificationMessage"] = "User has been unbanned";
            TempData["NotificationType"] = "info";

            return RedirectToAction("Users");
        }

        public ActionResult Categories()
        {
            var categories = db.Categories.ToList();

            return View("~/Views/Admin/Category/Categories.cshtml", categories);
        }

        //GET category
        public ActionResult CreateCategory()
        {
            return View("~/Views/Admin/Category/CreateCategory.cshtml");
        }

        //POST Create new category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Categories");
            }

            return View("~/Views/Admin/Category/CreateCategory.cshtml", category);
        }

        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View("~/Views/Admin/Category/EditCategory.cshtml", category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Categories");
            }
            return View("~/Views/Admin/Category/EditCategory.cshtml", category);
        }

        public ActionResult DeleteCategory()
        {
            return View("~/Views/Admin/Category/DeleteCategory.cshtml");
        }

        [HttpPost]
        public ActionResult DeleteCategory(int? id)
        {
            Category category = db.Categories.Find(id);
            
            db.Threads.RemoveRange(category.Threads);
            
            db.Categories.Remove(category);

            db.SaveChanges();
            return RedirectToAction("Categories");

        }

        public ActionResult DeleteUser()
        {
            return View("~/Views/Admin/User/DeleteUser.cshtml");
        }

        [HttpPost]
        public ActionResult DeleteUser(string id)
        {
            var user = db.Users.Find(id);

            db.Threads.RemoveRange(user.PostedThreads);

            var userAnswers = db.RegisteredUsersAnswer.Where(a => a.Author_Id == id);

            db.Answer.RemoveRange(userAnswers);

            db.Users.Remove(user);

            db.SaveChanges();

            return RedirectToAction("Users");
        }

    }
}