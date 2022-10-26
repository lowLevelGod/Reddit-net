using Microsoft.AspNetCore.Mvc;
using RedditNet.Models.CommentModel;
using System.Collections.Generic;

namespace RedditNet.Controllers
{
    public class CommentsController : Controller
    {
        [NonAction]
        private List<CommentThreadModel> getComments()
        {
            String postId = "postid";

            CommentNode root = new CommentNode();
            Comment rootComment = new Comment(postId, 0, "user 0", "DO NOT READ THIS !");

            DataLayer d = new DataLayer();

            Dictionary<String, Dictionary<int, CommentNode>> tree = DatabaseInterface.treeNodes;
            Dictionary<String, Dictionary<int, Comment>> comments = DatabaseInterface.comments;

            List<CommentNode> n = new List<CommentNode>();
            List<Comment> nc = new List<Comment>();

            n.Add(root);
            nc.Add(rootComment);

            for (int i = 1; i <= 7; ++i)
            {
                CommentNode node = new CommentNode(i);
                Comment comment = new Comment(postId, i, "user " + i.ToString(), "comment " + i.ToString());
                n.Add(node);
                nc.Add(comment);
            }


            n[1].makeChildOf(n[0]);
            n[2].makeChildOf(n[1]);
            n[3].makeChildOf(n[2]);
            n[4].makeChildOf(n[2]);
            n[5].makeChildOf(n[1]);
            n[6].makeChildOf(n[1]);
            n[7].makeChildOf(n[6]);

            for (int i = 0; i <= 7; ++i)
                d.createComment(n[i], nc[i]);

            List<CommentNode> desc = d.getDescendants(postId, root.Id);

            List<CommentThreadModel> result = new List<CommentThreadModel>();
            CommentMapper mapper = new CommentMapper();

            foreach (var x in desc)
            {
                CommentThreadModel t = mapper.toThreadModel(comments[postId][x.Id], x.Depth);
                result.Add(t);
            }

            return result;
        }
        [HttpGet("{postId}/comments")]
        public IActionResult Index(String postId)
        {
            List<CommentThreadModel> comments = getComments();
            return View(comments);
        }

        [HttpGet("{postId}/comments/{commentId?}")]
        public IActionResult Show(int? commentId = null)
        {

            return View();
        }
    }
}
