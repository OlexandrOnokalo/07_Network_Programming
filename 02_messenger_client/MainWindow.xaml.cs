using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;



namespace _02_messenger_client
{
    public partial class MainWindow : Window
    {
        private readonly string _nick;

        private UdpClient _client;
        private readonly IPEndPoint _serverEndPoint = new IPEndPoint(IPAddress.Loopback, 4040);
        private volatile bool _listening;

        public MainWindow(string nick)
        {
            InitializeComponent();
            _nick = nick;

        }
        private async void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _client ??= new UdpClient();
                _listening = true;

                StartListening();

                
                await SendRaw($"$<join>|{_nick}");

                
                list.Items.Add($"You ({_nick}) joined the chat.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to join the chat: {ex.Message}",
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string text = msgText.Text.Trim();

            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("Message cannot be empty!",
                                "Warning",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }


            await SendRaw($"{_nick}|{text}");
            msgText.Clear();
        }


        private async void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SendRaw($"$<leave>|{_nick}");
            }
            catch { }

            _listening = false;
            _client?.Dispose();
            _client = null;
            list.Items.Add("You left the chat");


            this.Close();
        }
        protected override async void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                await SendRaw($"$<leave>|{_nick}");
            }
            catch { }

            _listening = false;
            _client?.Dispose();
            _client = null;

            base.OnClosing(e);
        }



        private async Task SendRaw(string payload)
        {
            try
            {
                
                _client ??= new UdpClient();

                byte[] data = Encoding.UTF8.GetBytes(payload);
                await _client.SendAsync(data, data.Length, _serverEndPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send message: {ex.Message}",
                                "Network Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
        private void StartListening()
        {
            _client ??= new UdpClient();
            _listening = true;

            Task.Run(async () =>
            {
                try
                {
                    while (_listening)
                    {
                        
                        UdpReceiveResult res = await _client.ReceiveAsync();
                        string raw = Encoding.UTF8.GetString(res.Buffer);


                        if (raw.Contains('|'))
                        {
                            var parts = raw.Split('|', 2);
                            string nick = parts[0];
                            string text = parts.Length > 1 ? parts[1] : string.Empty;


                            Dispatcher.Invoke(() =>
                            {
                                
                                list.Items.Add($"{nick}: {text}");
                            });
                        }
                        else
                        {
                            Dispatcher.Invoke(() =>
                            {
                                
                                list.Items.Add(raw); 
                            });
                        }
                    }
                }

                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show($"Receive error: {ex.Message}", "Network Error",
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                }
            });
        }










    }
}