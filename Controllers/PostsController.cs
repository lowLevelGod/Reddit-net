using Microsoft.AspNetCore.Mvc;
using RedditNet.CommentFolder;
using RedditNet.DataLayerFolder;
using RedditNet.Models.CommentModel;
using RedditNet.Models.PostModel;
using RedditNet.PostFolder;
using RedditNet.SubRedditFolder;
using RedditNet.UserFolder;
using RedditNet.UtilityFolder;

namespace RedditNet.Controllers
{
    public class PostsController : Controller
    {
        private static DataLayerPosts dp = DatabaseInterface.dataLayerPosts;
        private static DataLayerUsers du = DatabaseInterface.dataLayerUsers;
        private static DataLayerSubReddits ds = DatabaseInterface.dataLayerSubReddits;
        private static int init = 0;
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
        //[HttpGet("{subId}/posts/{postId}/comments")]
        //public IActionResult Show(String subId, String postId)
        //{
        //    List<CommentThreadModel> comments = getComments(subId, postId, Constants.comparisonByTimeDesc);
        //    Post post = dp.readPost(subId, postId);
        //    if (postId != null)
        //    {
        //        PostMapper mapper = new PostMapper();
        //        PostThreadModel result = mapper.toThreadModel(comments, post);

        //        return View(result);
        //    }

        //    return View("Error");
        //}

        //[HttpPut("{subId}/posts/{postId}")]
        //public IActionResult Edit(String subId, String postId, [FromBody] PostUpdateModel p)
        //{
        //    dp.updatePost(subId, postId, p);
        //    return Ok();
        //}

        //[HttpPost("posts")]
        //public IActionResult Create([FromBody] PostCreateModel p)
        //{
        //    PostMapper mapper = new PostMapper();
        //    Post post = mapper.createModelToPost(p);

        //    dp.createPost(post);
        //    Console.WriteLine(post.Id);
        //    return Ok();
        //}

        //[HttpDelete("{subId}/posts/{postId}")]
        //public IActionResult Delete(String subId, String postId, [FromBody] PostDeleteModel p)
        //{
        //    dp.deletePost(subId, postId, p);
        //    return Ok();
        //}
    }
}
