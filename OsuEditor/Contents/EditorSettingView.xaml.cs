using OsuEditor.Models;
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
    }
}
