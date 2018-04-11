using System.IO;
using System.Windows;
using System.Windows.Input;
using OsuEditor.Models.Dialogs;
using OsuEditor.ViewModels;

namespace OsuEditor.Contents
{
    public partial class InitialSettingView
    {
        public InitialSettingView(InitSettings settings)
        {
            InitializeComponent();
            DataContext = new InitialSettingViewModel(settings);
        }

        private void IntTextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (var c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    break;
                }
            }
        }

        private void InitialSettingView_OnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            var path = ((string[]) e.Data.GetData(DataFormats.FileDrop))?[0];
            if(File.Exists(path))
                ((InitialSettingViewModel) DataContext).Mp3Path = path;
        }
    }
}
