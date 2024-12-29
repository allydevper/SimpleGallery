using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using SimpleGallery.Models;
using SimpleGallery.ViewModels;
using System;

namespace SimpleGallery.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnFolderDoubleTapped(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is FolderFilesModel _folder)
            {
                if (DataContext is MainWindowViewModel viewModel)
                {
                    viewModel.SelectFolderCommand.Execute(_folder);
                }
            }
        }

        private void OnImageDoubleTapped(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is FolderFilesModel _image)
            {
                if (DataContext is MainWindowViewModel viewModel)
                {
                    var currentImageIndex = viewModel.Images.IndexOf(_image);

                    var fullScreenWindow = new Window
                    {
                        WindowState = WindowState.FullScreen,
                        Background = Brushes.Black
                    };

                    var mainPanel = new Grid();
                    var image = new Image
                    {
                        Source = new Bitmap(_image.Path),
                        Stretch = Stretch.Uniform
                    };

                    Point? initialTouchPoint = null;

                    fullScreenWindow.PointerPressed += (s, e) =>
                    {
                        initialTouchPoint = e.GetPosition(fullScreenWindow);
                    };

                    fullScreenWindow.PointerReleased += (s, e) =>
                    {
                        if (initialTouchPoint is null)
                            return;

                        var finalTouchPoint = e.GetPosition(fullScreenWindow);
                        var deltaX = finalTouchPoint.X - initialTouchPoint.Value.X;

                        const double swipeThreshold = 50;

                        if (Math.Abs(deltaX) > swipeThreshold)
                        {
                            if (deltaX > 0)
                            {
                                currentImageIndex = (currentImageIndex - 1 + viewModel.Images.Count) % viewModel.Images.Count;
                            }
                            else
                            {
                                currentImageIndex = (currentImageIndex + 1) % viewModel.Images.Count;
                            }

                            image.Source = new Bitmap(viewModel.Images[currentImageIndex].Path);
                        }
                        else
                        {
                            var clickPosition = finalTouchPoint.X;
                            var screenWidth = fullScreenWindow.Bounds.Width;

                            if (clickPosition > screenWidth / 2)
                            {
                                currentImageIndex = (currentImageIndex + 1) % viewModel.Images.Count;
                            }
                            else
                            {
                                currentImageIndex = (currentImageIndex - 1 + viewModel.Images.Count) % viewModel.Images.Count;
                            }

                            image.Source = new Bitmap(viewModel.Images[currentImageIndex].Path);
                        }

                        initialTouchPoint = null;
                    };

                    fullScreenWindow.DoubleTapped += (s, e) => fullScreenWindow.Close();

                    mainPanel.Children.Add(image);

                    fullScreenWindow.Content = mainPanel;
                    fullScreenWindow.Show();
                }
            }
        }
    }
}