namespace OsuEditor.Models
{
    public class DiffVersion
    {
        public string DiffName { get; set; }

        public string FileName { get; set; }

        public bool Activated { get; set; }

        public DiffVersion()
        {
            DiffName = string.Empty;
            FileName = string.Empty;
            Activated = false;
        }

        public DiffVersion(string diff, string file, bool activated)
        {
            DiffName = diff;
            FileName = file;
            Activated = activated;
        }

        public DiffVersion(DiffVersion prevVersion)
        {
            DiffName = prevVersion.DiffName;
            FileName = prevVersion.FileName;
            Activated = prevVersion.Activated;
        }
    }
}
