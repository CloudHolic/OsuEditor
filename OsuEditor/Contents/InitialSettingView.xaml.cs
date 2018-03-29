using System.Windows;
using OsuEditor.ViewModels;

namespace OsuEditor.Contents
{
    public partial class InitialSettingView
    {
        public InitialSettingView()
        {
            InitializeComponent();
            DataContext = new InitialSettingViewModel(Application.Current.MainWindow, this);
        }
    }
}
