namespace RedditNet
{
    public class DatabaseInterface
    {
        public static Dictionary<String, Dictionary<int, CommentNode>> treeNodes = new Dictionary<string, Dictionary<int, CommentNode>>();
        public static Dictionary<String, Dictionary<int, Comment>> comments = new Dictionary<String, Dictionary<int, Comment>>();
    }
}
