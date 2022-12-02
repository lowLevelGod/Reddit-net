using RedditNet.CommentFolder;
using RedditNet.Models.CommentModel;

namespace RedditNet.Models.DatabaseModel
{
    public class DatabaseMapper
    {
        public DatabaseComment toDBComment(CommentNode n, Comment c)
        {
            DatabaseComment comment = new DatabaseComment();
            comment.Id = 0;
            comment.PostId = c.PostId;
            comment.Depth = n.Depth;
            comment.Parent = n.Parent;
            comment.Votes = c.Votes;
            comment.Text = c.Text;
            comment.UserId = c.UserId;
            comment.Lineage = n.Lineage;

            return comment;
        }

        public CommentThreadModel toThreadComment(DatabaseComment c)
        {
            CommentMapper mapper = new CommentMapper();
            Comment comment = new Comment(c.PostId, c.Id, c.UserId, c.Text, c.Votes);

            return mapper.toThreadModel(comment, c.Depth);
        }
    }
}
