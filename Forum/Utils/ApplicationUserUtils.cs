using System.Collections.Generic;
using System.Linq;
using Forum.Models.Answers;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Forum.Extensions
{

    using System.Web;

    using Forum.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    public class ApplicationUserUtils
    {
        private static ApplicationDbContext db = ApplicationDbContext.Create();

        public static ApplicationUser GetCurrentlyLoggedInUser()
        {
            var loggedInUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            bool isLoggedIn = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            var user = db.Users.Find(loggedInUserId);
            if (isLoggedIn)
            {
                var userThreads = db.Threads.Where(x => x.Author.UserName.Equals(user.UserName));
                var userAnswers = db.RegisteredUsersAnswer.Where(x => x.Author.UserName.Equals(user.UserName));
                List<IAnswer> userAnswersList = new List<IAnswer>(userAnswers);
                user.PostedThreads = userThreads.ToList();
                user.PostedAnswers = userAnswersList;
            }
            return user;
        }

        public static bool IsAdmin(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (userManager.IsInRole(id, "Admin"))
            {
                return true;
            }
            return false;
        }

        public static bool isBanned(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            if (userManager.IsInRole(id, "Banned"))
            {
                return true;
            }

            return false;
        }
    }
}