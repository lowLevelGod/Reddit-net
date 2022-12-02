namespace RedditNet.Models.CommentModel
{
    public class CommentCreateModel : CommentModel
    {
        public int Parent { get; set; }
        public String Text { get; set; }
    }
}
