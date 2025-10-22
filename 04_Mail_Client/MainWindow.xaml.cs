using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _04_Mail_Client
{

    public partial class MainWindow : Window
    {
        public string Login { get; set; }
        public string Password { get; set; }

        private const string server = "smtp.gmail.com";
        private const int port = 587;
        private string _attachmentPath;

        public MainWindow()
        {
            InitializeComponent();

            var loginWindow = new LoginWindow();
            var result = loginWindow.ShowDialog();

            if (result == true)
            {
                Login = loginWindow.Login;
                Password = loginWindow.Password;
                fromTextBox.Text = Login;

            }
            else
            {
                Close();
            }
        }
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private async void sendButton_Click(object sender, RoutedEventArgs e)
        {

            MailMessage message = new MailMessage(fromTextBox.Text, toTextBox.Text, themeTextBox.Text, bodyTextBox.Text);

            message.Priority = MailPriority.High; 

            if (!string.IsNullOrEmpty(_attachmentPath))
                message.Attachments.Add(new Attachment(_attachmentPath));

            SmtpClient client = new SmtpClient(server, port);
            client.EnableSsl = true;

            client.Credentials = new NetworkCredential(Login, Password);

            client.SendCompleted += Client_SendCompleted;
            client.SendAsync(message, message);

        }
        private void Client_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {

            var state = (MailMessage)e.UserState;

            MessageBox.Show($"Message was sent! Subject: {state.Subject}!");
            
        }
        private void attachButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                _attachmentPath = dlg.FileName;
                fileNameText.Text = System.IO.Path.GetFileName(_attachmentPath); 
            }
        }
    }

}
