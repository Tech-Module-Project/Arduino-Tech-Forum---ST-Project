using System.Web.Mvc;
using System.Linq;

namespace Forum.Controllers
{

    using Forum.Models;
    [RequireHttps]
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

            //TODO: CALCULATE EACH PLAYER SCORE AND RETURN LIST OF PLAYERS SORTED DSC BY SCORE TO THE VIEW
            return View();
        }

        

    }
}