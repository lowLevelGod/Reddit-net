using Microsoft.AspNetCore.Mvc;
using RedditNet.DataLayerFolder;
using RedditNet.Models.UserModel;
using RedditNet.UserFolder;

namespace RedditNet.Controllers
{
    public class UsersController : Controller
    {
        private static DataLayerPosts dp = DatabaseInterface.dataLayerPosts;
        private static DataLayerUsers du = DatabaseInterface.dataLayerUsers;
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("users/{userId}")]
        public IActionResult Show(String userId)
        {
            User u = du.readUser(userId);
            if (u != null)
            {
                UserMapper mapper = new UserMapper();
                UserShowModel result = mapper.toShowModel(u);

                return View(result);
            }

            return View("Error");
        }

        [HttpPut("users/{userId}")]
        public IActionResult Edit(String userId, [FromBody] UserUpdateModel u)
        {
            du.updateUser(userId, u);
            return Ok();
        }

        [HttpPost("users")]
        public IActionResult Create([FromBody] UserCreateModel u)
        {
            UserMapper mapper = new UserMapper();
            User user = mapper.createModelToUser(u);

            du.createUser(user);
            return Ok();
        }

        [HttpDelete("users/{userId}")]
        public IActionResult Delete(String userId, [FromBody] UserDeleteModel u)
        {
            du.deleteUser(userId, u);
            return Ok();
        }
    }
}
