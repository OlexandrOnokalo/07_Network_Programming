using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _02_messenger_client
{
    public partial class LoginWindow : Window
    {
        public string Nick { get; private set; } = string.Empty;

        public LoginWindow()
        {
            InitializeComponent();
            txtNick.Focus();
        }

        private void TxtNick_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            
            btnOk.IsEnabled = !string.IsNullOrWhiteSpace(txtNick.Text);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Nick = txtNick.Text.Trim();
            DialogResult = true; 
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

