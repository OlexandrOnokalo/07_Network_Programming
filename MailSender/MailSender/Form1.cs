using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Windows.Forms;

namespace MailSender
{
    public partial class Form1 : Form
    {
        string server= "smtp.gmail.com"; // sets the server address
        int port = 587;                  // int.Parse(ConfigurationManager.AppSettings["gmail_port"]); //sets the server port

        const string username = "lenailyshun@gmail.com";

        const string password = "dqmq yyqu uxfb ikfc";

        public Form1()
        {
            InitializeComponent();

            fromBox.Text = username;
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            // GMAIL - Allow less secure apps
            // old - https://myaccount.google.com/lesssecureapps?pli=1&rapt=AEjHL4MTRm8bMK7-4VvaGO5Ks_mRsfnKW3N7IHVwRioBJMo2SXMcP350EgzMWE8DHhVYavXsrIzpnmTjyDNROWK-Cojf4q1Qjg
            // new - https://stackoverflow.com/questions/72547853/unable-to-send-email-in-c-sharp-less-secure-app-access-not-longer-available
            
            // create a message object
            MailMessage message = new MailMessage(fromBox.Text, toBox.Text, themeBox.Text, bodyBox.Text);

            
            //// send HTML body
            using (StreamReader sr = new StreamReader("mail.html"))
            {
                message.Body = sr.ReadToEnd();
            }
            message.IsBodyHtml = true;
            

            message.Priority = MailPriority.High; // important))
          
            message.Attachments.Add(new Attachment(@"Files/text.txt"));
            message.Attachments.Add(new Attachment(@"Files/nuts.jpg"));
         
            // create a send object
            SmtpClient client = new SmtpClient(server, port);
            client.EnableSsl = true;

            // settings for sending mail
            client.Credentials = new NetworkCredential(username, password);

     
            client.SendCompleted  += Client_SendCompleted;
            // call asynchronous message sending
            client.SendAsync(message, message );

          
        }

        private void Client_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
           
            var state = (MailMessage)e.UserState;
           
            MessageBox.Show($"Message was sent! Subject: {state.Subject}!");
            //MessageBox.Show($"Message was sent! Subject: {e.UserState.ToString()}!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // here you need to add a file selection dialog box and add the selected file to the MailMessage
            // attachment collection and to the list on the form
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // here you need to clear all fields of the form and the content of the MailMessage object
        }
    }
}
