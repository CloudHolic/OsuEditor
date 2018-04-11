using OsuEditor.Models.Dialogs;
using OsuEditor.ViewModels;

namespace OsuEditor.Contents
{
    public partial class EditorSettingView
    {
        public EditorSettingView(EditorSettings settings)
        {
            InitializeComponent();
            DataContext = new EditorSettingViewModel(settings);
        }

        //  1. Fullscreen on/off
        //  2. Resolution
        //  3. Base mapset directory
        //  4. Side clipboard Autosave
        //  5. Plugins' display on/off
        //  (side clipboard, sv tool, note density, waveform, etc.)
        //  6. Volume
        //  7. Shortcut
    }
}
