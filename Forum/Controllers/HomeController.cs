using System.Web.Mvc;
using System.Linq;

namespace Forum.Controllers
{

    using Forum.Models;

    public class HomeController : Controller
    {
        protected readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var categories = db.Categories;
            return View(categories);
        }

        // GET: Users with points
        [OutputCache(Duration = 120, VaryByParam = "none")]
        public ActionResult Users()
        {
            ViewBag.Message = "Your application description page.";

            var users = db.Users.OrderByDescending(u => u.PointCount).ToList();

            

            return View(users);
        }

    }
}