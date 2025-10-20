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

            var login = new LoginWindow();
            bool? ok = login.ShowDialog();

            if (ok == true)
            {
                var main = new MainWindow(login.Nick);
                main.Show();
            }
            else
            {
                Shutdown();
            }
        }
    }
}
