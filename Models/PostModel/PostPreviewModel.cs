namespace RedditNet.Models.PostModel
{
    public class PostPreviewModel : PostModel
    {
        public String Id { get; set; }
        public String Title { get; set; }
        public int Votes { get; set; }
        public String UserName { get; set; }
    }
}
