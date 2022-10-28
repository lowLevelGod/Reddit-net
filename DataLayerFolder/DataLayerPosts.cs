using RedditNet.CommentFolder;
using RedditNet.Models.CommentModel;
using RedditNet.Models.PostModel;
using RedditNet.PostFolder;
using RedditNet.UserFolder;

namespace RedditNet.DataLayerFolder
{
    public class DataLayerPosts
    {
        private bool hasPermission(User affectedUser, User requestingUser)
        {
            return affectedUser.isSame(requestingUser) || requestingUser.isAdmin() || requestingUser.isMod();
        }
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
            User affectedUser = DatabaseInterface.dataLayerUsers.readUser(deletedPost.UserId);
            User requestingUser = DatabaseInterface.dataLayerUsers.readUser(p.UserId);

            if (affectedUser != null && requestingUser != null)
            {
                if (deletedPost != null && hasPermission(affectedUser, requestingUser))
                {
                    deletedPost.setDeletedState();
                }
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
