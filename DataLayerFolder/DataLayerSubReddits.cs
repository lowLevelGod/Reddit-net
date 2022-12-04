
using RedditNet.Models.DatabaseModel;
using RedditNet.Models.PostModel;
using RedditNet.Models.SubRedditModel;
using RedditNet.PostFolder;
using RedditNet.SubRedditFolder;
using RedditNet.UserFolder;
using RedditNet.UtilityFolder;
using System;

namespace RedditNet.DataLayerFolder
{
    public class DataLayerSubReddits
    {
        private readonly AppDbContext db;
        public DataLayerSubReddits(AppDbContext db)
        {
            this.db = db;
        }
        //private bool hasPermission(String userId)
        //{
        //    User u = DatabaseInterface.dataLayerUsers.readUser(userId);
        //    if (u == null)
        //        return false;

        //    return u.isAdmin();
        //}
        public DatabaseSubReddit? createSubReddit(SubReddit s, String userId)
        {
            DatabaseMapper mapper = new DatabaseMapper();
            DatabaseSubReddit subReddit = mapper.toDBSubReddit(s);

            try
            {
                db.Add<DatabaseSubReddit>(subReddit);
                db.SaveChanges();

                return subReddit;
            }catch (Exception)
            {

            }

            return null;
        }

        public bool deleteSubReddit(String id, String userId)
        {
            DatabaseSubReddit? dbs = readSubReddit(id);
            if (dbs != null)
            {
                try
                {
                    List<DatabasePost>? posts = (from b in db.Posts
                                                 where (b.SubId == dbs.Id)
                                                 select b).ToList<DatabasePost>();
                    DataLayerPosts dp = new DataLayerPosts(db);
                    foreach (DatabasePost post in posts)
                    {
                        dp.deletePost(post.SubId, post.Id, new PostDeleteModel());
                    }

                }
                catch (Exception)
                {

                }
                try
                {
                    db.Remove<DatabaseSubReddit>(dbs);
                    db.SaveChanges();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }

            return false;
        }

        public DatabaseSubReddit? readSubReddit(String id)
        {
            try
            {
                DatabaseSubReddit? p = (from b in db.SubReddits
                                   where (b.Id == id)
                                   select b).FirstOrDefault<DatabaseSubReddit>();

                return p;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DatabaseSubReddit? updateSubReddit(String id, SubRedditUpdateModel m)
        {
            DatabaseSubReddit? dbp = readSubReddit(id);
            if (dbp != null)
            {
                try
                {
                    dbp.Description = m.Description;
                    db.SaveChanges();

                    return dbp;
                }
                catch (Exception)
                {
                    return null;
                }

            }

            return null;
        }

        public List<PostPreviewModel>? getPosts(String id, int start)
        {
            List<PostPreviewModel> result = new List<PostPreviewModel>();
            List<DatabasePost>? dbPosts = (from b in db.Posts
                                             where (b.SubId == id)
                                             select b).OrderBy(o => o.Id).Skip(start * Constants.pageSizePosts).Take(Constants.pageSizePosts).ToList<DatabasePost>();
            if (dbPosts == null)
                return null;

            PostMapper mapper = new PostMapper();
            DatabaseMapper dmapper = new DatabaseMapper();
            foreach (DatabasePost x in dbPosts)
            {
                //String userName = DatabaseInterface.dataLayerUsers.readUser(p.UserId).UserName;
                Post post = dmapper.toPost(x);
                PostPreviewModel pm = mapper.toPreviewModel("user name here", post);
                result.Add(pm);
            }

            return result;
        }

        public List<SubRedditPreviewModel>? getSubs()
        {
            List<SubRedditPreviewModel> result = new List<SubRedditPreviewModel>();
            List<DatabaseSubReddit>? dbSubs = (from b in db.SubReddits
                                               where (1 == 1)
                                           select b).ToList<DatabaseSubReddit>();
            if (dbSubs == null)
                return null;

            SubRedditMapper mapper = new SubRedditMapper();
            DatabaseMapper dmapper = new DatabaseMapper();
            foreach (DatabaseSubReddit x in dbSubs)
            {
                //String userName = DatabaseInterface.dataLayerUsers.readUser(p.UserId).UserName;
                SubReddit sub = dmapper.toSubReddit(x);
                SubRedditPreviewModel pm = mapper.toPreviewModel(sub);
                result.Add(pm);
            }

            return result;
        }
    }
}
