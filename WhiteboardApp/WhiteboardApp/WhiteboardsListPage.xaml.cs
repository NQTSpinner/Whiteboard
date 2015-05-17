using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WhiteboardApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public class util
    {

        
    }


    public sealed partial class WhiteboardsListPage : Page
    {
        ObservableCollection<string> collection;

        public async void getBoardList(string username)
        {
            string URL = "http://107.170.241.204/api/getUserBoards/?user=" + username;
            HttpClient client = new HttpClient();
            string a = await client.GetStringAsync(URL);
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(a);
            List<string> keyList = new List<string>(values.Keys);
            collection = new ObservableCollection<string>(keyList);
        }

        public WhiteboardsListPage()
        {
            this.InitializeComponent();
            getBoardList("test");
        }

        private void WhiteboardsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void CreateWhiteboardButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
