namespace RedditNet
{
    public class Submission
    {
        private String userId;
        private String text;
        private int votes;

        public Submission(string userId, string text, int votes = 0)
        {
            UserId = userId;
            Text = text;
            Votes = votes;
        }

        public string UserId { get => userId; set => userId = value; }
        public string Text { get => text; set => text = value; }
        public int Votes { get => votes; set => votes = value; }
    }
}
