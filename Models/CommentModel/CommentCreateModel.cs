namespace RedditNet.Models.CommentModel
{
    public class CommentCreateModel : CommentModel
    {
        public String PostId { get; set; }
        public String Text { get; set; }
    }
}
