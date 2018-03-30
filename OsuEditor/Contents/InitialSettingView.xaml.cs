using System.Windows.Input;
using OsuEditor.Models;
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
    }
}
