using RedditNet.Models.CommentModel;
using RedditNet.Models.PostModel;

namespace RedditNet.PostFolder
{
    public class PostMapper
    {
        public PostThreadModel toThreadModel(List<CommentThreadModel> comments, Post post)
        {
            PostThreadModel result = new PostThreadModel();
            result.Comments = comments;
            result.Text = post.Text;
            result.Title = post.Title;
            result.UserId = post.UserId;
            result.RootId = post.Root.Id;
            result.Votes = post.Votes;
            result.Id = post.Id;

            return result;
        }

        public Post createModelToPost(PostCreateModel p)
        {
            Post post = new Post(p);

            return post;
        }
    }
}
