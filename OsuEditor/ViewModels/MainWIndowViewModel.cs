using System;
using System.Windows.Input;
using System.Windows.Threading;
using OsuEditor.Commands;
using OsuEditor.Contents;
using OsuEditor.Events;

namespace OsuEditor.ViewModels
{
    public class MainWIndowViewModel : ViewModelBase
    {
        #region Properties
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

        public double CurrentPosition
        {
            get { return Get(() => CurrentPosition); }
            set { Set(() => CurrentPosition, value); }
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
        #endregion

        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public MainWIndowViewModel()
        {
            Zoom = 5.0;
            Snap = 4;
            SongLength = 100000;

            _timer.Interval = TimeSpan.FromMilliseconds((double)1000 / 144);
            _timer.Tick += (sender, args) =>
            {
                CurrentPosition += (double)1000 / 144;
                if (CurrentPosition > SongLength)
                {
                    CurrentPosition = SongLength;
                    _timer.Stop();
                }

                EventBus.Instance.Publish(new CurPositionEvent {CurPosition = CurrentPosition});
            };

            ComposeCommand.Execute(null);
        }

        #region Commands
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

        public ICommand PlayCommand
        {
            get
            {
                return Get(() => PlayCommand, new RelayCommand(() =>
                {
                    if (_timer.IsEnabled)
                    {
                        _timer.Stop();
                        CurrentPosition = 0;
                    }
                    _timer.Start();
                }));
            }
        }

        public ICommand PauseCommand
        {
            get
            {
                return Get(() => PauseCommand, new RelayCommand(() =>
                {
                    if(_timer.IsEnabled)
                        _timer.Stop();
                    else
                        _timer.Start();
                }));
            }
        }

        public ICommand StopCommand
        {
            get
            {
                return Get(() => StopCommand, new RelayCommand(() =>
                {
                    if (_timer.IsEnabled)
                        _timer.Stop();
                    CurrentPosition = 0;
                }));
            }
        }

        public ICommand TestCommand
        {
            get
            {
                return Get(() => TestCommand, new RelayCommand(() =>
                {
                    //TODO: Implement Test.
                }));
            }
        }
        #endregion
    }
}
