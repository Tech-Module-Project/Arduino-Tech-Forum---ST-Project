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
            var dbContext = ApplicationDbContext.Create();
            var categories = dbContext.Categories;
            return View(categories);
        }

        // GET: Users
        public ActionResult Ranking()
        {
            ViewBag.Message = "Your application description page.";

            var users = db.Users.OrderByDescending(u => u.PointCount).ToList();

            ViewBag.Users = users;

            return View();
        }

    }
}