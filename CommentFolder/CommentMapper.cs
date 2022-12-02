using RedditNet.Models.CommentModel;

namespace RedditNet.CommentFolder
{
    public class CommentMapper
    {
        public CommentThreadModel toThreadModel(Comment c, int depth = 0)
        {
            CommentThreadModel result = new CommentThreadModel();
            result.Text = c.Text;
            result.UserId = c.UserId;
            result.Id = c.Id;
            result.Depth = depth;
            result.Votes = c.Votes;

            return result;
        }

        public Comment createToComment(string postId, CommentCreateModel c, int id = 0)
        {
            Comment result = new Comment(postId, id, c.UserId, c.Text);

            return result;
        }
    }
}
