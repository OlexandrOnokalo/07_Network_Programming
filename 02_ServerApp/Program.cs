using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _02_ServerApp
{
    class Server
    {

        private readonly int port = 9000; 


        private const string JOIN_PREFIX = "$<join>|";
        private const string LEAVE_PREFIX = "$<leave>|";


        private readonly Dictionary<IPEndPoint, string> members = new Dictionary<IPEndPoint, string>();

        private readonly UdpClient server_udp;

        public Server()
        {
            server_udp = new UdpClient(port);
            Console.WriteLine($"Server is listening UDP on port {port}");
        }

        public void Start()
        {
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 0);

            while (true)
            {
                byte[] data = server_udp.Receive(ref remote);
                string message = Encoding.UTF8.GetString(data);


                Console.WriteLine($"Message : {message}. From : {remote}");


                if (message.StartsWith(JOIN_PREFIX, StringComparison.Ordinal))
                {
                    string nick = message.Substring(JOIN_PREFIX.Length).Trim();
                    HandleJoin(remote, nick);
                    continue;
                }


                if (message.StartsWith(LEAVE_PREFIX, StringComparison.Ordinal))
                {
                    string nick = message.Substring(LEAVE_PREFIX.Length).Trim();
                    HandleLeave(remote, nick);
                    continue;
                }


                Broadcast(data);
            }
        }

        private void HandleJoin(IPEndPoint client, string nick)
        {
            if (!members.ContainsKey(client))
            {
                members[client] = string.IsNullOrWhiteSpace(nick) ? "(unknown)" : nick;
                Console.WriteLine($"Member added: {client} as '{members[client]}'");


                string joinedMsg = $"[system] {members[client]} joined";
                Broadcast(Encoding.UTF8.GetBytes(joinedMsg));
            }
            else
            {

                members[client] = nick;
            }
        }

        private void HandleLeave(IPEndPoint client, string nick)
        {
            if (members.TryGetValue(client, out var oldNick))
            {
                members.Remove(client);
                Console.WriteLine($"Member removed: {client} ('{oldNick}')");

                string leftMsg = $"[system] {oldNick} left";
                Broadcast(Encoding.UTF8.GetBytes(leftMsg));
            }
            else
            {

            }
        }

        private void Broadcast(byte[] data)
        {

            foreach (var ep in members.Keys.ToList())
            {

                server_udp.SendAsync(data, data.Length, ep);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var server = new Server();
            server.Start();
        }
    }
}
