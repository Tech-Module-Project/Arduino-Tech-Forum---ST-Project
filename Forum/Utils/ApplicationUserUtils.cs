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
            var user = db.Users.Find(loggedInUserId);
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