using RedditNet.Models.CommentModel;
using RedditNet.Models.PostModel;

namespace RedditNet
{
    public class Post : Submission
    {
        private List<CommentThreadModel> comments;
        private CommentNode root;
        private String id;
        private String title;

        public Post() : base()
        {

        }

        public bool isSame(PostUpdateModel p)
        {
            return UserId == p.UserId;
        }

        public void update(PostUpdateModel p)
        {

        }

        public Post(List<CommentThreadModel> l, String title, string userId, string text, String id = null, int? votes = 0) : base(userId, text, votes)
        {
            Comments = l;
            root = new CommentNode();
            if (id == null)
            {
                Hash hash = new Hash();

                TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));

                String toHash = "";
                toHash += Convert.ToString((int)t.TotalSeconds);
                toHash += title;
                toHash += text;
                toHash += userId;

                Id = hash.sha256_hash(toHash);
            }
        }

        public List<CommentThreadModel> Comments { get => comments; set => comments = value; }
        public CommentNode Root { get => root; set => root = value; }
        public string Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
    }
}
