
using RedditNet.Models.UserModel;
using RedditNet.UtilityFolder;

namespace RedditNet.UserFolder
{
    public class UserMapper
    {
        private String roleToString(int? role)
        {
            switch (role)
            {
                case Constants.admin:
                    return "admin";
                case Constants.mod:
                    return "mod";
                case Constants.regular:
                    return "regular";
                default:
                    return "guest";
            }
        }
        //public UserShowModel toShowModel(User u)
        //{
        //    UserShowModel result = new UserShowModel();
        //    result.Id = u.Id;
        //    result.UserName = u.UserName;
        //    result.Role = roleToString(u.Role);
        //    result.Email = u.Email;
        //    result.Description = u.Description;

        //    return result;
        //}

        //public User createModelToUser(UserCreateModel u)
        //{
        //    User result = new User(u.UserName, u.Password, u.Email, u.Description, Constants.regular);

        //    return result;
        //}
    }
}
