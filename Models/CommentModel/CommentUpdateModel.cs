namespace RedditNet.Models.CommentModel
{
    public class CommentUpdateModel : CommentModel
    {
        public String Text { get; set; }
        public int? Votes { get; set; }
    }
}
