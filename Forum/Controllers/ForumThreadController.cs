using System.Web.Mvc;

namespace Forum.Controllers
{

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Forum.Extensions;
    using Forum.Models;
    using Forum.Models.Answers;

    using Microsoft.Ajax.Utilities;

    using WebGrease.Css.Extensions;

    public class ForumThreadController : Controller
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
            var thread = this.db.Threads.Include(t => t.Author)
                .Include(t => t.BestAnswer)
                .Include(t => t.Answers.Select(a => a.Replies))
                .Include(t => t.Category)
                .Include(t => t.Tags)
                .FirstOrDefault(t => t.Id == id);

            if (thread == null)
            {
                TempData["NotificationMessage"] = "Cannot found thread with id " + id;
                TempData["NotificationType"] = "error";

                return RedirectToAction("Index", "Home");
            }

            thread.ViewCount++;

            this.db.Entry(thread)
                .Property(t => t.ViewCount)
                .IsModified = true;
            this.db.SaveChanges();

            thread.Answers.ToList()
                .ForEach(
                    a =>
                        {
                            var answerTypeName = a.GetType()
                                .Name;

                            if (answerTypeName.Contains("RegisteredUserAnswer"))
                            {
                                var registeredUserAnswer = (RegisteredUserAnswer)a;
                                var author = this.db.RegisteredUsersAnswer.Include(rua => rua.Author)
                                    .First(rua => rua.Id == a.Id)
                                    .Author;

                                registeredUserAnswer.Author = author;
                            }
                        });

            thread.Answers = thread.Answers.Where(a => a.ParentAnswer == null).ToList();
            
            var viewModel = new ForumThreadDetailsModelView()
            {
                Thread = thread,
                Answers = thread.Answers.Select(a => a as IAnswer).ToList()
            };

  

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplyToThread(int? forumThreadId, string replyBody, string email, string previousPageUrl)
        {
            var thread = this.db.Threads.Find(forumThreadId);
            AnswerBase answer = null;

            if (thread == null)
            {
                TempData["NotificationMessage"] = "Thread doesnt exits";
                TempData["NotificationType"] = "error";

                return RedirectToAction("Index", "Home");
            }

            if (replyBody.IsNullOrWhiteSpace())
            {
                var redirectUrl = previousPageUrl ?? "/";

                TempData["NotificationMessage"] = "Cannot post empty reply";
                TempData["NotificationType"] = "error";

                return Redirect(redirectUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                var loggedInUser = ApplicationUserUtils.GetCurrentlyLoggedInUser();
                var registeredUserReply = new RegisteredUserAnswer()
                {
                    Author = loggedInUser,
                    Body = replyBody,
                    CreationDate = DateTime.Now,
                    ForumThread = thread
                };

                this.db.RegisteredUsersAnswer.Add(registeredUserReply);
                this.db.Entry(loggedInUser).State = EntityState.Unchanged;

                answer = registeredUserReply;
            }
            else
            {
                if (email.IsNullOrWhiteSpace())
                {
                    var redirectUrl = previousPageUrl ?? "/";

                    TempData["NotificationMessage"] = "Must fill email field";
                    TempData["NotificationType"] = "error";

                    return Redirect(redirectUrl);
                }

                var anonymousUserReply = new AnonymousUserAnswer()
                {
                    Body = replyBody,
                    CreationDate = DateTime.Now,
                    Email = email,
                    ForumThread = thread
                };

                this.db.AnonymousUsersAnswer.Add(anonymousUserReply);

                answer = anonymousUserReply;
            }

            thread.Answers.Add(answer);

            this.db.SaveChanges();

            return Redirect("/ForumThread/Details/" + forumThreadId);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            var allCategories = this.db.Categories;
            return this.View(allCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(int? categoryId, string title, string body)
        {
            if (title.IsNullOrWhiteSpace() || title.Length < ForumThread.MinTitleLength)
            {
                TempData["NotificationMessage"] = "Title must be min" + ForumThread.MinTitleLength + " symbols long";
                TempData["NotificationType"] = "error";
                return this.View();
            }

            if (body.IsNullOrWhiteSpace() || body.Length < ForumThread.MinBodyLength)
            {
                TempData["NotificationMessage"] = "Body must be min " + ForumThread.MinBodyLength + " symbols long";
                TempData["NotificationType"] = "error";
                return this.View();
            }

            var category = this.db.Categories.Find(categoryId);

            if (category == null)
            {
                TempData["NotificationMessage"] = "Invalid category";
                TempData["NotificationType"] = "error";
                return View();
            }

            var loggedInUser = ApplicationUserUtils.GetCurrentlyLoggedInUser();
            var thread = new ForumThread()
                         {
                             Author = loggedInUser,
                             Body = body,
                             Title = title,
                             CreationDate = DateTime.Now,
                             LastModified = DateTime.Now,
                             Category = category
                         };

            this.db.Entry(category).State = EntityState.Unchanged;
            this.db.Entry(loggedInUser).State = EntityState.Unchanged;

            this.db.Threads.Add(thread);
            this.db.SaveChanges();

            TempData["NotificationMessage"] = "Successfully created thread";
            TempData["NotificationType"] = "success";

            var threadId = this.db.Entry(thread)
                .Entity.Id;

            return Redirect("/ForumThread/Details/" + threadId);
        }
    }
}