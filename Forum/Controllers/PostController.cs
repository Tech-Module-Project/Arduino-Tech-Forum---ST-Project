using System.Web.Mvc;

namespace Forum.Controllers
{

    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using Forum.Models;

    using WebGrease.Css.Extensions;

    public class PostController : Controller
    {
        const int DefaultMaxThreadsToShowForCategory = 10;

        private ApplicationDbContext db = ApplicationDbContext.Create();

        [HttpGet]
        public ActionResult ShowRecentPostsInCategory(int? id)
        {
            List<ForumThread> posts;

            if (id == null)
            {
                posts = db.Threads
                    .Include(t => t.Author)
                    .Take(DefaultMaxThreadsToShowForCategory)
                    .ToList();
            }
            else
            {
                var category = db.Categories.Include(c => c.Threads)
                    .Include(c => c.Threads.Select(t => t.Author))
                    .FirstOrDefault(c => c.Id == id);

                if (category == null)
                {
                    TempData["NotificationMessage"] = "Cannot found category";
                    TempData["NotificationType"] = "error";
                    return null;
                }

                posts = category
                    .Threads
                    .Take(DefaultMaxThreadsToShowForCategory)
                    .ToList();
            }

            return PartialView(posts);
        }

        public ActionResult Details(int? id)
        {
            var thread = this.db.Threads.FirstOrDefault(t => t.Id == id);

            if (thread == null)
            {
                TempData["NotificationMessage"] = "Cannot found thread with id " + id;
                TempData["NotificationType"] = "error";

                return RedirectToAction("Index", "Home");
            }

            return this.View(thread);
        }
    }
}