using RedditNet.Models.CommentModel;

namespace RedditNet.Models.PostModel
{
    public class PostThreadModel : PostModel
    {
        public String Id { get; set; }
        public String Text { get; set; }
        public String Title { get; set; }

        public int? Votes { get; set; }   

        public List<CommentThreadModel> Comments { get; set; }
    }
}
