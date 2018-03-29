using OsuEditor.ViewModels;

namespace OsuEditor.Contents
{
    public partial class EditorSettingView
    {
        public EditorSettingView()
        {
            InitializeComponent();
            DataContext = new EditorSettingViewModel();
        }
    }
}
