using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using OsuEditor.Commands;

namespace OsuEditor.ViewModels
{
    public class InitialSettingViewModel : ViewModelBase
    {
        public string Mp3Path
        {
            get { return Get(() => Mp3Path); }
            set { Set(() => Mp3Path, value); }
        }

        private readonly MetroWindow _owner;
        private readonly CustomDialog _view;

        public InitialSettingViewModel(Window owner, CustomDialog view)
        {
            _owner = owner as MetroWindow;
            _view = view;
        }

        public ICommand SaveCommand
        {
            get
            {
                return Get(() => SaveCommand, new RelayCommand(() =>
                {
                    _owner.HideMetroDialogAsync(_view);
                }, () => !string.IsNullOrWhiteSpace(Mp3Path)));
            }
        }

        public ICommand CloseCommand
        {
            get
            {
                return Get(() => CloseCommand, new RelayCommand(() =>
                {
                    _owner.HideMetroDialogAsync(_view);
                }));
            }
        }
    }
}
