namespace OsuEditor.Models
{
    public class InitSettings
    {
        public string Mp3Path { get; set; }

        public PlayMode Mode { get; set; }

        public int Keys { get; set; }

        public bool SpecialStyle { get; set; }

        public InitSettings()
        {
            Mp3Path = string.Empty;
            Mode = PlayMode.Std;
            Keys = 0;
            SpecialStyle = false;
        }

        public InitSettings(string mp3, PlayMode mode, int keys, bool specialStyle)
        {
            Mp3Path = mp3;
            Mode = mode;
            Keys = keys;
            SpecialStyle = specialStyle;
        }
    }
}
