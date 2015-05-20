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
using Windows.Graphics.Display;
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
        bool isDouble = false;
        string password;
        string defaultText = "Username or email";
        private string URL = "104.236.134.122/api/authenticate?user=[username]&pass=[pass]";
        public LoginPage()
        {
            this.InitializeComponent();
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            URL = "http://104.236.134.122/api/authenticate?user=" + UsernameBox.Text + "&pass=" + PasswordBoxHidden.Password;
            HttpClient client = new HttpClient();
            string a = await client.GetStringAsync(URL);
            a = a.Substring(18, a.Length - 18 - 1);
            if (String.Compare(a, "true") == 0)
            {
                if (this.Frame != null)
                {
                        UserVariables.UserName = UsernameBox.Text;
                        this.Frame.Navigate(typeof(WhiteboardsListPage));
                }
            }
            else if (String.Compare(a, "\"User Doesnt Exist\"") == 0)
            {
                URL = "104.236.134.122/api/createAccount?user=" + UsernameBox.Text + "&pass=" + PasswordBoxHidden.Password;
                HttpClient clientB = new HttpClient();
                string b = await client.GetStringAsync(URL);
                b = b.Substring(14, b.Length - 14 - 1);
                if (String.Compare(b, "true") == 0) { 
                    LoginMessageBlock.Text = "User account created, please sign in.";
                }
                else
                {
                    LoginMessageBlock.Text = "Unable to create new account.";
                }
            }
            else {
                LoginMessageBlock.Text = "Incorrect password";
            }
        }

        private void UsernameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            UsernameBox.Text = UsernameBox.Text == defaultText ? string.Empty : UsernameBox.Text;
        }

        private void UsernameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UsernameBox.Text = UsernameBox.Text == string.Empty ? defaultText : UsernameBox.Text;
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBoxHidden.Visibility = Visibility.Visible;
            PasswordBox.Visibility = Visibility.Collapsed;
            this.PasswordBoxHidden.Focus(FocusState.Pointer);
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox.Text = PasswordBox.Text == string.Empty ? "Password" : PasswordBox.Text;
        }

        private void PasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void PasswordBoxHidden_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.Empty == PasswordBoxHidden.Password) {
                PasswordBoxHidden.Visibility = Visibility.Collapsed;
                PasswordBox.Text = "Password";
                PasswordBox.Visibility = Visibility.Visible;
            }
        }

        private async void PasswordBoxHidden_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && !isDouble)
            {
                URL = "http://104.236.134.122/api/authenticate?user=" + UsernameBox.Text + "&pass=" + PasswordBoxHidden.Password;
                HttpClient client = new HttpClient();
                string a = await client.GetStringAsync(URL);
                a = a.Substring(18, a.Length - 18 - 1);
                if (String.Compare(a, "true") == 0)
                {
                    if (this.Frame != null)
                    {
                        UserVariables.UserName = UsernameBox.Text;
                        this.Frame.Navigate(typeof(WhiteboardsListPage));
                    }
                }
                else if (String.Compare(a, "\"User Doesnt Exist\"") == 0)
                {
                    URL = "http://104.236.134.122/api/createAccount?user=" + UsernameBox.Text + "&pass=" + PasswordBoxHidden.Password;
                    HttpClient clientB = new HttpClient();
                    string b = await client.GetStringAsync(URL);
                    b = b.Substring(14, b.Length - 14 - 1);
                    if (String.Compare(b, "true") == 0)
                    {
                        LoginMessageBlock.Text = "User account created, please sign in.";
                    }
                    else
                    {
                        LoginMessageBlock.Text = "Unable to create new account.";
                    }
                }
                else
                {
                    LoginMessageBlock.Text = "Incorrect password";
                }
                isDouble = true;
            }
            else
            {
                isDouble = false;
            }
        }
    
    }
}
