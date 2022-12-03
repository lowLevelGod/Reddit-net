using Microsoft.AspNetCore.Mvc;
using RedditNet.CommentFolder;
using RedditNet.DataLayerFolder;
using RedditNet.Models.CommentModel;
using RedditNet.Models.DatabaseModel;
using RedditNet.Models.PostModel;
using RedditNet.PostFolder;
using RedditNet.SubRedditFolder;
using RedditNet.UserFolder;
using RedditNet.UtilityFolder;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;

namespace RedditNet.Controllers
{
    public class PostsController : Controller
    {
        private DataLayerPosts dbPosts;
        private DataLayerComments dbComments;
        private DataLayerSubReddits dbSubs;
        private DataLayerUsers dbUsers;
        public PostsController(AppDbContext context)
        {
            dbPosts = new DataLayerPosts(context);
            dbComments = new DataLayerComments(context);
            dbSubs = new DataLayerSubReddits(context);
            dbUsers = new DataLayerUsers(context);
        }
        //TODO
        //Use [deleted] for posts with deleted user
        //Add edit for admins
        //Introduce tokens for authentication
        public IActionResult Index()
        {
            return View();
        }

        [NonAction]
        private List<CommentThreadModel> getComments(String subId, String postId, int cmpMethod)
        {
            //if (init == 0)
            //{
            //    init = 1;

            //    User admin = new User("admin user", "1234", "admin@bmail.com", "I'm an admin!", Constants.admin, "admin");
            //    User mod = new User("mod user", "5678", "mod@bmail.com", "I'm a mod!", Constants.mod, "mod");
            //    User regular = new User("regular user", "8910", "regular@bmail.com", "I'm a regular!", Constants.regular, "regular");


            //    du.createUser(admin);
            //    du.createUser(mod);
            //    du.createUser(regular);

            //    SubReddit sub = new SubReddit("r/Romania", "Welcome to r/Romania!", "subid");
            //    ds.createSubReddit(sub, admin.Id);

            //    Post post = new Post("This is my first post!", "regular", "Hello guys!", "subid", "postid");
            //    dp.createPost(post);


            //    Dictionary<String, Dictionary<int, CommentNode>> tree = DatabaseInterface.treeNodes;
            //    Dictionary<String, Dictionary<int, Comment>> comments = DatabaseInterface.comments;

            //    List<CommentNode> n = new List<CommentNode>();
            //    List<Comment> nc = new List<Comment>();

            //    n.Add(post.Root);
            //    nc.Add(new Comment(post.Id, post.Root.Id));

            //    for (int i = 1; i <= 7; ++i)
            //    {
            //        DatabaseInterface.idGenerator++;
            //        CommentNode node = new CommentNode(DatabaseInterface.idGenerator);
            //        Comment comment = new Comment(post.Id, DatabaseInterface.idGenerator, "user " + DatabaseInterface.idGenerator.ToString(), "comment " + DatabaseInterface.idGenerator.ToString());
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

            //    for (int i = 0; i <= 7; ++i)
            //        d.createComment(n[i], nc[i]);
            //}

            //List<CommentNode> desc = d.getDescendants(postId, 0, cmpMethod);

            //List<CommentThreadModel> result = new List<CommentThreadModel>();
            //CommentMapper mapper = new CommentMapper();

            //foreach (var x in desc)
            //{
            //    if (x.Parent != Constants.noParent)
            //    {
            //        CommentThreadModel t = mapper.toThreadModel(d.readComment(postId, x.Id), x.Depth);
            //        result.Add(t);
            //    }
            //}

            return null;
        }

        [HttpGet("{subId}/posts/{postId}/comments/{sortType?}", Name = "ShowPostComments")]
        public IActionResult Show(String subId, String postId, int? sortType)
        {

            List<CommentNode>? nodes = dbComments.getDescendants(postId, -1, sortType == null ? Constants.comparisonByTimeDesc : (int)sortType);
            if (nodes == null)
                return View("Error");

            List<CommentThreadModel> comments = new List<CommentThreadModel>();
            foreach (CommentNode n in nodes)
            {
                DatabaseMapper mapper = new DatabaseMapper();
                DatabaseComment? dbc = dbComments.readComment(postId, n.Id);
                if (dbc != null)
                {
                    comments.Add(mapper.toThreadComment(dbc, subId));
                }
            }

            DatabasePost? dbPost = dbPosts.readPost(subId, postId);

            if (dbPost != null)
            {
                PostMapper pmapper = new PostMapper();
                DatabaseMapper databaseMapper = new DatabaseMapper();

                DatabaseSubReddit? sub = dbSubs.readSubReddit(subId);
                PostThreadModel result = pmapper.toThreadModel(comments, databaseMapper.toPost(dbPost), sub == null ? "" : sub.Name);

                ViewBag.ByTimeAsc = Constants.comparisonByTimeAsc;
                ViewBag.ByTimeDesc = Constants.comparisonByTimeDesc;
                ViewBag.ByVotesAsc = Constants.comparisonByVotesAsc;
                ViewBag.ByVotesDesc = Constants.comparisonByVotesDesc;

                return View("Show", result);
            }

            return View("Error");
        }

        [HttpPost("{subId}/posts/edit/{postId}")]
        public IActionResult Edit(String subId, String postId, PostUpdateModel p)
        {
            DatabasePost? dbPost = dbPosts.updatePost(subId, postId, p);
            if (dbPost != null)
                return RedirectToAction("Show", new { subId = dbPost.SubId, postId = dbPost.Id });

            return BadRequest();
        }

        [HttpGet("{subId}/posts/edit/{postId}", Name = "EditPost")]
        public IActionResult EditForm(String subId, String postId)
        {
            DatabasePost? post = dbPosts.readPost(subId, postId);
            if (post != null)
            {
                PostUpdateModel pm = new PostUpdateModel();
                pm.Text = post.Text;
                pm.SubId = post.SubId;
                pm.UserId = post.User.Id;
                pm.Votes = post.Votes;
                pm.Id = post.Id;
                pm.Title = post.Title;

                return View("Edit", pm);
            }
            return View("Error");
        }

        [HttpPost("{subId}/posts/create")]
        public IActionResult Create(PostCreateModel p)
        {
            PostMapper mapper = new PostMapper();
            Post post = mapper.createModelToPost(p);

            DatabasePost? dbPost = dbPosts.createPost(post, p.UserId);
            if (dbPost != null)
            {
                return RedirectToAction("Show", new { subId = dbPost.SubId, postId = dbPost.Id });
            }
            return BadRequest();
        }

        [HttpGet("{subId}/posts/create")]
        public IActionResult CreateForm(String subId)
        {
            PostCreateModel cm = new PostCreateModel();
            cm.Title = "";
            cm.Text = "";
            cm.UserId = "";
            cm.SubId = subId;

            return View("Create", cm);
        }

        [HttpPost("{subId}/posts/delete/{postId}")]
        public IActionResult Delete(String subId, String postId, PostDeleteModel p)
        {
            if (dbPosts.deletePost(subId, postId, p) == true)
                return Ok();
            return BadRequest();
        }

        [HttpGet("{subId}/posts/delete/{postId}", Name = "DeletePost")]
        public IActionResult DeleteForm(String subId, String postId, PostDeleteModel p)
        {
            DatabasePost? post = dbPosts.readPost(subId, postId);
            if (post != null)
            {
                PostDeleteModel pm = new PostDeleteModel();
                pm.SubId = post.SubId;
                pm.UserId = post.User.Id;
                pm.Id = post.Id;
                pm.Title = post.Title;

                return View("Delete", pm);
            }

            return View("Error");
        }
    }
}
