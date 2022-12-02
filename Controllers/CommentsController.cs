using Microsoft.AspNetCore.Mvc;
using RedditNet.CommentFolder;
using RedditNet.DataLayerFolder;
using RedditNet.Models.CommentModel;
using RedditNet.Models.DatabaseModel;
using RedditNet.UtilityFolder;

namespace RedditNet.Controllers
{
    public class CommentsController : Controller
    {
        private DataLayerComments dbComments;
        public CommentsController(AppDbContext context)
        {
            dbComments = new DataLayerComments(context);
        }

        [HttpGet("{postId}/comments/{commentId}")]
        public IActionResult Show(String postId, int commentId)
        {
            DatabaseComment? dbComment = dbComments.readComment(postId, commentId);
            if (dbComment != null)
            {
                DatabaseMapper mapper = new DatabaseMapper();
                CommentThreadModel cm = mapper.toThreadComment(dbComment);

                return View(cm);
            }

            return View("Error");
        }

        //[HttpPut("{postId}/comments/{commentId}")]
        //public IActionResult Edit(String postId, int commentId, [FromBody] CommentUpdateModel c)
        //{
        //    // TODO 
        //    // CHECK IF USER ALREADY VOTED
        //    // POSSIBLE RACE CONDITION
        //    d.updateComment(postId, commentId, c);

        //    return Ok();
        //}

        [HttpGet("{postId}/comments/reply/{commentId}")]

        public IActionResult CreateForm(String postId, int commentId)
        {
            CommentModel cm = new CommentModel();
            cm.PostId = postId;
            cm.Parent = commentId;

            return View("Create", cm);
        }

        [HttpGet("{postId}/comments")]
        public IActionResult ListComments(String postId)
        {
            List<CommentNode>? nodes = dbComments.getDescendants(postId);
            if (nodes == null)
                return View("Error");

            List<CommentThreadModel> result = new List<CommentThreadModel>();
            foreach (CommentNode n in nodes)
            {
                DatabaseMapper mapper = new DatabaseMapper();
                DatabaseComment? dbc = dbComments.readComment(postId, n.Id);
                if (dbc != null)
                {
                    result.Add(mapper.toThreadComment(dbc));
                }
            }

            return View("AllComments", result);
        }


        [HttpPost("{postId}/comments/{commentId}")]
        public IActionResult Create(String postId, int commentId, CommentCreateModel c)
        {
            CommentMapper mapper = new CommentMapper();
            Comment comment = mapper.createToComment(postId, c);
            CommentNode n = new CommentNode(commentId);

            DatabaseComment? dbComment = dbComments.createComment(n, comment);

            if (dbComment != null)
                return RedirectToAction("Show", new { postId = dbComment.PostId, commentId = dbComment.Id});
            Console.WriteLine("Bad");
            return BadRequest();
        }

        //[HttpDelete("{postId}/comments/{commentId}")]
        //public IActionResult Delete(String postId, int commentId, [FromBody] CommentDeleteModel c)
        //{
        //    d.deleteComment(postId, commentId, c);
        //    return Ok();
        //}

    }
}
