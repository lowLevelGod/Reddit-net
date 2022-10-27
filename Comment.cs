using RedditNet.Models.CommentModel;
using System.Data;

namespace RedditNet
{
    public class Comment : Submission
    {
        private String postId;
        private int id;

        public Comment(String postId, int id) : base()
        {
            PostId = postId;
            Id = id;
        }

        public Comment(String postId, int id, string userId, string text, int votes = 0) : base(userId, text, votes)
        {
            PostId = postId;
            Id = id;
        }

        public bool isSame(CommentUpdateModel c)
        {
            return UserId == c.UserId;
        }

        public void update(CommentUpdateModel c) 
        {
            Text = c.Text == null ? Text : c.Text;
            Votes = c.Votes == null ? Votes : c.Votes;
        }

        public string PostId { get => postId; set => postId = value; }
        public int Id { get => id; set => id = value; }
    }
}
