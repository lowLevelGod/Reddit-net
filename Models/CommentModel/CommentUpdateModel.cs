namespace RedditNet.Models.CommentModel
{
    public class CommentUpdateModel
    {
        public String Text { get; set; }
        public int? Votes { get; set; }

        public String UserId { get; set; }
    }
}
