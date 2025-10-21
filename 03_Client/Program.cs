using System.Net.Sockets;
using System.Text;

namespace _03_Client
{
    internal class Program
    {
        private const string ServerIp = "127.0.0.1";
        private const int ServerPort = 4040;

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine("Client started. Type two-letter code (e.g., BK) or 'exit' to quit.");

            using var client = new TcpClient();
            try
            {
                await client.ConnectAsync(ServerIp, ServerPort);

                using NetworkStream ns = client.GetStream();
                using var reader = new StreamReader(ns, Encoding.UTF8, leaveOpen: true);
                using var writer = new StreamWriter(ns, Encoding.UTF8, leaveOpen: true) { AutoFlush = true };

                while (true)
                {
                    Console.Write("> ");
                    string? input = Console.ReadLine();
                    if (input is null) break;
                    input = input.Trim();

                    if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
                        break;

                    await writer.WriteLineAsync(input);
                    string? response = await reader.ReadLineAsync();
                    if (response is null)
                    {
                        Console.WriteLine("Disconnected by server.");
                        break;
                    }
                    Console.WriteLine(response);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }
        }
    }
}
