using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RedditNet.DataLayerFolder;
using RedditNet.Models.DatabaseModel;
using RedditNet.Models.PostModel;
using RedditNet.Models.SubRedditModel;
using RedditNet.PostFolder;
using RedditNet.SubRedditFolder;

namespace RedditNet.Controllers
{
    public class SubRedditsController : Controller
    {
        private DataLayerPosts dbPosts;
        private DataLayerSubReddits dbSubs;
        private DataLayerUsers dbUsers;
        public SubRedditsController(
            AppDbContext context,
            UserManager<DatabaseUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            dbPosts = new DataLayerPosts(context);
            dbSubs = new DataLayerSubReddits(context);
            dbUsers = new DataLayerUsers(userManager, roleManager);
        }

        ////TODO
        ////MOVE POST TO OTHER SUBREDDIT

        [HttpGet("subs/{subId}/posts/{start}", Name = "ShowSubPosts")]
        public IActionResult Show(String subId, int start)
        {
            DatabaseSubReddit? s = dbSubs.readSubReddit(subId);
            if (s != null)
            {
                List<PostPreviewModel>? pm = dbSubs.getPosts(subId, start);
                if (pm != null)
                {
                    SubRedditMapper mapper = new SubRedditMapper();
                    DatabaseMapper dmapper = new DatabaseMapper();
                    SubReddit sub = dmapper.toSubReddit(s);

                    SubRedditPostsModel result = mapper.toPostsModel(pm, sub);

                    ViewBag.Page = start;

                    return View("Show", result);
                }
            }

            return View("Error");
        }

        [HttpPost("subs/edit/{subId}")]
        public IActionResult Edit(String subId, SubRedditUpdateModel m)
        {
            DatabaseSubReddit? dbSub = dbSubs.updateSubReddit(subId, m);
            if (dbSub != null)
                return RedirectToAction("Show", new { subId = dbSub.Id, start = 0 });

            return BadRequest();
        }

        [HttpGet("subs/edit/{subId}", Name = "EditSub")]
        public IActionResult EditForm(String subId)
        {
            DatabaseSubReddit? sub = dbSubs.readSubReddit(subId);
            if (sub != null)
            {
                SubRedditUpdateModel pm = new SubRedditUpdateModel();
                pm.Description = sub.Description;
                pm.Name = sub.Name;
                pm.Id = sub.Id;
                pm.UserId = "user id here";

                return View("Edit", pm);
            }
            return View("Error");
        }

        [HttpPost("subs/create")]
        public IActionResult Create(SubRedditCreateModel m)
        {
            SubRedditMapper mapper = new SubRedditMapper();
            SubReddit s = mapper.createModelToSubReddit(m);

            DatabaseSubReddit? res = dbSubs.createSubReddit(s, m.UserId);
            
            if (res != null)
            {
                return RedirectToAction("Show", new { subId = res.Id, start = 0 });
            }

            return BadRequest();
        }

        [HttpGet("subs/create")]
        public IActionResult CreateForm()
        {
            SubRedditCreateModel cm = new SubRedditCreateModel();
            cm.UserId = "";
            cm.Name = "";
            cm.Description = "";

            return View("Create", cm);
        }

        [HttpPost("subs/delete/{subId}")]
        public IActionResult Delete(String subId, SubRedditDeleteModel m)
        {
            if (dbSubs.deleteSubReddit(subId, m.UserId) == true)
                return Ok();
            return BadRequest();
        }

        [HttpGet("subs/delete/{subId}", Name = "DeleteSub")]
        public IActionResult DeleteForm(String subId)
        {
            DatabaseSubReddit? sub = dbSubs.readSubReddit(subId);
            if (sub != null)
            {
                SubRedditDeleteModel pm = new SubRedditDeleteModel();
                pm.Id = sub.Id;
                pm.Name = sub.Name;
                pm.UserId = "user id here";

                return View("Delete", pm);
            }

            return View("Error");
        }
    }
}
