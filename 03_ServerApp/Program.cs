using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _03_ServerApp
{
    internal class Program
    {
        private const string BindIp = "127.0.0.1";
        private const int Port = 4040;

        // Mapping per 2004+ Ukrainian regional prefixes (Latin letters only).
        // Names are in English transliteration with "oblast" suffix; cities labeled "city".
        private static readonly Dictionary<string, string> Regions = new()
        {
            ["AA"] = "Kyiv city",
            ["AB"] = "Vinnytska oblast",
            ["AC"] = "Volynska oblast",
            ["AE"] = "Dnipropetrovska oblast",
            ["AH"] = "Donetska oblast",
            ["AI"] = "Kyivska oblast",
            ["AK"] = "Autonomous Republic of Crimea",
            ["AM"] = "Zhytomyrska oblast",
            ["AO"] = "Zakarpatska oblast",
            ["AP"] = "Zaporizka oblast",
            ["AT"] = "Ivano-Frankivska oblast",
            ["AX"] = "Kharkivska oblast",
            ["BA"] = "Kirovohradska oblast",
            ["BB"] = "Luhanska oblast",
            ["BC"] = "Lvivska oblast",
            ["BE"] = "Mykolaivska oblast",
            ["BH"] = "Odeska oblast",
            ["BI"] = "Poltavska oblast",
            ["BK"] = "Rivnenska oblast",
            ["BM"] = "Sumska oblast",
            ["BO"] = "Ternopilska oblast",
            ["BT"] = "Khersonska oblast",
            ["BX"] = "Khmelnytska oblast",
            ["CA"] = "Cherkaska oblast",
            ["CB"] = "Chernihivska oblast",
            ["CE"] = "Chernivetska oblast",
            ["CH"] = "Sevastopol city",
        };

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var listener = new TcpListener(IPAddress.Parse(BindIp), Port);
            listener.Start();
            Console.WriteLine($"Server listening on {BindIp}:{Port}. Waiting for a single client...");

            using TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");

            using NetworkStream ns = client.GetStream();
            using var reader = new StreamReader(ns, Encoding.UTF8, leaveOpen: true);
            using var writer = new StreamWriter(ns, Encoding.UTF8, leaveOpen: true) { AutoFlush = true };

            try
            {
                while (true)
                {
                    string? line = await reader.ReadLineAsync();
                    if (line is null) break;

                    string normalized = Normalize(line);
                    string response = Lookup(normalized);
                    await writer.WriteLineAsync(response);
                    Console.WriteLine($"Request: '{line}' -> '{response}'");
                }
            }
            catch (IOException)
            {

            }
            finally
            {
                listener.Stop();
                Console.WriteLine("Server stopped.");
            }
        }

        private static string Normalize(string input)
        {
            input = (input ?? string.Empty).Trim().ToUpperInvariant();
            return input;
        }

        private static string Lookup(string code)
        {
            if (code.Length != 2) return "Unknown";
            if (!IsLatinLetter(code[0]) || !IsLatinLetter(code[1])) return "Unknown";
            return Regions.TryGetValue(code, out var region) ? region : "Unknown";
        }

        private static bool IsLatinLetter(char c)
            => c is >= 'A' and <= 'Z';
    }
}
