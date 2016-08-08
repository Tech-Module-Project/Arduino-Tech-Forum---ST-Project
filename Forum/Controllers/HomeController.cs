using System.Web.Mvc;

namespace Forum.Controllers
{

    using Forum.Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var dbContext = ApplicationDbContext.Create();
            var categories = dbContext.Categories;
            return View(categories);
        }
        
    }
}