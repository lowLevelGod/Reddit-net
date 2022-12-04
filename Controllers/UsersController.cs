using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RedditNet.DataLayerFolder;
using RedditNet.Models.DatabaseModel;
using RedditNet.Models.UserModel;
using RedditNet.UserFolder;

namespace RedditNet.Controllers
{
    public class UsersController : Controller
    {

        private DataLayerComments dbComments;
        private DataLayerUsers dbUsers;
        private readonly UserManager<DatabaseUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersController(
            AppDbContext context,
            UserManager<DatabaseUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            dbComments = new DataLayerComments(context);
            dbUsers = new DataLayerUsers(userManager, roleManager);
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("users/{userId}", Name = "ShowUser")]
        public IActionResult Show(String userId)
        {
            DatabaseUser? u = dbUsers.readUser(userId);
            if (u != null)
            {
                UserMapper mapper = new UserMapper();
                UserShowModel result = mapper.toShowModel(
                    u,
                    _userManager.GetRolesAsync(u).Result.ElementAt(0)
                    );

                return View("Show", result);
            }

            return View("Error");
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("users/{userId}/changeRole/{newRole}", Name = "ChangeRoleUser")]
        public IActionResult ChangeRole(String userId, String newRole)
        {
            //if (User.IsInRole("Admin"))
            //{
                DatabaseUser? user = dbUsers.readUser(userId);
                String[] roles = new string[] { "Admin", "Regular", "Moderator" };
                if (user != null)
                {
                List<String> currentRoles = (List<string>)_userManager.GetRolesAsync(user).Result;
                _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (!roles.Contains<String>(newRole))
                    newRole = "Regular";

                _userManager.AddToRoleAsync(user, newRole);
                _userManager.UpdateSecurityStampAsync(user);



                }   

                return RedirectToRoute("ShowUser", new { userId = userId });
            //}

            return View("Error");
        }

        //[HttpPut("users/{userId}")]
        //public IActionResult Edit(String userId, [FromBody] UserUpdateModel u)
        //{
        //    du.updateUser(userId, u);
        //    return Ok();
        //}

        //[HttpPost("users")]
        //public IActionResult Create([FromBody] UserCreateModel u)
        //{
        //    UserMapper mapper = new UserMapper();
        //    User user = mapper.createModelToUser(u);

        //    du.createUser(user);
        //    return Ok();
        //}

        //[HttpDelete("users/{userId}")]
        //public IActionResult Delete(String userId, [FromBody] UserDeleteModel u)
        //{
        //    du.deleteUser(userId, u);
        //    return Ok();
        //}
    }
}
