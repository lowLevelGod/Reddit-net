using RedditNet.CommentFolder;
using RedditNet.Models.CommentModel;
using RedditNet.UserFolder;

namespace RedditNet.DataLayerFolder
{
    public class DataLayerComments
    {
        private bool hasPermission(User affectedUser, User requestingUser)
        {
            return affectedUser.isSame(requestingUser) || requestingUser.isAdmin() || requestingUser.isMod();
        }
        public void createComment(CommentNode node, Comment c)
        {
            if (DatabaseInterface.treeNodes.ContainsKey(c.PostId))
            {
                if (!DatabaseInterface.treeNodes[c.PostId].ContainsKey(c.Id))
                {
                    DatabaseInterface.treeNodes[c.PostId][c.Id] = node;
                    DatabaseInterface.comments[c.PostId][c.Id] = c;
                }
            }
        }

        public void deleteComment(string postId, int id, CommentDeleteModel c)
        {
            Comment deletedComment = readComment(postId, id);
            User affectedUser = DatabaseInterface.dataLayerUsers.readUser(deletedComment.UserId);
            User requestingUser = DatabaseInterface.dataLayerUsers.readUser(c.UserId);

            if (affectedUser != null && requestingUser != null)
            {
                if (deletedComment != null && hasPermission(affectedUser, requestingUser))
                {
                    deletedComment.setDeletedState();
                }
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
