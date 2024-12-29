using Avalonia.Controls;
using Avalonia.Interactivity;
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
            if (sender is Button button && button.DataContext is var folder)
            {
                if (DataContext is MainWindowViewModel viewModel)
                {
                    viewModel.SelectFolderCommand.Execute(folder);
                }
            }
        }
    }
}