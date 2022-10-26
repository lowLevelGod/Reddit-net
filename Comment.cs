namespace RedditNet
{
    public class Comment : Submission
    {
        private String postId;
        private int id;

        public Comment(String postId, int id, string userId, string text, int votes = 0) : base(userId, text, votes)
        {
            PostId = postId;
            Id = id;
        }

        public string PostId { get => postId; set => postId = value; }
        public int Id { get => id; set => id = value; }
    }
}
