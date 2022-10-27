using Microsoft.AspNetCore.Mvc;

namespace RedditNet.Controllers
{
    public class PostsController : Controller
    {
        //TODO
        //Implement CRUD for posts
        public IActionResult Index()
        {
            return View();
        }
    }
}
