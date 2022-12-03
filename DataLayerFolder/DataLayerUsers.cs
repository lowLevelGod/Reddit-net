using RedditNet.Models.DatabaseModel;
using RedditNet.Models.UserModel;
using RedditNet.UserFolder;

namespace RedditNet.DataLayerFolder
{

    public class DataLayerUsers
    {
        private readonly AppDbContext db;
        public DataLayerUsers(AppDbContext db)
        {
            this.db = db;
        }
        //private bool hasPermission(User affectedUser, User requestingUser)
        //{
        //    return affectedUser.isSame(requestingUser) || requestingUser.isAdmin();
        //}
        //public void createUser(User u)
        //{
        //    if (!DatabaseInterface.users.ContainsKey(u.Id))
        //    {
        //        DatabaseInterface.users[u.Id] = u;
        //    }
        //}

        public DatabaseUser? readUser(String id)
        {
            try
            {
                DatabaseUser? p = (from b in db.Users
                                        where (b.Id == id)
                                        select b).FirstOrDefault<DatabaseUser>();

                return p;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public void updateUser(String id, UserUpdateModel u)
        //{
        //    User updatedUser = readUser(id);
        //    User otherUser = readUser(u.Id);

        //    if (updatedUser != null && otherUser != null)
        //    {
        //        if (hasPermission(updatedUser, otherUser))
        //            updatedUser.update(u);
        //        if (otherUser.isAdmin())
        //            updatedUser.updateRole(u.Role);

        //    }
        //}

        //public void deleteUser(String id, UserDeleteModel u)
        //{
        //    User deletedUser = readUser(id);
        //    User otherUser = readUser(u.Id);
        //    if (deletedUser != null && otherUser != null)
        //    {
        //        if (deletedUser.isSame(u) || otherUser.isAdmin())
        //        {
        //            DatabaseInterface.users.Remove(id);
        //        }
        //    }
        //}
    }
}
