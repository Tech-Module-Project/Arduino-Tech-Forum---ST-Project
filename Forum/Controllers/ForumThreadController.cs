using System.Web.Mvc;

namespace Forum.Controllers
{

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading;

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
            var thread = this.db.Threads.FirstOrDefault(t => t.Id == id);

            if (thread == null)
            {
                TempData["NotificationMessage"] = "Cannot found thread with id " + id;
                TempData["NotificationType"] = "error";

                return RedirectToAction("Index", "Home");
            }

            var registeredUserAnswers = this.db.RegisteredUsersAnswer
                .Include(a => a.Replies)
                .Include(a => a.Author)
                .Where(a => a.ForumThread.Id == thread.Id)
                .ToList();

            var anonymousUserAnswers = this.db.AnonymousUsersAnswer
                .Include(a => a.Replies)
                .Where(a => a.ForumThread.Id == thread.Id)
                .ToList();

            var allUserAnswers = new List<IAnswer>();

            allUserAnswers.AddRange(registeredUserAnswers);
            allUserAnswers.AddRange(anonymousUserAnswers);

            allUserAnswers.ForEach(a => a.ForumThread = thread);
            
            var viewModel = new ForumThreadDetailsModelView()
                            {
                                Thread = thread,
                                Answers = allUserAnswers.OrderBy(a => a.CreationDate)
                                    .ToList()
                            };
            
            return View(viewModel);
        }

        [HttpPost]
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
    }
}