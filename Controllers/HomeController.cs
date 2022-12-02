using Microsoft.AspNetCore.Mvc;
using RedditNet.CommentFolder;
using RedditNet.DataLayerFolder;
using RedditNet.Models;
using RedditNet.Models.DatabaseModel;
using RedditNet.PostFolder;
using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;

namespace RedditNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AppDbContext db;
        public HomeController(AppDbContext context, ILogger<HomeController> logger)
        {
            db = context;
            _logger = logger;
        }
        public IActionResult Index()
        {

            //try
            //{
            //    Post post = new Post("This is my first post!", "regular", "Hello guys!", "subid", "postid2");

            //    List<CommentNode> n = new List<CommentNode>();
            //    List<Comment> nc = new List<Comment>();

            //    n.Add(post.Root);
            //    nc.Add(new Comment(post.Id, post.Root.Id, "user 1", "root text"));

            //    for (int i = 2; i <= 8; ++i)
            //    {
            //        CommentNode node = new CommentNode(i);
            //        Comment comment = new Comment(post.Id, i, "user " + i.ToString(), "comment " + i.ToString());
            //        n.Add(node);
            //        nc.Add(comment);
            //    }


            //    n[1].makeChildOf(n[0]);
            //    n[2].makeChildOf(n[1]);
            //    n[3].makeChildOf(n[2]);
            //    n[4].makeChildOf(n[2]);
            //    n[5].makeChildOf(n[1]);
            //    n[6].makeChildOf(n[1]);
            //    n[7].makeChildOf(n[6]);

            //    DatabaseMapper mapper = new DatabaseMapper();
            //    List<DatabaseComment> coms = new List<DatabaseComment>();

            //    for (int i = 0; i < n.Count; ++i)
            //    {
            //        DatabaseComment c = mapper.toDBComment(n[i], nc[i]);
            //        Console.WriteLine(c.UserId);
            //        db.Comments.Add(c);
            //    }

            //    db.SaveChanges();
            //    return View();
            //}
            //catch (Exception)
            //{
            //    return View();
            //}

            return View();
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