using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Authentication;
using System.Threading;



namespace mark_as_read
{

    internal class Program
    {

        private const string Email = "lenailyshun@gmail.com";
        private const string AppPasswordRaw = "dqmq yyqu uxfb ikfc";
        private const string ImapHost = "imap.gmail.com";
        private const int ImapPort = 993;


        private const int BatchSize = 1000;               
        private const int PauseMsBetweenBatches = 500;    

        private static void Main()
        {

            var appPassword = AppPasswordRaw.Replace(" ", "");


                using var client = new ImapClient { Timeout = 120_000 };

                Console.WriteLine("Connecting to Gmail IMAP...");
                client.Connect(ImapHost, ImapPort, SecureSocketOptions.SslOnConnect);

                Console.WriteLine("Authenticating...");
                client.Authenticate(Email, appPassword);


                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite);
                Console.WriteLine("Opened folder: INBOX");


                Console.WriteLine("Counting UNREAD messages in Inbox...");
                var unreadUids = inbox.Search(SearchQuery.NotSeen);
                var total = unreadUids.Count;
                Console.WriteLine($"UNREAD messages in Inbox: {total}");

                if (total == 0)
                {
                    Console.WriteLine("Nothing to do. All messages are already read.");
                    inbox.Close();
                    client.Disconnect(true);
                    return;
                }

                Console.Write("Proceed to mark all as read? (y/N): ");
                var answer = Console.ReadLine();
                if (answer != "y" && answer != "yes")
                {
                    Console.WriteLine("Aborted by user. No changes made.");
                    inbox.Close();
                    client.Disconnect(true);
                    return;
                }


                int processed = 0;
                for (int i = 0; i < total; i += BatchSize)
                {
                    var slice = unreadUids.Skip(i).Take(Math.Min(BatchSize, total - i)).ToList();


                    inbox.AddFlags(slice, MessageFlags.Seen, true);

                    processed += slice.Count;
                    Console.WriteLine($"Marked as read: {processed}/{total}");

                    Thread.Sleep(PauseMsBetweenBatches);
                }

                Console.WriteLine("Done. Closing folder...");
                inbox.Close();
                client.Disconnect(true);
                Console.WriteLine("All unread Inbox messages have been marked as read.");

        }
    }

}
