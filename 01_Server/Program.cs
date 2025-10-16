using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _01_Server
{
    class Program
    {
        static string address = "127.0.0.1";
        static int port = 8080;

        static Dictionary<string, string> responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"hi", "hello"},
            {"bye", "goodbye"},
            {"thanks", "welcome"},
            {"ping", "pong"},
            {"name", "server"}
        };

        static async Task Main(string[] args)
        {
            Console.WriteLine($"Server starting on {address}:{port}");
            using (var listener = new UdpClient(new IPEndPoint(IPAddress.Parse(address), port)))
            {
                try
                {
                    while (true)
                    {
                        var result = await listener.ReceiveAsync();
                        var remote = result.RemoteEndPoint;
                        string message = Encoding.Unicode.GetString(result.Buffer);
                        Console.WriteLine($"Received from {remote.Address}:{remote.Port}: \"{message}\"");

                        string key = message.Trim();

                        if (responses.TryGetValue(key, out string reply))
                        {
                            byte[] outData = Encoding.Unicode.GetBytes(reply);
                            await listener.SendAsync(outData, outData.Length, remote);
                            Console.WriteLine($"Sent to {remote.Address}:{remote.Port}: \"{reply}\"");
                        }
                        else
                        {
                            string question = $"What reply should I save for \"{message}\"?";
                            byte[] qData = Encoding.Unicode.GetBytes(question);
                            await listener.SendAsync(qData, qData.Length, remote);
                            Console.WriteLine($"Sent question to {remote.Address}:{remote.Port}: \"{question}\"");

                            // Wait for response from same remote endpoint with timeout
                            var receiveTask = listener.ReceiveAsync();
                            var delayTask = Task.Delay(TimeSpan.FromSeconds(30));
                            var completed = await Task.WhenAny(receiveTask, delayTask);
                            if (completed == receiveTask)
                            {
                                var resp = receiveTask.Result;
                                var respRemote = resp.RemoteEndPoint;
                                string respMessage = Encoding.Unicode.GetString(resp.Buffer).Trim();
                                if (respRemote.Address.Equals(remote.Address) && respRemote.Port == remote.Port)
                                {
                                    responses[key] = respMessage;
                                    Console.WriteLine($"Received reply from {respRemote.Address}:{respRemote.Port}: \"{respMessage}\" — saved mapping {key} -> {respMessage}");
                                    // Optionally send confirmation
                                    string conf = $"Saved reply \"{respMessage}\" for \"{message}\"";
                                    byte[] confData = Encoding.Unicode.GetBytes(conf);
                                    await listener.SendAsync(confData, confData.Length, remote);
                                    Console.WriteLine($"Sent confirmation to {remote.Address}:{remote.Port}: \"{conf}\"");
                                }
                                else
                                {
                                    Console.WriteLine($"Received message from different endpoint {respRemote.Address}:{respRemote.Port}, ignoring as reply.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"No reply received for \"{message}\" within timeout.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
