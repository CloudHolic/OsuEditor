using System.Windows;
using System.Windows.Input;
using OsuEditor.Commands;
using OsuEditor.Contents;
using OsuEditor.Events;

namespace OsuEditor.ViewModels
{
    public class MainWIndowViewModel : ViewModelBase, IEvent<BeatSnapEvent>
    {
        public bool IsComposeTab
        {
            get { return Get(() => IsComposeTab); }
            set { Set(() => IsComposeTab, value); }
        }

        public int Snap
        {
            get { return Get(() => Snap); }
            set { Set(() => Snap, value); }
        }

        public double Zoom
        {
            get { return Get(() => Zoom); }
            set { Set(() => Zoom, value); }
        }

        public double SongLength
        {
            get { return Get(() => SongLength); }
            set { Set(() => SongLength, value); }
        }

        public object HeaderContent
        {
            get { return Get(() => HeaderContent); }
            set { Set(() => HeaderContent, value); }
        }

        public object BodyContent
        {
            get { return Get(() => BodyContent); }
            set { Set(() => BodyContent, value); }
        }

        public MainWIndowViewModel()
        {
            Zoom = 1.0;
            Snap = 8;
            SongLength = 10000;
            ComposeCommand.Execute(null);
            EventBus.Instance.RegisterHandler(this);
        }

        public ICommand ComposeCommand
        {
            get
            {
                return Get(() => ComposeCommand, new RelayCommand(() =>
                {
                    IsComposeTab = true;
                    HeaderContent = new ComposeHeaderView(Snap);
                    BodyContent = new ComposeBodyView();
                }));
            }
        }

        public ICommand TimingCommand
        {
            get
            {
                return Get(() => TimingCommand, new RelayCommand(() =>
                {
                    IsComposeTab = false;
                    HeaderContent = new TimingHeaderView();
                    BodyContent = new TimingBodyView();
                }));
            }
        }
        
        public void HandleEvent(BeatSnapEvent e)
        {
            Snap = e.Snap;
        }
    }
}
