using System.Windows.Input;
using OsuEditor.Commands;
using OsuEditor.Models;

namespace OsuEditor.ViewModels
{
    public class EditorSettingViewModel : DialogViewModelBase<EditorSettings>
    {
        #region Properties
        public string MapsetRoot
        {
            get { return Get(() => MapsetRoot); }
            set { Set(() => MapsetRoot, value); }
        }
        #endregion

        public EditorSettingViewModel(EditorSettings settings)
        {
            MapsetRoot = settings.MapsetRoot;
        }

        #region Commands
        public ICommand SaveCommand
        {
            get
            {
                return Get(() => SaveCommand,
                    new RelayCommand(() => { Close(new EditorSettings(MapsetRoot)); },
                        () => !string.IsNullOrWhiteSpace(MapsetRoot)));
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
