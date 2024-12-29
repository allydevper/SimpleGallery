using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using SimpleGallery.Models;
using SimpleGallery.ViewModels;

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

                    // Botón para cambiar imagen anterior
                    var prevButton = new Button
                    {
                        Content = "<",
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                        Margin = new Thickness(20),
                        FontSize = 30
                    };

                    prevButton.Click += (s, e) =>
                    {
                        currentImageIndex = (currentImageIndex - 1 + viewModel.Images.Count) % viewModel.Images.Count;
                        image.Source = new Bitmap(viewModel.Images[currentImageIndex].Path);
                    };

                    // Botón para cambiar imagen siguiente
                    var nextButton = new Button
                    {
                        Content = ">",
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                        Margin = new Thickness(20),
                        FontSize = 30
                    };

                    nextButton.Click += (s, e) =>
                    {
                        currentImageIndex = (currentImageIndex + 1) % viewModel.Images.Count;
                        image.Source = new Bitmap(viewModel.Images[currentImageIndex].Path);
                    };

                    // Manejar el cierre con doble clic
                    image.DoubleTapped += (s, e) => fullScreenWindow.Close();

                    // Agrega los elementos al panel
                    mainPanel.Children.Add(image);
                    mainPanel.Children.Add(prevButton);
                    mainPanel.Children.Add(nextButton);

                    fullScreenWindow.Content = mainPanel;
                    fullScreenWindow.Show();
                }
            }
        }
    }
}