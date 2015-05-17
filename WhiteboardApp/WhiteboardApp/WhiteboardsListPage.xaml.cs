using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

    public sealed partial class WhiteboardsListPage : Page
    {
        ObservableCollection<string> collection;
        string result;
        public async void getBoardList()
        {
            string URL = "http://107.170.241.204/api/getUserBoards/?user=" + UserVariables.UserName;
            HttpClient client = new HttpClient();
            string a = await client.GetStringAsync(URL);
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(a);
            List<string> keyList = new List<string>(values.Keys);
            collection = new ObservableCollection<string>(keyList);
            WhiteboardsListBox.ItemsSource = collection;
        }

        public WhiteboardsListPage()
        {
            this.InitializeComponent();
            getBoardList();
        }

        private void PageLoaded(object sender, object e)
        {
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.None;
        }

        private void WhiteboardsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserVariables.CurrentBoard = (string)(((ListBox)sender).SelectedValue);
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private async void CreateWhiteboardButton_Click(object sender, RoutedEventArgs e)
        {
            string URL = "http://107.170.241.204/api/CreateBoard/?user=" + UserVariables.UserName;
            HttpClient client = new HttpClient();
            string a = await client.GetStringAsync(URL);
            Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(a);
            UserVariables.CurrentBoard = values["boardId"];

            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
