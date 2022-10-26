﻿

namespace RedditNet
{
    public class DataLayer
    {
        public void createComment(CommentNode node, Comment c)
        {
            if (!DatabaseInterface.treeNodes.ContainsKey(c.PostId))
            {
                DatabaseInterface.treeNodes[c.PostId] = new Dictionary<int, CommentNode>();
                DatabaseInterface.comments[c.PostId] = new Dictionary<int, Comment>();
            }
            DatabaseInterface.treeNodes[c.PostId][c.Id] = node;
            DatabaseInterface.comments[c.PostId][c.Id] = c;
        }

        public void deleteComment(String postId, int id)
        {
            if (DatabaseInterface.treeNodes.ContainsKey(postId))
            {
                DatabaseInterface.treeNodes[postId].Remove(id);
                DatabaseInterface.comments[postId].Remove(id);
            }
        }

        public CommentNode readNode(String postId, int id)
        {
            if (DatabaseInterface.treeNodes.ContainsKey(postId) && DatabaseInterface.treeNodes[postId].ContainsKey(id))
            {
                return DatabaseInterface.treeNodes[postId][id];
            }

            return null;
        }

        public Comment readComment(String postId, int id)
        {
            if (DatabaseInterface.comments.ContainsKey(postId) && DatabaseInterface.comments[postId].ContainsKey(id))
            {
                return DatabaseInterface.comments[postId][id];
            }

            return null;
        }

        public List<CommentNode> getDescendants(String postId, int parentId)
        {
            List<CommentNode> nodes = new List<CommentNode>();

            Dictionary<int, CommentNode> tree = DatabaseInterface.treeNodes[postId];
            String prefix = tree[parentId].Lineage;

            foreach (CommentNode node in tree.Values)
            {
                if (node.Parent != null && node.Lineage.StartsWith(prefix))
                    nodes.Add(node);
            }

            return nodes;
        }
    }
}
