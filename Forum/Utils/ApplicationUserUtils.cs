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
    }
}