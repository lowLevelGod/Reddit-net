using RedditNet.UtilityFolder;

namespace RedditNet.CommentFolder
{
    public class CommentNode : IComparable<CommentNode>
    {
        private int id;
        private int? parent;
        private int depth;
        private string lineage;

        public int CompareTo(CommentNode other)
        {
            String[] otherArr = other.joinLineage(Lineage, Id).Split(Constants.lineageSeparator);
            String[] thisArr = this.joinLineage(Lineage, Id).Split(Constants.lineageSeparator);

            if (otherArr.Length > thisArr.Length)
                return 1;
            if (otherArr.Length < thisArr.Length)
                return -1;

            for (int i = 1; i < otherArr.Length; ++i)
            {
                int o = Convert.ToInt32(otherArr[i]);
                int t = Convert.ToInt32(thisArr[i]);

                if (o > t)
                    return 1;
                if (o < t)
                    return -1;
            }

            return 0;
        }

        private string joinLineage(string l, int id)
        {
            if (l == Constants.lineageSeparator)
                return l + id.ToString();
            return l + Constants.lineageSeparator + id.ToString();
        }

        public void makeChildOf(CommentNode parent)
        {
            Parent = parent.Id;
            Depth = parent.Depth + 1;
            Lineage = joinLineage(parent.Lineage, parent.Id);
        }
        public CommentNode(int id = 0, int? parent = null, int depth = 0, string lineage = "/")
        {
            Id = id;
            Parent = parent;
            Depth = depth;
            Lineage = lineage;
        }

        public CommentNode(int id, CommentNode parent)
        {
            Id = id;
            makeChildOf(parent);
        }

        public int Id { get => id; set => id = value; }
        public int? Parent { get => parent; set => parent = value; }
        public int Depth { get => depth; set => depth = value; }
        public string Lineage { get => lineage; set => lineage = value; }
    }
}
