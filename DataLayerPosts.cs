using RedditNet.Models.CommentModel;
using RedditNet.Models.PostModel;

namespace RedditNet
{
    public class DataLayerPosts
    {
        public void createPost(Post post)
        {
            DatabaseInterface.posts[post.Id] = post;
            DataLayerComments d = new DataLayerComments();

            d.createComment(post.Root, new Comment(post.Id, post.Root.Id));
        }

        public void deletePost(String id)
        {
            if (DatabaseInterface.posts.ContainsKey(id))
            {
                DatabaseInterface.posts.Remove(id);
            }
        }

        public void updatePost(String id, PostUpdateModel p)
        {
            //TO DO
            //Implement updatedPost.update()
            //Put properties in PostUpdateModel
            Post updatedPost = readPost(id);
            if (updatedPost != null && updatedPost.isSame(p))
            {
                updatedPost.update(p);
            }
        }

        public Post readPost(String id)
        {
            if (DatabaseInterface.posts.ContainsKey(id))
            {
                return DatabaseInterface.posts[id];
            }

            return null;
        }
    }
}
