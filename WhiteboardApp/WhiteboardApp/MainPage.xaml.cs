using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WhiteboardApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer dispatchTimer;
        public MainPage()
        {
            this.InitializeComponent();
            //dispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            //dispatchTimer.Tick += Update_Canvas;
            //dispatchTimer.Start();

            InkCanvas.InkPresenter.StrokesCollected += Save_Strokes;
            InkCanvas.InkPresenter.StrokesErased += Erase_Strokes;

        }

        private async void Erase_Strokes(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
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

                }
            }
        }

        private async void Save_Strokes(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
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

                }
            }
        }

        private async void Load_Strokes(object sender, object e)
        {
            var file = ApplicationData.Current.RoamingFolder.CreateFileAsync("ink.isf", CreationCollisionOption.OpenIfExists);
            var openedFile = await file.AsTask();
            if (openedFile != null)
            {
                using (var stream = await openedFile.OpenSequentialReadAsync())
                {
                    try
                    {
                        await InkCanvas.InkPresenter.StrokeContainer.LoadAsync(stream);
                    }
                    catch(Exception ex)
                    {
                        
                    }
                }
            }
        }

        private void Blue_Button_Click(object sender, RoutedEventArgs e)
        {
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 0, 0, 255);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void Green_Button_Click(object sender, RoutedEventArgs e)
        {
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 0, 255, 0);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void Red_Button_Click(object sender, RoutedEventArgs e)
        {
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 255, 0, 0);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void Small_Button_Click(object sender, RoutedEventArgs e)
        {
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Size = new Size(5, 5);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void Medium_Button_Click(object sender, RoutedEventArgs e)
        {
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Size = new Size(10, 10);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void Large_Button_Click(object sender, RoutedEventArgs e)
        {
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Size = new Size(20, 20);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            InkCanvas.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Erasing;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            InkCanvas.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Inking;
        }
    }
}
