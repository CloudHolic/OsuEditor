using System;
using System.Windows;
using OsuEditor.Events;
using OsuEditor.ViewModels;

namespace OsuEditor
{
    public partial class MainWindow : IEvent<BeatSnapEvent>
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWIndowViewModel();

            EventBus.Instance.RegisterHandler(this);
        }

        private void IncreaseZoom_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWIndowViewModel)DataContext).Zoom = HeaderTimeline.Zoom = Math.Min(10, HeaderTimeline.Zoom + 0.2);
        }

        private void DecreaseZoom_OnClick(object sender, RoutedEventArgs e)
        {
            ((MainWIndowViewModel)DataContext).Zoom = HeaderTimeline.Zoom = Math.Max(0, HeaderTimeline.Zoom - 0.2);
        }

        public void HandleEvent(BeatSnapEvent e)
        {
            HeaderTimeline.SubInterval = ((MainWIndowViewModel) DataContext).Snap = e.Snap;
        }
    }
}
