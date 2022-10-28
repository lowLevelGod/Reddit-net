using RedditNet.CommentFolder;
using RedditNet.Models.CommentModel;

namespace RedditNet.DataLayerFolder
{
    public class DataLayerComments
    {
        public void createComment(CommentNode node, Comment c)
        {
            if (DatabaseInterface.treeNodes.ContainsKey(c.PostId))
            {
                DatabaseInterface.treeNodes[c.PostId][c.Id] = node;
                DatabaseInterface.comments[c.PostId][c.Id] = c;
            }
        }

        public void deleteComment(string postId, int id, CommentDeleteModel c)
        {
            Comment deletedComment = readComment(postId, id);
            if (deletedComment != null && deletedComment.isSame(c))
            {
                deletedComment.setDeletedState();
            }
            //if (DatabaseInterface.treeNodes.ContainsKey(postId))
            //{
            //    DatabaseInterface.treeNodes[postId].Remove(id);
            //    DatabaseInterface.comments[postId].Remove(id);
            //}
        }

        public void updateComment(string postId, int id, CommentUpdateModel c)
        {
            Comment updatedComment = readComment(postId, id);
            if (updatedComment != null && updatedComment.isSame(c))
            {
                updatedComment.update(c);
            }
        }

        public CommentNode readNode(string postId, int id)
        {
            if (DatabaseInterface.treeNodes.ContainsKey(postId) && DatabaseInterface.treeNodes[postId].ContainsKey(id))
            {
                return DatabaseInterface.treeNodes[postId][id];
            }

            return null;
        }

        public Comment readComment(string postId, int id)
        {
            if (DatabaseInterface.comments.ContainsKey(postId) && DatabaseInterface.comments[postId].ContainsKey(id))
            {
                return DatabaseInterface.comments[postId][id];
            }

            return null;
        }

        public List<CommentNode> getDescendants(string postId, int parentId)
        {
            Console.WriteLine(postId);
            List<CommentNode> nodes = new List<CommentNode>();

            Dictionary<int, CommentNode> tree = DatabaseInterface.treeNodes[postId];

            string prefix = tree[parentId].Lineage;

            foreach (CommentNode node in tree.Values)
            {
                if (node.Parent != null && node.Lineage.StartsWith(prefix))
                    nodes.Add(node);
            }

            return nodes;
        }
    }
}
