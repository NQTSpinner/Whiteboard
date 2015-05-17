using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.MobileServices;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WhiteboardApp
{
    public class ChatMessage
    {
        public string Message { get; set; }
        public string User { get; set; }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Variables
        bool eraserModeToggle = false;
        bool fingerModeToggle = false;
        bool isDouble = false;
        string myuser = UserVariables.UserName;
        bool colourClicked = false;
        bool sizeClicked = false;
        public static ObservableCollection<string> ParticipantsCollection = new ObservableCollection<string>();
        public static ObservableCollection<ChatMessage> ChatCollection = new ObservableCollection<ChatMessage>();
        public static MobileServiceClient ServiceClient;

        DispatcherTimer dispatchTimer = new DispatcherTimer();

        DispatcherTimer InkUpdateTimer = new DispatcherTimer();
        //public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors
        public MainPage()
        {
            ChatCollection = new ObservableCollection<ChatMessage>();
            ChatCollection.Add(new ChatMessage { Message = "hey!", User = "Carmen" });
            ChatCollection.Add(new ChatMessage { Message = "what's up", User = "Thai" });
            ChatCollection.Add(new ChatMessage { Message = "yoo", User = "Adrian" });
            ParticipantsCollection = new ObservableCollection<string>();
            ParticipantsCollection.Add(" Carmen");
            ParticipantsCollection.Add(" Thai");
            ParticipantsCollection.Add(" Adrian");

            this.InitializeComponent();

            UserName.Text = UserVariables.UserName;

            this.ParticipantsListBox.ItemsSource = ParticipantsCollection;
            this.ChatListBox.ItemsSource = ChatCollection;

            dispatchTimer.Interval = new TimeSpan(0, 0, 3);
            dispatchTimer.Tick += Load_Strokes;
            dispatchTimer.Start();

            InkUpdateTimer.Interval = new TimeSpan(0, 0, 1);
            InkUpdateTimer.Tick += Update_Strokes;

            //ParticipantsCollection.CollectionChanged += new PropertyChangedEventHandler(RaisePropertyChanged);
            InkCanvas.InkPresenter.StrokesCollected += Save_Strokes;
            InkCanvas.InkPresenter.StrokesErased += Erase_Strokes;
            InkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Pen;
        }
        #endregion

        #region Page Events Handlers
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InkCanvas.Height = 1920;
            InkCanvas.Width = 1080;
            await Download_Strokes();
        }
        #endregion

        #region Inkcanvas and Timers Events Handlers
        private async void Update_Strokes(object sender, object e)
        {
            await Upload_Strokes();
        }

        private void Erase_Strokes(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
            dispatchTimer.Stop();
            dispatchTimer.Start();

            if (InkUpdateTimer.IsEnabled)
            {
                InkUpdateTimer.Stop();
                InkUpdateTimer.Start();
            }
            else
            {
                InkUpdateTimer.Start();
            }
        }

        private async void Save_Strokes(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            dispatchTimer.Stop();
            dispatchTimer.Start();

            if (InkUpdateTimer.IsEnabled)
            {
                InkUpdateTimer.Stop();
                InkUpdateTimer.Start();
            }
            else
            {
                InkUpdateTimer.Start();
            }
        }

        private async Task Download_Strokes()
        {
            string URL = "http://whiteboard01.blob.core.windows.net/test/" + UserVariables.CurrentBoard;
            HttpClient httpClient = new HttpClient();
            Stream streamAsync = await httpClient.GetStreamAsync(URL);

            if (streamAsync != null)
            {
                Windows.Storage.Streams.IInputStream inStream = streamAsync.AsInputStream();

                using (inStream)
                {
                    try
                    {
                        InkCanvas.InkPresenter.StrokeContainer.LoadAsync(inStream);
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
        }

        private async void Load_Strokes(object sender, object e)
        {
            await Download_Strokes();
        }

        #endregion

        #region Buttons Events Handler
        private void BlueButton_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 0, 0, 255);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            SetColourIcon("Images/blue.png");
        }

        private void GreenButton_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 0, 255, 0);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            SetColourIcon("Images/green.png");
        }

        private void RedButton_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 255, 0, 0);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            SetColourIcon("Images/red.png");
        }

        private void PurpleButton_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 138, 43, 236); // TO DOOO!!!
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            SetColourIcon("Images/purple.png");
        }

        private void OrangeButton_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 255, 140, 0); // TO DOOO!!!!
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            SetColourIcon("Images/orange.png");
        }

        private void YellowButton_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 255, 255, 0); //TO DO !!!!
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            SetColourIcon("Images/yellow.png");
        }

        private void BlackButton_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 0, 0, 0);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            SetColourIcon("Images/black.png");
        }

        private void SetColourIcon(string path)
        {
            colourClicked = false;
            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("ms-appx:/" + path));
            this.ColourButton.Background = brush;
        }

        private void Thin_Stroke_Button_Click(object sender, RoutedEventArgs e)
        {
            sizeClicked = false;
            CloseOtherPanels("");
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Size = new Size(5, 5);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void Medium_Stroke_Button_Click(object sender, RoutedEventArgs e)
        {
            sizeClicked = false;
            CloseOtherPanels("");
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Size = new Size(10, 10);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void Thick_Stroke_Button_Click(object sender, RoutedEventArgs e)
        {
            sizeClicked = false;
            CloseOtherPanels("");
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Size = new Size(20, 20);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void EraserButton_Click(object sender, RoutedEventArgs e)
        {
            if (!eraserModeToggle) {
                CloseOtherPanels("");
                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("ms-appx:/Images/pen.png"));
                this.EraserButton.Background = brush;
                InkCanvas.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Erasing;
                eraserModeToggle = true;
            }
            else
            {
                CloseOtherPanels("");
                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("ms-appx:/Images/eraser.png"));
                this.EraserButton.Background = brush;
                InkCanvas.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Inking;
                eraserModeToggle = false;
            }
        }

        private void FingerDrawButton_Click(object sender, RoutedEventArgs e)
        {
            if (!fingerModeToggle)
            {
                CloseOtherPanels("");
                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("ms-appx:/Images/fingerselect.png"));
                this.FingerDrawButton.Background = brush;
                InkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Touch | Windows.UI.Core.CoreInputDeviceTypes.Pen;
                fingerModeToggle = true;
            }
            else
            {
                CloseOtherPanels("");
                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("ms-appx:/Images/finger.png"));
                this.FingerDrawButton.Background = brush;
                InkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Pen;
                fingerModeToggle = false;
            }
        }

        private void OpenMenuButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SplitView.IsPaneOpen = true;
            this.ParticipantsListBox.Visibility = Visibility.Collapsed;
        }

        private void OpenColourButton_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("Colour");
            if (colourClicked)
            {
                this.ColourPanel.Visibility = Visibility.Collapsed;
                colourClicked = false;
            }
            else
            {
                this.ColourPanel.Visibility = Visibility.Visible;
                colourClicked = true;
            }
        }

        private void SizeButton_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("Size");
            if (sizeClicked)
            {
                this.SizePanel.Visibility = Visibility.Collapsed;
                sizeClicked = false;
            }
            else
            {
                this.SizePanel.Visibility = Visibility.Visible;
                sizeClicked = true;
            }
        }

        private void CloseOtherPanels(string currentPanel)
        {
            if (currentPanel != "Size") this.SizePanel.Visibility = Visibility.Collapsed;
            if (currentPanel != "Colour") this.ColourPanel.Visibility = Visibility.Collapsed;
        }

        private void SquareButton_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
        }

        private async void CircleButton_Click(object sender, RoutedEventArgs e)
        {
            await Upload_Strokes();
        }



        private void TextButton_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
        }

        private void SignOutButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(LoginPage));
            }
        }

        private void ParticipantsButton_Tapped(object sender, RoutedEventArgs e)
        {
            if (ParticipantsListBox.Visibility.Equals(Visibility.Visible))
            {
                ParticipantsListBox.Visibility = Visibility.Collapsed;
            }
            else //http://www.softwareandfinance.com/VSNET_40/ListBoxBinding.html
            {
                ParticipantsListBox.Visibility = Visibility.Visible;
            }
        }

        private void ColourButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CloseOtherPanels("Colour");
            this.ColourPanel.Visibility = Visibility.Visible;
        }

        private void ColourPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!colourClicked)
            CloseOtherPanels("");
        }

        private void SizeButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CloseOtherPanels("Size");
            this.SizePanel.Visibility = Visibility.Visible;
        }

        private void SizePanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!sizeClicked)
            CloseOtherPanels("");
        }

        private async void PushNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            IMobileServiceTable<Notifications> messageTable = ServiceClient.GetTable<Notifications>();
            await messageTable.InsertAsync(new Notifications() { Text = "Notification button clicked" });
        }

        private void ChatTab_Click(object sender, RoutedEventArgs e)
        {
            if (this.HiddenChatPanel.Visibility == Visibility.Visible)
            {
                this.HiddenChatPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.HiddenChatPanel.Visibility = Visibility.Visible;
                ChatTextBox.Focus(FocusState.Pointer);
            }
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            ChatCollection.Add(new ChatMessage { User = myuser, Message = ChatTextBox.Text });
            ChatListBox.ItemsSource = ChatCollection;
            ChatTextBox.Text = "";
            ChatTextBox.Focus(FocusState.Pointer);
        }

        private void ChatTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && !isDouble)
            {
                ChatCollection.Add(new ChatMessage { User = myuser, Message = ChatTextBox.Text });
                ChatListBox.ItemsSource = ChatCollection;
                ChatTextBox.Text = "";
                isDouble = true;
                ChatTextBox.Focus(FocusState.Pointer);
            }
            else
            {
                isDouble = false;
            }
        }

        private void WhiteboardsButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(WhiteboardsListPage));
            }
        }

        #endregion

        #region Helper Functions
        private async Task Upload_Strokes()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=whiteboard01;AccountKey=z0rUBRkcznOHooMWbQ/lTJrytth6PgYsnnnTzH++AaLVd3tqe8nyinwebXW4OKfrVNfjt3726zlI6DZlQiXkoQ==");

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("test");

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(UserVariables.CurrentBoard);
            
            var file = ApplicationData.Current.RoamingFolder.CreateFileAsync("ink.isf", CreationCollisionOption.OpenIfExists);
            var openedFile = await file.AsTask();
            if (openedFile != null)
            {
                try
                {
                    using (IRandomAccessStream stream = await openedFile.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        await InkCanvas.InkPresenter.StrokeContainer.SaveAsync(stream);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                blockBlob.UploadFromFileAsync(openedFile);
            }
        }
        #endregion
        
        private void InviteFriend_GotFocus(object sender, RoutedEventArgs e)
        {
            InviteFriend.Text = InviteFriend.Text == "User or Email" ? string.Empty : InviteFriend.Text;
        }

        private void InviteFriend_LostFocus(object sender, RoutedEventArgs e)
        {
            InviteFriend.Text = InviteFriend.Text == string.Empty ? "User or Email" : InviteFriend.Text;
        }

        private void FriendsButton_Click(object sender, RoutedEventArgs e)
        {
            if (InviteFriendPanel.Visibility == Visibility.Visible)
            {
                InviteFriendPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                InviteFriendPanel.Visibility = Visibility.Visible;
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            InviteFriend.Text = "User or Email";
            InviteFriendPanel.Visibility = Visibility.Collapsed;
        }

        private void InsertImageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExtendToolbar_Click(object sender, RoutedEventArgs e)
        {
            if (ExtendedToolbarPanel.Visibility == Visibility.Visible) //compress it
            {
                ExtendedToolbarPanel.Visibility = Visibility.Collapsed;
                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("ms-appx:/Images/extendToolbar.png"));
                this.ExtendToolbar.Background = brush;
            }
            else
            {
                var brush = new ImageBrush();
                brush.ImageSource = new BitmapImage(new Uri("ms-appx:/Images/compressToolbar.png"));
                this.ExtendToolbar.Background = brush;
                ExtendedToolbarPanel.Visibility = Visibility.Visible;
            }
        }
    }

    internal class Notifications
    {
        public Notifications()
        {
        }

        public string Text { get; set; }
        public string id { get; set; }
    }


}
