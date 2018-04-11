namespace OsuEditor.Models.Dialogs
{
    public class EditorSettings
    {
        public string MapsetRoot { get; set; }

        public EditorSettings()
        {
            MapsetRoot = string.Empty;
        }

        public EditorSettings(string mapsetRoot)
        {
            MapsetRoot = mapsetRoot;
        }
    }
}
