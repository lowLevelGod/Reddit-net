using RedditNet.CommentFolder;
using RedditNet.PostFolder;
using System.Runtime.Serialization;

namespace RedditNet.DataLayerFolder
{
    public class DatabaseInterface
    {
        public static Dictionary<string, Dictionary<int, CommentNode>> treeNodes = new Dictionary<string, Dictionary<int, CommentNode>>();
        public static Dictionary<string, Dictionary<int, Comment>> comments = new Dictionary<string, Dictionary<int, Comment>>();
        public static Dictionary<string, Post> posts = new Dictionary<string, Post>();
        public static DataLayerComments dataLayerComments = new DataLayerComments();
        public static DataLayerPosts dataLayerPosts = new DataLayerPosts();
        public static int idGenerator = 0;
    }
}
