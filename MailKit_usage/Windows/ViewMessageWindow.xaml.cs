using System.Linq;
using System.Windows;
using MailKit;
using MailKit_usage.Helpers;
using MimeKit;

namespace MailKit_usage.Windows
{
    public partial class ViewMessageWindow : Window
    {
        private readonly ImapService _imap;
        private readonly IMailFolder _folder;
        private readonly MimeMessage _message;
        private readonly UniqueId _uid;

        public ViewMessageWindow(ImapService imap, IMailFolder folder, MimeMessage message, UniqueId uid)
        {
            InitializeComponent();
            _imap = imap;
            _folder = folder;
            _message = message;
            _uid = uid;

            txtFrom.Text = _message.From?.ToString() ?? "";
            txtTo.Text = _message.To?.ToString() ?? "";
            txtSubject.Text = _message.Subject ?? "";
            txtDate.Text = _message.Date.ToString();
            txtBody.Text = _message.TextBody ?? _message.HtmlBody ?? "(no text)";
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnReply_Click(object sender, RoutedEventArgs e)
        {
            var replyTo = _message.ReplyTo?.FirstOrDefault() ?? _message.From?.FirstOrDefault();
            var toAddress = replyTo?.ToString() ?? "";
            var subject = _message.Subject ?? "";
            if (!subject.StartsWith("Re:", System.StringComparison.OrdinalIgnoreCase))
                subject = "Re: " + subject;

            var compose = new ComposeWindow(toAddress, subject);
            compose.Owner = this;
            compose.ShowDialog();
        }
    }
}