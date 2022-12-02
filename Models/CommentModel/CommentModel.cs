namespace RedditNet.Models.CommentModel
{
    public class CommentModel
    {
        public String UserId { get; set; }
        public int Parent { get; set; }
        public String PostId { get; set; }
    }
}
