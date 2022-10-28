namespace RedditNet.Models.CommentModel
{
    public class CommentThreadModel : CommentModel
    {
        public int Id { get; set; }
        public String Text { get; set; }
        public int Depth { get; set; }
        public int? Votes { get; set; }
    }
}
