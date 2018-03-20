using System;
using System.Windows;
using OsuEditor.Contents;
using OsuEditor.ViewModels;

namespace OsuEditor
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWIndowViewModel();
        }

        private void IncreaseZoom_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWIndowViewModel)DataContext).Zoom = HeaderTimeline.Zoom = Math.Min(10, HeaderTimeline.Zoom + 0.2);
        }

        private void DecreaseZoom_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWIndowViewModel)DataContext).Zoom = HeaderTimeline.Zoom = Math.Max(0, HeaderTimeline.Zoom - 0.2);
        }
    }
}
