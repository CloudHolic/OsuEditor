namespace OsuEditor.Models
{
    public class Bookmark
    {
        public int Offset { get; set; }

        public string Memo { get; set; }

        public Bookmark()
        {
            Offset = 0;
            Memo = string.Empty;
        }

        public Bookmark(int offset, string memo)
        {
            Offset = offset;
            Memo = memo;
        }
    }
}
