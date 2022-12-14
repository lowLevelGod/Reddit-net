using Microsoft.AspNetCore.Mvc;
using RedditNet.CommentFolder;
using RedditNet.DataLayerFolder;
using RedditNet.Models;
using RedditNet.Models.DatabaseModel;
using RedditNet.Models.SubRedditModel;
using RedditNet.PostFolder;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

namespace RedditNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private DataLayerSubReddits dbSubs;
        public HomeController(AppDbContext context, ILogger<HomeController> logger)
        {
            dbSubs = new DataLayerSubReddits(context);
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            List<SubRedditPreviewModel>? result = dbSubs.getSubs();
            if (result != null)
            {
                if (TempData.ContainsKey("message"))
                {
                    ViewBag.message = TempData["message"].ToString();
                }
                return View(result);
            }
            return View("Error");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}