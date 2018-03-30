using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;
using OsuEditor.Commands;
using OsuEditor.Contents;
using OsuEditor.Events;
using OsuEditor.Models;
using OsuEditor.Util;
using OsuParser;
using OsuParser.Structures;

namespace OsuEditor.ViewModels
{
    public class MainWIndowViewModel : ViewModelBase
    {
        #region Properties
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

        public int PlayRate
        {
            get { return Get(() => PlayRate); }
            set { Set(() => PlayRate, value); }
        }

        public Beatmap CurrentMap
        {
            get { return Get(() => CurrentMap); }
            set { Set(() => CurrentMap, value); }
        }
        #endregion

        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private double _oldTime;

        private readonly ICustomDialogManager _dialogManager;

        public MainWIndowViewModel()
        {
            CurrentMap = Parser.CreateBeatmap();
            PlayRate = 100;
            SongLength = 100000;
            CurrentMap.Edit.BeatDivisor = 4;
            CurrentMap.Edit.TimelineZoom = 5.0;

            _timer.Interval = TimeSpan.FromMilliseconds((double) 1000 / 144);
            _timer.Tick += (sender, args) =>
            {
                var curTime = _stopWatch.Elapsed.TotalMilliseconds;
                CurrentPosition += (curTime - _oldTime) * PlayRate / 100;
                _oldTime = curTime;

                if (CurrentPosition > SongLength)
                {
                    CurrentPosition = SongLength;
                    _timer.Stop();
                }

                EventBus.Instance.Publish(new CurPositionEvent {CurPosition = CurrentPosition});
            };

            EventBus.Instance.RegisterHandler(this);

            _dialogManager = new CustomDialogManager(new MetroDialogSettings
            {
                AnimateShow = false,
                AnimateHide = false
            });
        }

        #region Commands
        public ICommand InitialCommand
        {
            get
            {
                return Get(() => InitialCommand, new RelayCommand(async () =>
                {
                    var initSettingView = new InitialSettingView(new InitSettings
                    {
                        Mp3Path = string.Empty,
                        Mode = (PlayMode) CurrentMap.Gen.Mode,
                        Keys = CurrentMap.Gen.Mode == 3 ? (int) CurrentMap.Diff.CircleSize : 4,
                        SpecialStyle = CurrentMap.Gen.SpecialStyle
                    });

                    var result = await _dialogManager.ShowDialogAsync<InitSettings>(initSettingView);
                    if (result != null)
                    {
                        //TODO: Copy MP3 file to the basic mapset directory.

                        CurrentMap.Gen.Mode = (int) result.Mode;
                        if (CurrentMap.Gen.Mode == 3)
                        {
                            CurrentMap.Diff.CircleSize = result.Keys;
                            CurrentMap.Gen.SpecialStyle = result.SpecialStyle;
                        }

                        RaisePropertyChanged(nameof(CurrentMap));
                    }
                }));
            }
        }

        public ICommand EditorCommand
        {
            get
            {
                return Get(() => EditorCommand, new RelayCommand(async () =>
                {
                    var editorSettingView = new EditorSettingView(new EditorSettings());
                    await _dialogManager.ShowDialogAsync<EditorSettings>(editorSettingView);
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
                        _stopWatch.Reset();
                        CurrentPosition = 0;
                        EventBus.Instance.Publish(new CurPositionEvent { CurPosition = CurrentPosition });
                    }

                    _oldTime = 0;
                    _timer.Start();
                    _stopWatch.Start();
                }));
            }
        }

        public ICommand PauseCommand
        {
            get
            {
                return Get(() => PauseCommand, new RelayCommand(() =>
                {
                    if (_timer.IsEnabled)
                    {
                        _timer.Stop();
                        _stopWatch.Stop();
                    }
                    else
                    {
                        _oldTime = 0;
                        _timer.Start();
                        _stopWatch.Start();
                    }
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
                    {
                        _timer.Stop();
                        _stopWatch.Reset();
                    }
                    CurrentPosition = 0;
                    EventBus.Instance.Publish(new CurPositionEvent { CurPosition = CurrentPosition });
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
