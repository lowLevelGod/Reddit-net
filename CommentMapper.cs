using RedditNet.Models.CommentModel;

namespace RedditNet
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

            return result;
        }
    }
}
