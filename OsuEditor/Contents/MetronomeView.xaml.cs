using OsuEditor.ViewModels;

namespace OsuEditor.Contents
{
    public partial class TimingHeaderView
    {
        public TimingHeaderView()
        {
            InitializeComponent();
            DataContext = new MetronomeViewModel();
        }
    }
}
