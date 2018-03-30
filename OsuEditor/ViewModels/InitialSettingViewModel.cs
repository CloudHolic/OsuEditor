using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using OsuEditor.Commands;
using OsuEditor.Models;

namespace OsuEditor.ViewModels
{
    public class InitialSettingViewModel : DialogViewModelBase<InitSettings>
    {
        #region Properties
        public string Mp3Path
        {
            get { return Get(() => Mp3Path); }
            set { Set(() => Mp3Path, value); }
        }

        public PlayMode Mode
        {
            get { return Get(() => Mode); }
            set { Set(() => Mode, value); }
        }

        public int Keys
        {
            get { return Get(() => Keys); }
            set { Set(() => Keys, value); }
        }

        public bool SpecialStyle
        {
            get { return Get(() => SpecialStyle); }
            set { Set(() => SpecialStyle, value); }
        }
        #endregion

        public InitialSettingViewModel(InitSettings settings)
        {
            Mp3Path = settings.Mp3Path;
            Mode = settings.Mode;
            Keys = settings.Keys;
            SpecialStyle = settings.SpecialStyle;
        }

        #region Commands
        public ICommand OpenFileCommand
        {
            get
            {
                return Get(() => OpenFileCommand, new RelayCommand(() =>
                {
                    var ofd = new OpenFileDialog
                    {
                        Filter = @"MP3 files (*.mp3)|*.mp3|All files (*.*)|*.*",
                        RestoreDirectory = true
                    };

                    var result = ofd.ShowDialog();
                    if (result.HasValue && result.Value)
                        Mp3Path = ofd.FileName;
                }));
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return Get(() => SaveCommand,
                    new RelayCommand(() => { Close(new InitSettings(Mp3Path, Mode, Keys, SpecialStyle)); },
                        () => !string.IsNullOrWhiteSpace(Mp3Path)));
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return Get(() => CloseCommand, new RelayCommand(() => { Close(null); }));
            }
        }
        #endregion
    }
}
