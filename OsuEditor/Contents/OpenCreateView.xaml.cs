using OsuEditor.ViewModels;

namespace OsuEditor.Contents
{
    public partial class OpenCreateView
    {
        public OpenCreateView()
        {
            InitializeComponent();
            DataContext = new OpenCreateViewModel();
        }
    }
}
