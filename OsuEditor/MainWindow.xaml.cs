using System.Windows;
using MahApps.Metro.Controls;
using OsuEditor.Contents;

namespace OsuEditor
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            HeaderControl.Content = new ComposeHeaderView();
        }
    }
}
