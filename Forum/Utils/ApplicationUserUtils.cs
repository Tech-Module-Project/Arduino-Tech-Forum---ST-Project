namespace Forum.Extensions
{

    using System.Web;

    using Forum.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    public static class ApplicationUserUtils
    {
        
        public static ApplicationUser GetCurrentlyLoggedInUser()
        {    
            var loggedInUserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(loggedInUserId);
            return user;
        }
    }
}