using RedditNet.Models.UserModel;
using RedditNet.UtilityFolder;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace RedditNet.UserFolder
{
    public class User
    {
        private String id;
        private String userName;
        private String password;
        private int? role;

        public bool isAdmin()
        {
            return role == Constants.admin;
        }

        public bool isMod()
        {
            return role == Constants.mod;
        }

        public bool isRegular()
        {
            return role == Constants.regular;
        }

        public bool isGuest()
        {
            return role == Constants.guest;
        }

        public bool isSame(UserModel u)
        {
            return Id == u.Id;
        }

        public bool isSame(User u)
        {
            return Id == u.Id;
        }

        public void update(UserUpdateModel u)
        {
            Password = u.Password == null ? Password : u.Password;
        }

        public void updateRole(int? requestingRole)
        {
            if (requestingRole >= Constants.regular && requestingRole <= Constants.admin)
                Role = requestingRole;
            else
                Role = Constants.regular;
        }

        public User(string userName, string password, int role = Constants.guest, string id = null)
        {
            UserName = userName;
            Password = password;
            Role = role;

            if (id == null)
            {
                Hash hash = new Hash();

                Id = hash.sha256_hash(UserName);
            }
            else
                Id = id;
        }

        public string Id { get => id; set => id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }
        public int? Role { get => role; set => role = value; }
    }
}
