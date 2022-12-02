using RedditNet.CommentFolder;
using RedditNet.Models.CommentModel;
using RedditNet.Models.DatabaseModel;
using RedditNet.Models.PostModel;
using RedditNet.PostFolder;
using RedditNet.SubRedditFolder;
using RedditNet.UserFolder;

namespace RedditNet.DataLayerFolder
{
    public class DataLayerPosts
    {
        private readonly AppDbContext db;
        public DataLayerPosts(AppDbContext db)
        {
            this.db = db;
        }
        //private bool hasPermission(User affectedUser, User requestingUser)
        //{
        //    return (affectedUser?.isSame(requestingUser) ?? false) || requestingUser.isAdmin() || requestingUser.isMod();
        //}
        public DatabasePost? createPost(Post post)
        {
            DataLayerComments d = new DataLayerComments(db);
            if (d.createComment(post.Root, new Comment(post.Id, post.Root.Id, post.UserId, "root comment")) != null)
            {
                DatabaseMapper mapper = new DatabaseMapper();
                DatabasePost p = mapper.toDBPost(post);

                try
                {
                    db.Add<DatabasePost>(p);
                    db.SaveChanges();

                    return p;
                }catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        //public void deletePost(string subId, string id, PostDeleteModel p)
        //{
        //    Post deletedPost = readPost(subId, id);
        //    User affectedUser = DatabaseInterface.dataLayerUsers.readUser(deletedPost.UserId);
        //    User requestingUser = DatabaseInterface.dataLayerUsers.readUser(p.UserId);

        //    if (requestingUser != null)
        //    {
        //        if (deletedPost != null && hasPermission(affectedUser, requestingUser))
        //        {
        //            deletedPost.setDeletedState();
        //        }
        //    }
        //    //if (DatabaseInterface.posts.ContainsKey(id))
        //    //{
        //    //    DatabaseInterface.posts.Remove(id);
        //    //}
        //}

        //public void updatePost(string subId, string id, PostUpdateModel p)
        //{
        //    //TO DO
        //    //Implement updatedPost.update()
        //    //Put properties in PostUpdateModel

        //    Post updatedPost = readPost(subId, id);
        //    User affectedUser = DatabaseInterface.dataLayerUsers.readUser(updatedPost.UserId);
        //    User requestingUser = DatabaseInterface.dataLayerUsers.readUser(p.UserId);

        //    if (requestingUser != null)
        //    {
        //        if (updatedPost != null)
        //        {
        //            if (hasPermission(affectedUser, requestingUser))
        //                updatedPost.update(p);
        //            if (p.SubId != null)
        //                changeSubReddit(updatedPost.SubId, p.SubId, updatedPost.Id, requestingUser.Id);
        //        }
        //    }
        //}

        public DatabasePost? readPost(string subId, string id)
        {
            try
            {
                DatabasePost? p = (from b in db.Posts
                                   where (b.Id == id) && (b.SubId == subId)
                                   select b).FirstOrDefault<DatabasePost>();

                return p;
            }catch (Exception)
            {
                return null;
            }

        }

        //public void removePost(string subId, string id)
        //{
        //    if (DatabaseInterface.posts.ContainsKey(subId) && DatabaseInterface.posts[subId].ContainsKey(id))
        //    {
        //        DatabaseInterface.posts[subId].Remove(id);
        //    }
        //}

        //public void changeSubReddit(String oldSubId, String newSubId, String id, String userId)
        //{
        //    User u = DatabaseInterface.dataLayerUsers.readUser(userId);
        //    if (u != null && 
        //        (u.isMod() || u.isAdmin())
        //        )
        //    {
        //        SubReddit oldSub = DatabaseInterface.dataLayerSubReddits.readSubReddit(oldSubId);
        //        SubReddit newSub = DatabaseInterface.dataLayerSubReddits.readSubReddit(newSubId);

        //        if (oldSub != null && newSub != null)
        //        {
        //            Post p = readPost(oldSub.Id, id);
        //            if (p != null)
        //            {
        //                p.SubId = newSub.Id;
        //                removePost(oldSub.Id, id);
        //                DatabaseInterface.posts[newSubId][id] = p;
        //            }
        //        }
        //    }
        //}
    }
}
