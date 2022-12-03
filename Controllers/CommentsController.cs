using Microsoft.AspNetCore.Mvc;
using RedditNet.CommentFolder;
using RedditNet.DataLayerFolder;
using RedditNet.Models.CommentModel;
using RedditNet.Models.DatabaseModel;
using RedditNet.UtilityFolder;
using System.Xml.Linq;

namespace RedditNet.Controllers
{
    public class CommentsController : Controller
    {
        private DataLayerComments dbComments;
        public CommentsController(AppDbContext context)
        {
            dbComments = new DataLayerComments(context);
        }

        [HttpGet("{subId}/{postId}/comments/{commentId}")]
        public IActionResult Show(String subId, String postId, int commentId)
        {
            DatabaseComment? dbComment = dbComments.readComment(postId, commentId);
            if (dbComment != null)
            {
                DatabaseMapper mapper = new DatabaseMapper();
                CommentThreadModel cm = mapper.toThreadComment(dbComment, subId);

                return View(cm);
            }

            return View("Error");
        }

        [HttpPost("{subId}/{postId}/comments/edit/{commentId}")]
        public IActionResult Edit(String subId, String postId, int commentId, CommentUpdateModel c)
        {
            DatabaseComment? result = dbComments.updateComment(postId, commentId, c);  
            if (result != null)
                return RedirectToRoute("ShowPostComments", new { subId = subId, postId = postId});

            return BadRequest();
        }

        [HttpGet("{subId}/{postId}/comments/edit/{commentId}", Name = "EditComment")]
        public IActionResult EditForm(String subId, String postId, int commentId)
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
                cm.SubId = subId;

                return View("Edit", cm);
            }

            return View("Error");
        }


        [HttpGet("{subId}/{postId}/comments/reply/{commentId}", Name = "CreateComment")]

        public IActionResult CreateForm(String subId, String postId, int commentId)
        {
            CommentCreateModel cm = new CommentCreateModel();
            cm.PostId = postId;
            cm.Parent = commentId;
            cm.SubId = subId;

            return View("Create", cm);
        }

        [HttpGet("{subId}/{postId}/comments")]
        public IActionResult ListComments(String subId, String postId)
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
                    result.Add(mapper.toThreadComment(dbc, subId));
                }
            }

            return View("AllComments", result);
        }


        [HttpPost("{subId}/{postId}/comments/{commentId}")]
        public IActionResult Create(String subId, String postId, int commentId, CommentCreateModel c)
        {
            CommentMapper mapper = new CommentMapper();
            Comment comment = mapper.createToComment(postId, c);
            CommentNode n = new CommentNode(commentId);

            DatabaseComment? dbComment = dbComments.createComment(n, comment);

            if (dbComment != null)
                return RedirectToRoute("ShowPostComments", new { subId = subId, postId = postId });

            return BadRequest();
        }

        [HttpPost("{subId}/{postId}/comments/delete/{commentId}")]
        public IActionResult Delete(String subId, String postId, int commentId, CommentDeleteModel c)
        {
            if (dbComments.deleteComment(postId, commentId, c) == true)
                return RedirectToRoute("ShowPostComments", new { subId = subId, postId = postId });

            return BadRequest();
        }

        [HttpGet("{subId}/{postId}/comments/delete/{commentId}", Name = "DeleteComment")]
        public IActionResult DeleteForm(String subId, String postId, int commentId)
        {
            DatabaseComment? dbc = dbComments.readComment(postId, commentId);
            if (dbc != null)
            {
                CommentDeleteModel cm = new CommentDeleteModel();
                cm.PostId = dbc.PostId;
                cm.UserId = dbc.UserId;
                cm.Id = dbc.Id;
                cm.SubId = subId;

                return View("Delete", cm);
            }

            return View("Error");
        }

    }
}
