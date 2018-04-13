using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using MahApps.Metro.Controls.Dialogs;
using NAudio.Wave;
using OsuEditor.Commands;
using OsuEditor.Contents;
using OsuEditor.Events;
using OsuEditor.Models;
using OsuEditor.Models.Dialogs;
using OsuEditor.Models.Timings;
using OsuEditor.Util;
using OsuParser;
using OsuParser.Structures;

namespace OsuEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IEvent<ChangeCurrentMapEvent>
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

        public ObservableCollection<TimingMark> TimingMarks
        {
            get { return Get(() => TimingMarks); }
            set { Set(() => TimingMarks, value); }
        }

        public TimingMark CurrentTiming
        {
            get { return Get(() => CurrentTiming); }
            set { Set(() => CurrentTiming, value); }
        }

        public bool OffsetErrorOccurred
        {
            get { return Get(() => OffsetErrorOccurred); }
            set { Set(() => OffsetErrorOccurred, value); }
        }

        public ObservableCollection<DiffVersion> Diffs
        {
            get { return Get(() => Diffs); }
            set { Set(() => Diffs, value); }
        }

        public DiffVersion CurrentDiff
        {
            get { return Get(() => CurrentDiff); }
            set { Set(() => CurrentDiff, value); }
        }

        public string CurrentBookmarkNote
        {
            get { return Get(() => CurrentBookmarkNote); }
            set { Set(() => CurrentBookmarkNote, value); }
        }
        #endregion

        private readonly DispatcherTimer _playTimer = new DispatcherTimer();
        public DispatcherTimer ErrorTimer { get; } = new DispatcherTimer();
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private double _oldTime;

        private readonly ICustomDialogManager _dialogManager;

        private string _mapsetPath;

        public MainWindowViewModel()
        {
            CurrentMap = Parser.CreateBeatmap();
            PlayRate = 100;

            TimingMarks = new ObservableCollection<TimingMark> {new TimingMark()};
            CurrentTiming = TimingMarks[0];

            Diffs = new ObservableCollection<DiffVersion>();
            
            OffsetErrorOccurred = false;

            _playTimer.Interval = TimeSpan.FromMilliseconds((double) 1000 / 144);
            _playTimer.Tick += (sender, args) =>
            {
                var curTime = _stopWatch.Elapsed.TotalMilliseconds;
                CurrentPosition += (curTime - _oldTime) * PlayRate / 100;
                _oldTime = curTime;

                if (CurrentPosition > SongLength)
                {
                    CurrentPosition = SongLength;
                    CurrentPosition = SongLength;
                    _playTimer.Stop();
                }

                EventBus.Instance.Publish(new CurPositionEvent {CurPosition = CurrentPosition});
            };

            ErrorTimer.Interval = TimeSpan.FromSeconds(2);
            ErrorTimer.Tick += (sender, args) =>
            {
                OffsetErrorOccurred = false;
                ErrorTimer.Stop();
            };

            EventBus.Instance.RegisterHandler(this);

            _dialogManager = new CustomDialogManager(new MetroDialogSettings
            {
                AnimateShow = false,
                AnimateHide = false
            });
        }

        public void InitSettings(OpenSettings settings)
        {
            if (settings.Method == StartMethod.Open)
            {
                _mapsetPath = settings.MapsetPath;
                var osuFiles = Directory.GetFiles(_mapsetPath, "*.osu", SearchOption.TopDirectoryOnly);
                foreach (var cur in osuFiles)
                {
                    Diffs.Add(new DiffVersion
                    {
                        DiffName = OsuParserExtensions.GetDiffName(cur),
                        FileName = cur
                    });
                }
            }
            if (settings.Method == StartMethod.Create)
                InitialCommand.Execute(null);
        }
        
        #region Commands
        #region TimingMark 관련
        public ICommand AddTimingMarkCommand
        {
            get
            {
                return Get(() => AddTimingMarkCommand, new RelayCommand(() =>
                {
                    var normalizeCurrentOffset = (int)Math.Round(CurrentPosition);

                    if (TimingMarks.Count == 0)
                    {
                        TimingMarks.Add(new TimingMark {Offset = normalizeCurrentOffset});
                        CurrentTiming = TimingMarks[0];
                        EventBus.Instance.Publish(new CurrentTimingChangedEvent());
                        return;
                    }
                    
                    //  Find previous TimingMark
                    TimingMark prevMark = null;
                    foreach (var timing in TimingMarks)
                    {
                        if (normalizeCurrentOffset == timing.Offset)
                        {
                            CurrentTiming = timing;
                            return;
                        }
                        if (normalizeCurrentOffset > timing.Offset)
                            prevMark = timing;
                    }

                    if (prevMark == null)
                        prevMark = TimingMarks.First();

                    var newTiming = new TimingMark(prevMark)
                    {
                        Offset = normalizeCurrentOffset,
                        Preview = false,
                        Bookmark = string.Empty
                    };

                    TimingMarks.Add(newTiming);
                    TimingMarks = new ObservableCollection<TimingMark>(TimingMarks.OrderBy(x => x.Offset));
                    CurrentTiming = newTiming;
                    EventBus.Instance.Publish(new CurrentTimingChangedEvent
                    {
                        Bpm = CurrentTiming.Bpm,
                        Offset = CurrentTiming.Offset
                    });
                }));
            }
        }

        public ICommand DeleteTimingMarkCommand
        {
            get
            {
                return Get(() => DeleteTimingMarkCommand, new RelayCommand(() =>
                {
                    TimingMarks.Remove(CurrentTiming);
                }));
            }
        }

        #region Current Timing 관련
        public ICommand UseCurrentTimeCommand
        {
            get
            {
                return Get(() => UseCurrentTimeCommand, new RelayCommand(() =>
                {
                    if (CurrentTiming == null)
                        return;

                    var normalizeCurrentOffset = (int) Math.Round(CurrentPosition);

                    foreach (var timing in TimingMarks)
                    {
                        if (timing == CurrentTiming)
                            continue;

                        if (timing.Offset == normalizeCurrentOffset)
                        {
                            OffsetErrorOccurred = true;
                            if (ErrorTimer.IsEnabled)
                                ErrorTimer.Stop();
                            ErrorTimer.Start();

                            return;
                        }
                    }

                    CurrentTiming.Offset = (int) Math.Round(CurrentPosition);
                    CurrentTimingChangedCommand.Execute(null);
                }));
            }
        }
        #endregion

        public ICommand PreviewCheckedCommand
        {
            get
            {
                return Get(() => PreviewCheckedCommand, new RelayCommand(() =>
                {
                    foreach (var timing in TimingMarks)
                        timing.Preview = timing == CurrentTiming;
                    CurrentTimingChangedCommand.Execute(null);
                }));
            }
        }
        public ICommand CurrentTimingChangedCommand
        {
            get
            {
                return Get(() => CurrentTimingChangedCommand, new RelayCommand(() =>
                {
                    EventBus.Instance.Publish(new TimingChangedEvent
                    {
                        NewTiming = TimingConverter.TimingMarkListToTiming(TimingMarks)
                    });
                }));
            }
        }
        #endregion

        #region Difficulty Panel 관련
        public ICommand AddDifficultyCommand
        {
            get { return Get(() => AddDifficultyCommand, new RelayCommand(() => { })); }
        }

        public ICommand OpenDifficultyCommand
        {
            get { return Get(() => OpenDifficultyCommand, new RelayCommand(() => { })); }
        }

        public ICommand RenameDifficultyCommand
        {
            get { return Get(() => RenameDifficultyCommand, new RelayCommand(() => { })); }
        }

        public ICommand DeleteDifficultyCommand
        {
            get { return Get(() => DeleteDifficultyCommand, new RelayCommand(() => { })); }
        }
        #endregion

        #region Dialog 관련
        public ICommand InitialCommand
        {
            get
            {
                return Get(() => InitialCommand, new RelayCommand(async () =>
                {
                    var initSettingView = new InitialSettingView(new InitSettings
                    {
                        Mp3Path = CurrentMap.Gen.AudioFilename,
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
        #endregion

        #region MusicBar 관련
        public ICommand PlayCommand
        {
            get
            {
                return Get(() => PlayCommand, new RelayCommand(() =>
                {
                    if (_playTimer.IsEnabled)
                    {
                        _playTimer.Stop();
                        _stopWatch.Reset();
                        CurrentPosition = 0;
                        EventBus.Instance.Publish(new CurPositionEvent { CurPosition = CurrentPosition });
                    }

                    if (Math.Abs(SongLength) < 0.001 || CurrentDiff == null)
                        return;

                    _oldTime = 0;
                    _playTimer.Start();
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
                    if (_playTimer.IsEnabled)
                    {
                        _playTimer.Stop();
                        _stopWatch.Stop();
                    }
                    else
                    {
                        _oldTime = 0;
                        _playTimer.Start();
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
                    if (_playTimer.IsEnabled)
                    {
                        _playTimer.Stop();
                        _stopWatch.Reset();
                    }
                    CurrentPosition = 0;
                    EventBus.Instance.Publish(new CurPositionEvent { CurPosition = CurrentPosition });
                }));
            }
        }

        public ICommand TestCommand
        {
            get { return Get(() => TestCommand, new RelayCommand(() => { })); }
        }
        #endregion
        #endregion

        #region Event Handler
        public void HandleEvent(ChangeCurrentMapEvent e)
        {
            CurrentMap = Parser.LoadOsuFile(e.OsuFileName);

            var reader = new Mp3FileReader(Path.Combine(_mapsetPath, CurrentMap.Gen.AudioFilename));
            SongLength = reader.TotalTime.TotalMilliseconds;

            var bookmarks = new List<Bookmark>();
            foreach (var cur in CurrentMap.Edit.Bookmarks)
                bookmarks.Add(new Bookmark
                {
                    Offset = cur,
                    Memo = string.Empty
                });

            TimingMarks = TimingConverter.TimingPointListToTimingMarkList(CurrentMap.Timing, CurrentMap.Gen.PreviewTime, bookmarks);
            CurrentTiming = TimingMarks[0];
            EventBus.Instance.Publish(new TimingChangedEvent
            {
                NewTiming = TimingConverter.TimingMarkListToTiming(TimingMarks)
            });

            EventBus.Instance.Publish(new BeatSnapEvent
            {
                Snap = CurrentMap.Edit.BeatDivisor
            });
        }
        #endregion
    }
}
