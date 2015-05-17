﻿using System;
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
using Windows.UI.Xaml.Media.Imaging;
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
            InkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Pen;
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

        private void BlueButton_Click(object sender, RoutedEventArgs e)
        {
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 0, 0, 255);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            SetColourIcon("Images/blue.png");
        }

        private void GreenButton_Click(object sender, RoutedEventArgs e)
        {
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 0, 255, 0);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            SetColourIcon("Images/green.png");
        }

        private void RedButton_Click(object sender, RoutedEventArgs e)
        {
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 255, 0, 0);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            SetColourIcon("Images/red.png");
        }

        private void PurpleButton_Click(object sender, RoutedEventArgs e)
        {
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 138, 43, 236); // TO DOOO!!!
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            SetColourIcon("Images/purple.png");
        }

        private void OrangeButton_Click(object sender, RoutedEventArgs e)
        {
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 255, 140, 0); // TO DOOO!!!!
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            SetColourIcon("Images/orange.png");
        }

        private void YellowButton_Click(object sender, RoutedEventArgs e)
        {
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Color = Windows.UI.Color.FromArgb(0, 255, 255, 0); //TO DO !!!!
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            SetColourIcon("Images/yellow.png");
        }

        private void BlackButton_Click(object sender, RoutedEventArgs e)
        {
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
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Size = new Size(5, 5);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void Medium_Stroke_Button_Click(object sender, RoutedEventArgs e)
        {
            InkDrawingAttributes drawingAttributes = InkCanvas.InkPresenter.CopyDefaultDrawingAttributes();
            drawingAttributes.Size = new Size(10, 10);
            InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
        }

        private void Thick_Stroke_Button_Click(object sender, RoutedEventArgs e)
        {
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

        }

        private void CircleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FingerDrawButton_Checked(object sender, RoutedEventArgs e)
        {
            InkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Touch | Windows.UI.Core.CoreInputDeviceTypes.Pen;
        }

        private void FingerDrawButton_Unchecked(object sender, RoutedEventArgs e)
        {
            InkCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Pen;
        }

        private void SignOutButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(LoginPage));
            }
        }
    }
}
