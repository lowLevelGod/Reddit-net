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

        [HttpPost("{postId}/comments/edit/{commentId}")]
        public IActionResult Edit(String postId, int commentId, CommentUpdateModel c)
        {
            DatabaseComment? result = dbComments.updateComment(postId, commentId, c);  
            if (result != null)
                return RedirectToAction("Show", new { postId = result.PostId, commentId = result.Id });

            return BadRequest();
        }

        [HttpGet("{postId}/comments/edit/{commentId}")]
        public IActionResult EditForm(String postId, int commentId)
        {
            DatabaseComment? dbc = dbComments.readComment(postId, commentId);
            if (dbc != null)
            {
                CommentUpdateModel cm = new CommentUpdateModel();
                cm.PostId = dbc.PostId;
                cm.UserId = dbc.UserId;
                cm.Text = dbc.Text;
                cm.Votes = dbc.Votes;
                cm.Id = dbc.Id;

                return View("Edit", cm);
            }

            return View("Error");
        }


        [HttpGet("{postId}/comments/reply/{commentId}")]

        public IActionResult CreateForm(String postId, int commentId)
        {
            CommentCreateModel cm = new CommentCreateModel();
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

            return BadRequest();
        }

        [HttpPost("{postId}/comments/delete/{commentId}")]
        public IActionResult Delete(String postId, int commentId, CommentDeleteModel c)
        {
            if (dbComments.deleteComment(postId, commentId, c) == true)
                return RedirectToAction("ListComments", new { postId = c.PostId });

            return BadRequest();
        }

        [HttpGet("{postId}/comments/delete/{commentId}")]
        public IActionResult DeleteForm(String postId, int commentId)
        {
            DatabaseComment? dbc = dbComments.readComment(postId, commentId);
            if (dbc != null)
            {
                CommentDeleteModel cm = new CommentDeleteModel();
                cm.PostId = dbc.PostId;
                cm.UserId = dbc.UserId;
                cm.Id = dbc.Id;

                return View("Delete", cm);
            }

            return View("Error");
        }

    }
}
