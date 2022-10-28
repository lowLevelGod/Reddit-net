using Microsoft.AspNetCore.Mvc;
using RedditNet.CommentFolder;
using RedditNet.DataLayerFolder;
using RedditNet.Models.CommentModel;
using System.Collections.Generic;
using System.Net;

namespace RedditNet.Controllers
{
    public class CommentsController : Controller
    {
        private static DataLayerComments d = DatabaseInterface.dataLayerComments;

        [HttpGet("{postId}/comments/{commentId}")]
        public IActionResult Show(String postId, int commentId)
        {

            Comment c = d.readComment(postId, commentId);

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
                DatabaseInterface.idGenerator++;
                CommentMapper mapper = new CommentMapper();
                Comment newComment = mapper.createToComment(postId, DatabaseInterface.idGenerator, c);
                CommentNode newNode = new CommentNode(DatabaseInterface.idGenerator, parentNode);

                d.createComment(newNode, newComment);

            }
            return Ok();
        }

        [HttpDelete("{postId}/comments/{commentId}")]
        public IActionResult Delete(String postId, int commentId, [FromBody] CommentDeleteModel c)
        {
            d.deleteComment(postId, commentId, c);
            return Ok();
        }

    }
}
