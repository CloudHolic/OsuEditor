using System.Windows.Input;
using System.Windows.Forms;
using MahApps.Metro.Controls;
using OsuEditor.Commands;
using OsuEditor.Models;
using OsuEditor.Models.Dialogs;

namespace OsuEditor.ViewModels
{
    public class OpenWindowViewModel : ViewModelBase
    {
        #region Properties
        public StartMethod OpenCreate
        {
            get { return Get(() => OpenCreate); }
            set { Set(() => OpenCreate, value); }
        }

        public string DirectoryPath
        {
            get { return Get(() => DirectoryPath); }
            set { Set(() => DirectoryPath, value); }
        }

        public bool RememberPath
        {
            get { return Get(() => RememberPath); }
            set { Set(() => RememberPath, value); }
        }
        #endregion

        public OpenWindowViewModel(OpenSettings settings)
        {
            OpenCreate = settings.Method;
            DirectoryPath = settings.MapsetPath;
            RememberPath = settings.Remember;
        }

        #region Commands
        public ICommand BrowseCommand
        {
            get {
                return Get(() => BrowseCommand, new RelayCommand(() =>
                {
                    var fbd = new FolderBrowserDialog();
                    var result = fbd.ShowDialog();
                    if (result == DialogResult.OK)
                        DirectoryPath = fbd.SelectedPath;
                }));
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return Get(() => SaveCommand, new RelayCommand<MetroWindow>(window =>
                {
                    window.Tag = new OpenSettings
                    {
                        MapsetPath = DirectoryPath,
                        Method = OpenCreate,
                        Remember = RememberPath
                    };
                    window.Close();
                }, window => OpenCreate == StartMethod.Create || !string.IsNullOrWhiteSpace(DirectoryPath)));
            }
        }

        public ICommand CloseCommand
        {
            get { return Get(() => CloseCommand, new RelayCommand<MetroWindow>(window => { window.Close(); })); }
        }
        #endregion
    }
}
