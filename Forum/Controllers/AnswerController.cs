namespace Forum.Controllers
{

    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    using Forum.Extensions;
    using Forum.Models;
    using Forum.Models.Answers;

    using Microsoft.Ajax.Utilities;

    public class AnswerController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
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

            try
            {
                parentAnswer = GetAnswer(parentAnswerId, parentAnswerType);
            }
            catch (ArgumentException exception)
            {
                TempData["NotificationMessage"] = exception.Message;
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

        AnswerBase GetAnswer(int? answerId, string answerType)
        {
            AnswerBase answer = null;

            if (string.IsNullOrWhiteSpace(answerType))
            {
                throw new ArgumentException("Invalid answer type");
            }

            if (answerType.Contains("AnonymousUserAnswer"))
            {
                answer = this.db.AnonymousUsersAnswer
                    .Include(a => a.ForumThread)
                    .Include(a => a.ParentAnswer)
                    .FirstOrDefault(a => a.Id == answerId);
            }
            else if (answerType.Contains("RegisteredUserAnswer"))
            {
                answer = this.db.RegisteredUsersAnswer.Include(a => a.Author)
                    .Include(a => a.ForumThread)
                    .Include(a => a.ParentAnswer)
                    .FirstOrDefault(a => a.Id == answerId);
            }

            if (answer == null)
            {
                throw new ArgumentException("Cant found answer with " + answerId + " id of " + answerType + " answer type");
            }

            return answer;
        }

        [HttpGet]
        [Authorize]
        public ActionResult UpVoteAnswer(int? answerId, string answerType)
        {
            var answer = GetAnswer(answerId, answerType);
            var positivePoints = answer.PositivePoints + 1;

            ChangeAnswerPoints(answer, positivePoints, answer.NegativePoints);

            var points = new
            {
                points = positivePoints - answer.NegativePoints
            };

            return Json(points, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult DownVoteAnswer(int? answerId, string answerType)
        {
            var answer = GetAnswer(answerId, answerType);
            var negativePoints = answer.NegativePoints + 1;

            ChangeAnswerPoints(answer, answer.PositivePoints, answer.NegativePoints + 1);

            var points = new
            {
                points = answer.PositivePoints - negativePoints
            };

            return Json(points, JsonRequestBehavior.AllowGet);
        }

        void ChangeAnswerPoints(AnswerBase answer, int newPositivePoints, int newNegativePoints)
        {
            answer.PositivePoints = newPositivePoints;
            answer.NegativePoints = newNegativePoints;

            if (answer.GetType().Name.Contains("RegisteredUserAnswer"))
            {
                var registeredUserAnswer = (RegisteredUserAnswer)answer;
                this.db.Entry(registeredUserAnswer.Author)
                    .State = EntityState.Unchanged;
            }

            this.db.Entry(answer.ForumThread).State = EntityState.Unchanged;

            if (answer.ParentAnswer != null)
            {
                this.db.Entry(answer.ParentAnswer).State = EntityState.Unchanged;
            }
            
            this.db.SaveChanges();
        }
    }

}