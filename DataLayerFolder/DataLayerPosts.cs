using RedditNet.CommentFolder;
using RedditNet.Models.CommentModel;
using RedditNet.Models.PostModel;
using RedditNet.PostFolder;

namespace RedditNet.DataLayerFolder
{
    public class DataLayerPosts
    {
        public void createPost(Post post)
        {
            DatabaseInterface.posts[post.Id] = post;

            DatabaseInterface.treeNodes[post.Id] = new Dictionary<int, CommentNode>();
            DatabaseInterface.comments[post.Id] = new Dictionary<int, Comment>();

            DataLayerComments d = DatabaseInterface.dataLayerComments;

            d.createComment(post.Root, new Comment(post.Id, post.Root.Id));

        }

        public void deletePost(string id, PostDeleteModel p)
        {
            Post deletedPost = readPost(id);
            if (deletedPost != null && deletedPost.isSame(p))
            {
                deletedPost.setDeletedState();
            }
            //if (DatabaseInterface.posts.ContainsKey(id))
            //{
            //    DatabaseInterface.posts.Remove(id);
            //}
        }

        public void updatePost(string id, PostUpdateModel p)
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

        public Post readPost(string id)
        {
            if (DatabaseInterface.posts.ContainsKey(id))
            {
                return DatabaseInterface.posts[id];
            }

            return null;
        }
    }
}
