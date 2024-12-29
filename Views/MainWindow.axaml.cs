using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
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

                    // Variables para detectar gestos táctiles
                    Point? initialTouchPoint = null;

                    image.PointerPressed += (s, e) =>
                    {
                        initialTouchPoint = e.GetPosition(image);
                    };

                    image.PointerReleased += (s, e) =>
                    {
                        if (initialTouchPoint is null)
                            return;

                        var finalTouchPoint = e.GetPosition(image);
                        var deltaX = finalTouchPoint.X - initialTouchPoint.Value.X;

                        // Define un umbral para considerar un deslizamiento
                        const double swipeThreshold = 50;

                        if (Math.Abs(deltaX) > swipeThreshold)
                        {
                            if (deltaX > 0)
                            {
                                // Desliza hacia la derecha: imagen anterior
                                currentImageIndex = (currentImageIndex - 1 + viewModel.Images.Count) % viewModel.Images.Count;
                            }
                            else
                            {
                                // Desliza hacia la izquierda: imagen siguiente
                                currentImageIndex = (currentImageIndex + 1) % viewModel.Images.Count;
                            }

                            // Actualiza la imagen mostrada
                            image.Source = new Bitmap(viewModel.Images[currentImageIndex].Path);
                        }

                        initialTouchPoint = null; // Reinicia para el próximo gesto
                    };

                    // Manejar el cierre con doble clic
                    image.DoubleTapped += (s, e) => fullScreenWindow.Close();

                    // Agrega el elemento de imagen al panel
                    mainPanel.Children.Add(image);

                    fullScreenWindow.Content = mainPanel;
                    fullScreenWindow.Show();

                }
            }
        }
    }
}