using Microsoft.AspNetCore.Mvc;
using RedditNet.Models.CommentModel;
using System.Collections.Generic;
using System.Net;

namespace RedditNet.Controllers
{
    public class CommentsController : Controller
    {
        private static DataLayerComments d = new DataLayerComments();
        private static int init = 0;
        private static int idGenerator = 0;
        [NonAction]
        private List<CommentThreadModel> getComments()
        {
            String postId = "postid";
            if (init == 0)
            {
                init = 1;

                CommentNode root = new CommentNode();
                Comment rootComment = new Comment(postId, 0, "user 0", "DO NOT READ THIS !");

                Dictionary<String, Dictionary<int, CommentNode>> tree = DatabaseInterface.treeNodes;
                Dictionary<String, Dictionary<int, Comment>> comments = DatabaseInterface.comments;

                List<CommentNode> n = new List<CommentNode>();
                List<Comment> nc = new List<Comment>();

                n.Add(root);
                nc.Add(rootComment);

                for (int i = 1; i <= 7; ++i)
                {
                    idGenerator++;
                    CommentNode node = new CommentNode(idGenerator);
                    Comment comment = new Comment(postId, idGenerator, "user " + idGenerator.ToString(), "comment " + idGenerator.ToString());
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
            }

            List<CommentNode> desc = d.getDescendants(postId, 0);

            List<CommentThreadModel> result = new List<CommentThreadModel>();
            CommentMapper mapper = new CommentMapper();

            foreach (var x in desc)
            {
                CommentThreadModel t = mapper.toThreadModel(d.readComment(postId, x.Id), x.Depth);
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

        [HttpGet("{postId}/comments/{commentId}")]
        public IActionResult Show(String postId, int commentId)
        {

            Comment c = d.readComment(postId, commentId);
            Console.WriteLine(c.Votes);
            if (c != null)
            {
                CommentMapper mapper = new CommentMapper();
                CommentThreadModel m = mapper.toThreadModel(c, d.readNode(postId, commentId).Depth);

                return View(m);
            }
            
            return View("Error");
        }

        [HttpPut("{postId}/comments/{commentId}")]
        public IActionResult Edit(String postId, int commentId, [FromBody] CommentUpdateModel c)
        {
            // TODO 
            // CHECK IF USER ALREADY VOTED
            // POSSIBLE RACE CONDITION
            d.updateComment(postId, commentId, c);

            return Ok();
        }

        [HttpPost("{postId}/comments/{commentId}")]
        public IActionResult Create(String postId, int commentId, [FromBody] CommentCreateModel c)
        {
            //Some Error checking maybe?
            CommentNode parentNode = d.readNode(postId, commentId);
            if (parentNode != null)
            {
                idGenerator++;
                CommentMapper mapper = new CommentMapper();
                Comment newComment = mapper.createToComment(postId, idGenerator, c);
                CommentNode newNode = new CommentNode(idGenerator, parentNode);

                d.createComment(newNode, newComment);

            }
            return Ok();
        }
    }
}
