using RedditNet.Models.UserModel;
using RedditNet.UserFolder;

namespace RedditNet.DataLayerFolder
{
    public class DataLayerUsers
    {
        private bool hasPermission(User affectedUser, User requestingUser)
        {
            return affectedUser.isSame(requestingUser) || requestingUser.isAdmin();
        }
        public void createUser(User u)
        {
            if (!DatabaseInterface.users.ContainsKey(u.Id))
            {
                DatabaseInterface.users[u.Id] = u;
            }
        }

        public User readUser(String id)
        {
            if (DatabaseInterface.users.ContainsKey(id))
                return DatabaseInterface.users[id];
            return null;
        }

        public void updateUser(String id, UserUpdateModel u)
        {
            User updatedUser = readUser(id);
            User otherUser = readUser(u.Id);

            if (updatedUser != null && otherUser != null)
            {
                if (hasPermission(updatedUser, otherUser))
                    updatedUser.update(u);
                if (otherUser.isAdmin())
                    updatedUser.updateRole(u.Role);

            }
        }

        public void deleteUser(String id, UserDeleteModel u)
        {
            User deletedUser = readUser(id);
            User otherUser = readUser(u.Id);
            if (deletedUser != null && otherUser != null)
            {
                if (deletedUser.isSame(u) || otherUser.isAdmin())
                {
                    DatabaseInterface.users.Remove(id);
                }
            }
        }
    }
}
