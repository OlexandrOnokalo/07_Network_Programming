using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _01_Client
{
    public partial class MainWindow : Window
    {
        private UdpClient udpClient;
        private IPEndPoint serverEndpoint;

        public MainWindow()
        {
            InitializeComponent();

       
            txtIP.Text = "127.0.0.1";
            txtPort.Text = "8080";

            btnSend.Click += BtnSend_Click;
            txtMessage.KeyDown += TxtMessage_KeyDown;

            StartListener();
        }

        private void AddDialog(string who, string message)
        {
            var time = DateTime.Now.ToString("HH:mm:ss");
            lstDialog.Items.Add($"[{time}] {who}: {message}");
            lstDialog.ScrollIntoView(lstDialog.Items[lstDialog.Items.Count - 1]);
        }

        private async void StartListener()
        {
            try
            {
                udpClient = new UdpClient(0); 
                udpClient.EnableBroadcast = false;
                
                await Task.Run(async () =>
                {
                    while (true)
                    {
                        try
                        {
                            var result = await udpClient.ReceiveAsync();
                            string msg = Encoding.Unicode.GetString(result.Buffer);
                            
                            Dispatcher.Invoke(() =>
                            {
                                AddDialog("Server", msg);
                            });
                        }
                        catch (ObjectDisposedException)
                        {
                            break;
                        }
                        catch (Exception)
                        {
                            
                        }
                    }
                });
            }
            catch (Exception)
            {
                
            }
        }

        private void TxtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _ = SendMessageAsync();
                e.Handled = true;
            }
        }

        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            await SendMessageAsync();
        }

        private async Task SendMessageAsync()
        {
            try
            {
                var ip = txtIP.Text.Trim();
                if (!int.TryParse(txtPort.Text.Trim(), out int port))
                {
                    MessageBox.Show("Port is invalid");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtMessage.Text))
                    return;

                string message = txtMessage.Text.Trim();
                AddDialog("Client", message);

               
                serverEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);
                udpClient.Connect(serverEndpoint);
                byte[] data = Encoding.Unicode.GetBytes(message);
                await udpClient.SendAsync(data, data.Length);

                txtMessage.Clear();
                txtMessage.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Send error: " + ex.Message);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                udpClient?.Close();
            }
            catch { }
            base.OnClosed(e);
        }
    }
}
