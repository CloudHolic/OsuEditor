using System.IO;
using System.Windows;
using OsuEditor.Models.Dialogs;
using OsuEditor.ViewModels;

namespace OsuEditor.Contents
{
    public partial class OpenWindow
    {
        public OpenWindow(OpenSettings settings)
        {
            InitializeComponent();
            DataContext = new OpenWindowViewModel(settings);
        }

        private void OpenWindow_OnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            var path = ((string[])e.Data.GetData(DataFormats.FileDrop))?[0];
            if(Directory.Exists(path))
                ((OpenWindowViewModel)DataContext).DirectoryPath = path;
        }
    }
}
