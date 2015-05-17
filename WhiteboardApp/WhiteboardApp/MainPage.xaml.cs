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
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WhiteboardApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static ObservableCollection<string> ParticipantsCollection = new ObservableCollection<string>();

        DispatcherTimer dispatchTimer;

        //public event PropertyChangedEventHandler PropertyChanged;
        

        public MainPage()
        {
            ParticipantsCollection = new ObservableCollection<string>();
            ParticipantsCollection.Add(" Ann");
            ParticipantsCollection.Add(" Marie");
            ParticipantsCollection.Add(" Rem");
            this.InitializeComponent();

            this.ParticipantsListBox.ItemsSource = ParticipantsCollection;
            //dispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            //dispatchTimer.Tick += Update_Canvas;
            //dispatchTimer.Start();
            //ParticipantsCollection.CollectionChanged += new PropertyChangedEventHandler(RaisePropertyChanged);
            InkCanvas.InkPresenter.StrokesCollected += Save_Strokes;
            InkCanvas.InkPresenter.StrokesErased += Erase_Strokes;
            InkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Pen;
        }

        private async Task Upload_Strokes()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=whiteboard01;AccountKey=z0rUBRkcznOHooMWbQ/lTJrytth6PgYsnnnTzH++AaLVd3tqe8nyinwebXW4OKfrVNfjt3726zlI6DZlQiXkoQ==");

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("test");

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference("newfilehah");

            // Create or overwrite the "myblob" blob with contents from a local file.
            try
            {
                await blockBlob.DeleteAsync();
            }
            catch
            {

            }

            var file = ApplicationData.Current.RoamingFolder.CreateFileAsync("ink.isf", CreationCollisionOption.OpenIfExists);
            var openedFile = await file.AsTask();
            if (openedFile != null)
            {
                try
                {
                    blockBlob.DeleteAsync();
                    using (IRandomAccessStream stream = await openedFile.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        await InkCanvas.InkPresenter.StrokeContainer.SaveAsync(stream);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                await blockBlob.UploadFromFileAsync(openedFile);
            }
        }

        private async void Erase_Strokes(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
            await Upload_Strokes();
        }

        private async void Save_Strokes(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            await Upload_Strokes();
        }

        private async void Load_Strokes(object sender, object e)
        {
            string URL = "http://whiteboard01.blob.core.windows.net/test/test.isf";
            HttpClient httpClient = new HttpClient();
            Task<Stream> streamAsync = httpClient.GetStreamAsync(URL);
            Stream result = streamAsync.Result;
            Windows.Storage.Streams.IInputStream inStream = result.AsInputStream();
            
            using (inStream)
            {
                try
                {
                    await InkCanvas.InkPresenter.StrokeContainer.LoadAsync(inStream);
                }
                catch(Exception ex)
                {

                }
                
            }
        }

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
            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("ms-appx:/" + path));
            this.ColourButton.Background = brush;
        }

        private void Thin_Stroke_Button_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Size = new Size(5, 5);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void Medium_Stroke_Button_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Size = new Size(10, 10);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void Thick_Stroke_Button_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Size = new Size(20, 20);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void EraserButton_Checked(object sender, RoutedEventArgs e)
        {
            InkCanvas.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Erasing;
        }

        private void EraserButton_Unchecked(object sender, RoutedEventArgs e)
        {
            InkCanvas.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Inking;
        }

        private void OpenMenuButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.SplitView.IsPaneOpen = true;
        }

        private void OpenColourButton_Tapped(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("Colour");
            if (this.ColourPanel.Visibility.Equals(Visibility.Visible))
            {
                this.ColourPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.ColourPanel.Visibility = Visibility.Visible;
            }
        }

        private void SizeButton_Click(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("Size");
            if (this.SizePanel.Visibility.Equals(Visibility.Visible))
            {
                this.SizePanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.SizePanel.Visibility = Visibility.Visible;
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

        private void FingerDrawButton_Checked(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
            InkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Touch | Windows.UI.Core.CoreInputDeviceTypes.Pen;
        }

        private void FingerDrawButton_Unchecked(object sender, RoutedEventArgs e)
        {
            CloseOtherPanels("");
            InkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Pen;
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
            if (this.ParticipantsListBox.Visibility.Equals(Visibility.Visible))
            {
                this.ParticipantsListBox.Visibility = Visibility.Collapsed;
            }
            else //http://www.softwareandfinance.com/VSNET_40/ListBoxBinding.html
            {
                this.ParticipantsListBox.Visibility = Visibility.Visible;
            }
        }
    }
}
