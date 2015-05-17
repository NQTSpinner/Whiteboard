using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhiteboardApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private string URL = "107.170.241.204/api/authenticate?user=[username]&pass=[pass]";
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            URL = "http://107.170.241.204/api/authenticate?user=" + UsernameBox.Text + "&pass=" + PasswordBox.Text;
            HttpClient client = new HttpClient();
            string a = await client.GetStringAsync(URL);
            a = a.Substring(18, a.Length - 18 - 1);
            if (String.Compare(a, "true") == 0)
            {
                if (this.Frame != null)
                {
                    this.Frame.Navigate(typeof(MainPage));
                }
            }
            else if (String.Compare(a, "\"User Doesnt Exist\"") == 0)
            {
                URL = "http://107.170.241.204/api/createaccount?user=" + UsernameBox.Text + "&pass=" + PasswordBox.Text;
                HttpClient clientB = new HttpClient();
                string b = await client.GetStringAsync(URL);
                b = b.Substring(18, a.Length - 18 - 1);
                LoginMessageBlock.Text = "User account created, please sign in.";
            }
            else {
                LoginMessageBlock.Text = "Incorrect password";
            }
        }
    }
}
