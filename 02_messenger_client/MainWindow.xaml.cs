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
        // ====== CONFIG ======
        private readonly string _serverHost = "127.0.0.1"; // або IP/домен твого сервера
        private readonly int _serverPort = 9000;           // порт сервера

        // ====== STATE ======
        private readonly string _nick;
        private UdpClient _udp;
        private CancellationTokenSource _cts;
        private readonly ObservableCollection<string> _messages = new();

        public MainWindow(string nick)
        {
            InitializeComponent();
            _nick = nick;
            Title = $"Chat — {_nick}";
            _nick = nick ?? string.Empty;
            MessagesListBox.ItemsSource = _messages;
            Title = string.IsNullOrWhiteSpace(_nick) ? "Chat" : $"Chat — {_nick}";

        }
        public MainWindow()
        {
            InitializeComponent();


        }
        private async void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_udp == null)
                {
                    // ОС одразу робить Bind на еферемерний локальний порт
                    _udp = new UdpClient(0);

                    // Фіксуємо адресу сервера (куди слати)
                    _udp.Connect(_serverHost, _serverPort);
                }

                // Стартуємо цикл прийому ОДРАЗУ після Connect
                _cts?.Cancel();
                _cts = new CancellationTokenSource();
                _ = Task.Run(() => ReceiveLoopAsync(_cts.Token));

                // Надсилаємо JOIN
                string join = $"$<join>|{_nick}";
                byte[] data = Encoding.UTF8.GetBytes(join);
                await _udp.SendAsync(data, data.Length);

                _messages.Add($"Joined as '{_nick}'.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Join error: {ex.Message}", "Network Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Надсилання звичайного повідомлення
        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string msg = MessageTextBox.Text?.Trim();
                if (string.IsNullOrWhiteSpace(msg))
                    return;

                string payload = $"{_nick}: {msg}";
                byte[] data = Encoding.UTF8.GetBytes(payload);
                await _udp.SendAsync(data, data.Length);

                MessageTextBox.Clear();
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

        // LEAVE (відключення)
        private async void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            await LeaveAndCloseAsync(addToLog: true);
        }

        // Основний цикл прийому
        private async Task ReceiveLoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    // Тут уже є локальний Bind (бо UdpClient створений з 0)
                    UdpReceiveResult res = await _udp.ReceiveAsync();
                    string text = Encoding.UTF8.GetString(res.Buffer);

                    // Оновлюємо UI потік
                    await Dispatcher.InvokeAsync(() =>
                    {
                        _messages.Add(text);
                        // За бажанням: автопрокрутка ListBox на останнє повідомлення
                        if (MessagesListBox.Items.Count > 0)
                            MessagesListBox.ScrollIntoView(MessagesListBox.Items[^1]);
                    });
                }
            }
            catch (ObjectDisposedException)
            {
                // сокет закрито під час завершення — це нормально
            }
            catch (SocketException)
            {
                // мережеві збої під час закриття — ігноруємо
            }
            catch (Exception ex)
            {
                await Dispatcher.InvokeAsync(() =>
                    MessageBox.Show($"Receive error: {ex.Message}", "Network Error",
                        MessageBoxButton.OK, MessageBoxImage.Error));
            }
        }

        // Акуратне завершення сесії і закриття сокета
        private async Task LeaveAndCloseAsync(bool addToLog)
        {
            try
            {
                if (_udp != null)
                {
                    // Надішлемо $<leave>|nick (спробуємо; якщо впаде — не страшно)
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
                    _messages.Add("Left.");
            }
        }

        // На закриття вікна — теж робимо LEAVE
        protected override async void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // щоб діалогів не було під час закриття
            try
            {
                await LeaveAndCloseAsync(addToLog: false);
            }
            catch { /* ignore */ }

            base.OnClosing(e);
        }
    }
}










    }
}