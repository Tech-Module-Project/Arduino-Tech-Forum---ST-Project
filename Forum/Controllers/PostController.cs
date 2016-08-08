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

        [HttpGet]
        public ActionResult ShowRecentPostsInCategory(int? id)
        {
            var dbContext = ApplicationDbContext.Create();
            List<ForumThread> posts;

            if (id == null)
            {
                posts = dbContext.Threads
                    .Include(t => t.Author)               
                    .Take(DefaultMaxThreadsToShowForCategory)
                    .ToList();
            }
            else
            {
                var category = dbContext.Categories.Include(c => c.Threads)
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
    }
}