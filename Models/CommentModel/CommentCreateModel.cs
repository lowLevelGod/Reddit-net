namespace RedditNet.Models.CommentModel
{
    public class CommentCreateModel
    {
        public String UserId { get; set; }
        public String PostId { get; set; }
        public String Text { get; set; }
    }
}
