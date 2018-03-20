using System;
using System.Windows;
using OsuEditor.Contents;
using OsuEditor.Events;

namespace OsuEditor
{
    public partial class MainWindow : IEvent<BeatSnapEvent>
    {
        public MainWindow()
        {
            InitializeComponent();
            ComposeButton.IsChecked = true;
            TimingButton.IsChecked = false;

            EventBus.Instance.RegisterHandler(this);
        }

        private void ComposeButton_OnChecked(object sender, RoutedEventArgs e)
        {
            TimingButton.IsChecked = false;
            HeaderContent.Content = new ComposeHeaderView();
            BodyContent.Content = new ComposeBodyView();
        }

        private void TimingButton_OnChecked(object sender, RoutedEventArgs e)
        {
            ComposeButton.IsChecked = false;
            HeaderContent.Content = new TimingHeaderView();
            BodyContent.Content = new ComposeBodyView();
        }

        public void HandleEvent(BeatSnapEvent e)
        {
            HeaderTimeline.SubInterval = e.Snap;
        }

        private void IncreaseZoom_OnClick(object sender, RoutedEventArgs e)
        {
            HeaderTimeline.Zoom = Math.Min(10, HeaderTimeline.Zoom + 0.2);
        }

        private void DecreaseZoom_OnClick(object sender, RoutedEventArgs e)
        {
            HeaderTimeline.Zoom = Math.Max(0, HeaderTimeline.Zoom - 0.2);
        }
    }
}
