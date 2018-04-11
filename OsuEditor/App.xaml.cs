using System.Windows;
using OsuEditor.Contents;
using OsuEditor.Models;
using OsuEditor.Models.Dialogs;

namespace OsuEditor
{
    public partial class App
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            //TODO: Add default settings, load them.
            var openWindow = new OpenWindow(new OpenSettings
            {
                MapsetPath = string.Empty,
                Method = StartMethod.Open,
                Remember = false
            });

            openWindow.ShowDialog();
            if (!(openWindow.Tag is OpenSettings result))
                Current.Shutdown(-1);
            else
            {
                var mainWindow = new MainWindow(result);
                Current.MainWindow = mainWindow;
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                mainWindow.Show();
            }
        }
    }
}
