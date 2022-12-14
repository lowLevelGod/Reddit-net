using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using RedditNet.CommentFolder;
using RedditNet.DataLayerFolder;

using RedditNet.Models.CommentModel;
using RedditNet.Models.DatabaseModel;
using RedditNet.Models.PostModel;
using RedditNet.PostFolder;
using RedditNet.SubRedditFolder;
using RedditNet.UserFolder;
using RedditNet.UtilityFolder;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;

namespace RedditNet.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private DataLayerPosts dbPosts;
        private DataLayerComments dbComments;
        private DataLayerSubReddits dbSubs;
        private DataLayerUsers dbUsers;

        private readonly UserManager<DatabaseUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public PostsController(
            AppDbContext context,
            UserManager<DatabaseUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            dbPosts = new DataLayerPosts(context);
            dbComments = new DataLayerComments(context);
            dbSubs = new DataLayerSubReddits(context);
            dbUsers = new DataLayerUsers(userManager, roleManager);

            _userManager = userManager;
            _roleManager = roleManager;
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
        [Authorize(Roles = "Regular,Moderator,Admin")]
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
                    Console.WriteLine(dbc.Text);
                    comments.Add(mapper.toThreadComment(dbc, subId));
                }
            }

            DatabasePost? dbPost = dbPosts.readPost(subId, postId);
            
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
            }
            
            if (dbPost != null)
            {
                PostMapper pmapper = new PostMapper();
                DatabaseMapper databaseMapper = new DatabaseMapper();

                DatabaseSubReddit? sub = dbSubs.readSubReddit(subId);
                PostThreadModel result = pmapper.toThreadModel(
                    comments, 
                    databaseMapper.toPost(dbPost), sub == null ? "" : sub.Name,
                    dbPost.User.UserName);

                ViewBag.ByTimeAsc = Constants.comparisonByTimeAsc;
                ViewBag.ByTimeDesc = Constants.comparisonByTimeDesc;
                ViewBag.ByVotesAsc = Constants.comparisonByVotesAsc;
                ViewBag.ByVotesDesc = Constants.comparisonByVotesDesc;

                SetAccessRights();
                
                return View("Show", result);
            }

            return View("Error");
        }

        //
        private void SetAccessRights()
        {
            
            ViewBag.AfisareButoane = false;
            if (User.IsInRole("Moderator")||User.IsInRole("Regular"))
            {
                ViewBag.AfisareButoane = true;
            }
            ViewBag.UserCurent = _userManager.GetUserId(User);
            ViewBag.EsteAdmin = User.IsInRole("Admin");
            ViewBag.EsteModerator = User.IsInRole("Moderator");
            
        }
        //

        [Authorize(Roles = "Regular,Moderator,Admin")]
        [HttpPost("{subId}/posts/edit/{postId}")]
        public IActionResult Edit(String subId, String postId, PostUpdateModel p)
        {
            
            DatabasePost? dbPost = dbPosts.updatePost(subId, postId, p);

            if (dbPost != null && p.Text != null)
            {
                if (_userManager.GetUserId(User)==dbPost.User.Id || User.IsInRole("Admin"))
                {
                    TempData["message"] = "Your post has been modified";

                    return RedirectToAction("Show", new { subId = dbPost.SubId, postId = dbPost.Id });
                }
                else
                {
                    TempData["message"] = "You can't edit a post that is not yours";
                    return RedirectToAction("Show", new { subId = dbPost.SubId, postId = dbPost.Id });
                }
                
            }

            else
            {
                PostUpdateModel pm = new PostUpdateModel();
                
                pm.SubId =subId;
                pm.UserId = _userManager.GetUserId(User);
                pm.Id = postId;
                pm.Title = p.Title;
               
                return View(pm);
            }    


            /*var dbPost = await dbPosts.UpdatePost(subId, postId,p);

            if(p.Text==null)
            {
                //mesaj validare care va fi afisat in show
                TempData["message"] = "Your post cannot be changed";
                //
                return RedirectToAction("Show", new { subId = dbPost.SubId, postId = dbPost.Id });
                
            }

            //mesaj validare care va fi afisat in show
            TempData["message"] = "Your post has been modified";
            //
            return RedirectToAction("Show", new { subId = dbPost.SubId, postId = dbPost.Id });*/

            /*DatabasePost? dbPost = dbPosts.updatePost(subId, postId, p);
            
            if (dbPost == null|| p.Text == null)
            {
                return View(p);
            }
            
            //mesaj validare care va fi afisat in show
            TempData["message"] = "Your post has been modified";
            //
            return RedirectToAction("Show", new { subId = dbPost.SubId, postId = dbPost.Id });*/



        }
        [Authorize(Roles ="Regular,Moderator,Admin")]
        [HttpGet("{subId}/posts/edit/{postId}", Name = "EditPost")]
        public IActionResult EditForm(String subId, String postId)
        {
            DatabasePost? post = dbPosts.readPost(subId, postId);
            DatabaseUser? user = dbUsers.readUser(_userManager.GetUserId(User));


            if (post != null)
            {
                PostUpdateModel pm = new PostUpdateModel();
                pm.Text = post.Text;
                pm.SubId = post.SubId;
                pm.UserId = post.User.Id;
                pm.Votes = post.Votes;
                pm.Id = post.Id;
                pm.Title = post.Title;
                
                if (pm.UserId==_userManager.GetUserId(User)|| User.IsInRole("Admin"))
                {
                    
                    return View("Edit", pm);
                }
                else
                {
                    TempData["message"] = "You can't edit a post that is not yours";
                    return RedirectToAction("Show", new { subId = pm.SubId, postId = pm.Id });
                }

                
            }
            return View("Error");
        }

        [Authorize(Roles = "Regular,Moderator,Admin")]
        [HttpPost("{subId}/posts/create")]
        public IActionResult Create(PostCreateModel p)
        {
            PostMapper mapper = new PostMapper();
            Post post = mapper.createModelToPost(p);

            DatabaseUser? user = dbUsers.readUser(_userManager.GetUserId(User));
            Console.WriteLine(user.Id);
            DatabasePost? dbPost = dbPosts.createPost(post, user);
            if (dbPost != null)
            {
                //mesaj validare care va fi afisat in show
                TempData["message"] = "Your post has been added";
                //
                return RedirectToAction("Show", new { subId = dbPost.SubId, postId = dbPost.Id });
            }
            else
            {
                return View(p);
            }

            
        }

        [Authorize(Roles = "Regular,Moderator,Admin")]
        [HttpGet("{subId}/posts/create")]
        public IActionResult CreateForm(String subId)
        {
            PostCreateModel cm = new PostCreateModel();
            cm.Title = "";
            cm.Text = "";
            cm.UserId = _userManager.GetUserId(User);
            cm.SubId = subId;

            return View("Create", cm);
        }

        [Authorize(Roles ="Regular,Moderator,Admin")]
        [HttpPost("{subId}/posts/delete/{postId}")]
        public IActionResult Delete(String subId, String postId, PostDeleteModel p)
        {
            DatabasePost? post = dbPosts.readPost(subId, postId);
            if (post.User.Id == _userManager.GetUserId(User) || User.IsInRole("Admin")|| User.IsInRole("Moderator"))
            {
                if (dbPosts.deletePost(subId, postId, p) == true)
                {
                    TempData["message"] = "The post has been deleted";

                    return Redirect("/subs/" + subId + "/posts/0");
                }
                return BadRequest();
            }
            else
            {
                TempData["message"] = "You can't delete a post that is not yours";

                return Redirect("/subs/" + subId + "/posts/0");
            }
                
        }

        [Authorize(Roles = "Regular,Moderator,Admin")]
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

                if (pm.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin")|| User.IsInRole("Moderator"))
                    return View("Delete", pm);
                else
                {
                    TempData["message"] = "You can't delete a post that is not yours";
                    return Redirect("/subs/" + subId + "/posts/0");
                }
                
            }

            return View("Error");
        }
    }
}
