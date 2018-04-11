namespace OsuEditor.Models.Dialogs
{
    public class OpenSettings
    {
        public string MapsetPath { get; set; }

        public StartMethod Method { get; set; }

        public bool Remember { get; set; }

        public OpenSettings()
        {
            MapsetPath = string.Empty;
            Method = StartMethod.Create;
            Remember = false;
        }

        public OpenSettings(string path, StartMethod method, bool remember)
        {
            MapsetPath = path;
            Method = method;
            Remember = remember;
        }
    }
}
