using RedditNet.Models.PostModel;
using RedditNet.Models.SubRedditModel;
using RedditNet.PostFolder;
using RedditNet.SubRedditFolder;
using RedditNet.UserFolder;

namespace RedditNet.DataLayerFolder
{
    public class DataLayerSubReddits
    {
        //private bool hasPermission(String userId)
        //{
        //    User u = DatabaseInterface.dataLayerUsers.readUser(userId);
        //    if (u == null)
        //        return false;

        //    return u.isAdmin();
        //}
        //public void createSubReddit(SubReddit s, String userId)
        //{
        //    if (!DatabaseInterface.subs.ContainsKey(s.Id) && hasPermission(userId))
        //    {
        //        DatabaseInterface.subs[s.Id] = s;
        //        DatabaseInterface.posts[s.Id] = new Dictionary<string, Post>();
        //    }
        //}

        //public void removeSubReddit(String id, String userId)
        //{
        //    if (DatabaseInterface.subs.ContainsKey(id) && hasPermission(userId))
        //        DatabaseInterface.subs.Remove(id);
        //}

        //public SubReddit readSubReddit(String id)
        //{
        //    if (DatabaseInterface.subs.ContainsKey(id))
        //        return DatabaseInterface.subs[id];

        //    return null;
        //}

        //public void editSubReddit(String id, SubRedditUpdateModel m)
        //{
        //    if (hasPermission(m.UserId))
        //    {
        //        SubReddit s = readSubReddit(id);
        //        if (s != null)
        //        {
        //            s.update(m);
        //        }
        //    }
        //}

        //public List<PostPreviewModel> getPosts(String id)
        //{
        //    List<PostPreviewModel> result = new List<PostPreviewModel>();   
        //    if (DatabaseInterface.posts.ContainsKey(id))
        //    {
        //        PostMapper mapper = new PostMapper();
        //        foreach (var x in DatabaseInterface.posts[id].Keys)
        //        {
        //            Post p = DatabaseInterface.dataLayerPosts.readPost(id, x);
        //            String userName = DatabaseInterface.dataLayerUsers.readUser(p.UserId).UserName;

        //            PostPreviewModel pm = mapper.toPreviewModel(userName, p);
        //            result.Add(pm);
        //        }
        //    }

        //    return result;
        //}
    }
}
