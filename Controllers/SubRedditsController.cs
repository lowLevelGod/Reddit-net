using Microsoft.AspNetCore.Mvc;
using RedditNet.DataLayerFolder;
using RedditNet.Models.PostModel;
using RedditNet.Models.SubRedditModel;
using RedditNet.SubRedditFolder;

namespace RedditNet.Controllers
{
    public class SubRedditsController : Controller
    {
        private static DataLayerSubReddits ds = DatabaseInterface.dataLayerSubReddits;

        //TODO
        //MOVE POST TO OTHER SUBREDDIT

        [HttpGet("subs/{subId}/posts")]
        public IActionResult Show(String subId)
        {
            SubReddit s = ds.readSubReddit(subId);
            if (s != null)
            {
                List<PostPreviewModel> pm = ds.getPosts(subId);
                SubRedditMapper mapper = new SubRedditMapper();
                SubRedditPostsModel result = mapper.toPostsModel(pm, s);

                return View(result);
            }

            return View("Error");
        }

        [HttpPut("subs/{subId}")]
        public IActionResult Edit(String subId, [FromBody] SubRedditUpdateModel m)
        {
            ds.editSubReddit(subId, m);
            return Ok();
        }

        [HttpPost("subs")]
        public IActionResult Create(String subId, [FromBody] SubRedditCreateModel m)
        {
            SubRedditMapper mapper = new SubRedditMapper();
            SubReddit s = mapper.createModelToSubReddit(m);

            ds.createSubReddit(s, m.UserId);
            Console.WriteLine(s.Id);
            return Ok();
        }

        [HttpDelete("subs/{subId}")]
        public IActionResult Delete(String subId, [FromBody] SubRedditDeleteModel m)
        {
            ds.removeSubReddit(subId, m.UserId);
            return Ok();
        }
    }
}
