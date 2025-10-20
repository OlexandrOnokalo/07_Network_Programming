using System.Configuration;
using System.Data;
using System.Windows;
using _02_messenger_client;

namespace _02_messenger_client
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Prevent the app from auto-shutting down when the login dialog closes.
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var login = new LoginWindow
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowInTaskbar = false
            };

            bool? ok = login.ShowDialog();     // show login as a true dialog

            if (ok == true && !string.IsNullOrWhiteSpace(login.Nick))
            {
                var main = new MainWindow(login.Nick);
                MainWindow = main;             // assign main window
                main.Show();                   // show main window

                // Now restore normal shutdown behavior tied to the main window.
                ShutdownMode = ShutdownMode.OnMainWindowClose;
            }
            else
            {
                Shutdown();                    // cancel/empty nick — close the app
            }
        }
    }
}
