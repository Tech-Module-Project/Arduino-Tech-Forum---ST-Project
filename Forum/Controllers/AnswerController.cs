namespace Forum.Controllers
{

    using System;
    using System.Data.Entity;
    using System.Web.Mvc;

    using Forum.Extensions;
    using Forum.Models;
    using Forum.Models.Answers;

    using Microsoft.Ajax.Utilities;

    public class AnswerController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplyToAnswer(int? forumThreadId, int? parentAnswerId, string parentAnswerType, string replyBody, string email, string previousPageUrl)
        {
            var thread = this.db.Threads.Find(forumThreadId);
            AnswerBase parentAnswer = null;
            AnswerBase answer = null;

            if (thread == null)
            {
                TempData["NotificationMessage"] = "Cannot find thread";
                TempData["NotificationType"] = "error";

                return Redirect(previousPageUrl);
            }

            switch (parentAnswerType)
            {
                case "AnonymousUserAnswer":
                    parentAnswer = this.db.AnonymousUsersAnswer.Find(parentAnswerId);
                    break;
                case "RegisteredUserAnswer":
                    parentAnswer = this.db.RegisteredUsersAnswer.Find(parentAnswerId);
                    break;
                    
                default:
                    TempData["NotificationMessage"] = "Unknown answer type";
                    TempData["NotificationType"] = "error";

                    return Redirect(previousPageUrl);
            }

            if (parentAnswer == null)
            {
                TempData["NotificationMessage"] = "Cannot find answer";
                TempData["NotificationType"] = "error";

                return Redirect(previousPageUrl);
            }

            if (replyBody.IsNullOrWhiteSpace())
            {
                TempData["NotificationMessage"] = "Answer body is empty";
                TempData["NotificationType"] = "error";

                return Redirect(previousPageUrl);
            }

            if (User.Identity.IsAuthenticated)
            {
                var loggedInUser = ApplicationUserUtils.GetCurrentlyLoggedInUser();

                this.db.Entry(loggedInUser).State = EntityState.Unchanged;
                
                answer = new RegisteredUserAnswer()
                         {
                             Author = loggedInUser,
                             Body = replyBody,
                             CreationDate = DateTime.Now,
                             ForumThread = thread
                         };
            }
            else
            {
                answer = new AnonymousUserAnswer()
                         {
                             Body = replyBody,
                             CreationDate = DateTime.Now,
                             Email = email,
                             ForumThread = thread
                         };
            }

            parentAnswer.Replies.Add(answer);

            this.db.SaveChanges();

            return this.Redirect(previousPageUrl);
        }
    }

}