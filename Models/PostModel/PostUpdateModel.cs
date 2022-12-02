namespace RedditNet.Models.PostModel
{
    public class PostUpdateModel : PostModel
    {
        public String Text { get; set; }

        public int Votes { get; set; }
    }
}
