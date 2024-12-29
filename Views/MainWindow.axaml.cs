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
            if (sender is Button button && button.DataContext is FolderFilesModel folder)
            {
                if (DataContext is MainWindowViewModel viewModel)
                {
                    viewModel.SelectFolderCommand.Execute(folder);
                }
            }
        }

        private void OnImageDoubleTapped(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is FolderFilesModel image)
            {
                if (DataContext is MainWindowViewModel viewModel)
                {
                    var fullScreenWindow = new Window
                    {
                        WindowState = WindowState.FullScreen,
                        Background = Brushes.Black,
                        Content = new Image
                        {
                            Source = new Bitmap(image.Path),
                            Stretch = Stretch.Uniform
                        }
                    };

                    fullScreenWindow.KeyDown += (s, e) =>
                    {
                        if (e.Key == Key.Escape) fullScreenWindow.Close();
                    };

                    fullScreenWindow.PointerPressed += (s, e) => fullScreenWindow.Close();
                    fullScreenWindow.Show();
                }
            }
        }
    }
}