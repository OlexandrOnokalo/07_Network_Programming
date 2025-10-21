using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace _02_messenger_client
{
    public partial class MainWindow : Window
    {
        // ===== CONFIG =====
        private readonly string _serverHost = "127.0.0.1"; // IP/домен сервера
        private readonly int _serverPort = 9000;           // порт сервера

        // ===== STATE =====
        private readonly string _nick;
        private UdpClient _udp;
        private CancellationTokenSource _cts;
        private readonly ObservableCollection<string> _messages = new();

        public MainWindow(string nick)
        {
            InitializeComponent();

            _nick = nick ?? string.Empty;
            Title = string.IsNullOrWhiteSpace(_nick) ? "Chat" : $"Chat — {_nick}";

            // У тебе в XAML: <ListBox x:Name="list" ItemsSource="{Binding}">
            // Тому підв'язуємо всю колекцію через DataContext:
            DataContext = _messages;
        }

        // JOIN
        private async void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_udp == null)
                {
                    // ОС одразу робить Bind на локальний еферемерний порт
                    _udp = new UdpClient(0);
                    _udp.Connect(_serverHost, _serverPort);
                }

                // Старт прийому після Connect
                _cts?.Cancel();
                _cts = new CancellationTokenSource();
                _ = Task.Run(() => ReceiveLoopAsync(_cts.Token));

                // Надсилаємо JOIN
                string join = $"$<join>|{_nick}";
                byte[] data = Encoding.UTF8.GetBytes(join);
                await _udp.SendAsync(data, data.Length);

                _messages.Add($"Joined as '{_nick}'.");
                // Автопрокрутка ListBox на останній елемент:
                if (list.Items.Count > 0)
                    list.ScrollIntoView(list.Items[^1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Join error: {ex.Message}", "Network Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // SEND
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string msg = msgText.Text?.Trim();
                if (string.IsNullOrWhiteSpace(msg))
                    return;

                string payload = $"{_nick}: {msg}";
                byte[] data = Encoding.UTF8.GetBytes(payload);
                await _udp.SendAsync(data, data.Length);

                msgText.Clear();
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Connection is closed.", "Network",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Send error: {ex.Message}", "Network Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // LEAVE (кнопка)
        private async void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            await LeaveAndCloseAsync(addToLog: true);
        }

        // Прийом у циклі
        private async Task ReceiveLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var res = await _udp.ReceiveAsync();               // сокет уже прив'язаний
                    string text = Encoding.UTF8.GetString(res.Buffer); // UTF-8

                    await Dispatcher.InvokeAsync(() =>
                    {
                        _messages.Add(text);
                        if (list.Items.Count > 0)
                            list.ScrollIntoView(list.Items[^1]);
                    });
                }
            }
            catch (ObjectDisposedException)
            {
                // закрито при виході — ок
            }
            catch (SocketException)
            {
                // мережевий збій під час закриття — ок
            }
            catch (Exception ex)
            {
                await Dispatcher.InvokeAsync(() =>
                    MessageBox.Show($"Receive error: {ex.Message}", "Network Error",
                        MessageBoxButton.OK, MessageBoxImage.Error));
            }
        }

        // Акуратне завершення
        private async Task LeaveAndCloseAsync(bool addToLog)
        {
            try
            {
                if (_udp != null)
                {
                    string leave = $"$<leave>|{_nick}";
                    byte[] data = Encoding.UTF8.GetBytes(leave);
                    try { await _udp.SendAsync(data, data.Length); } catch { /* ignore */ }
                }
            }
            catch { /* ignore */ }
            finally
            {
                _cts?.Cancel();
                _cts = null;

                try { _udp?.Close(); } catch { /* ignore */ }
                _udp = null;

                if (addToLog)
                {
                    _messages.Add("Left.");
                    if (list.Items.Count > 0)
                        list.ScrollIntoView(list.Items[^1]);
                }
            }
        }

        // На закриття вікна — теж шлемо LEAVE
        protected override async void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try { await LeaveAndCloseAsync(addToLog: false); } catch { }
            base.OnClosing(e);
        }
    }
}
