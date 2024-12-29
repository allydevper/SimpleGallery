using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using SimpleGallery.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleGallery.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string localPath = "C:\\Users\\WILMER\\Pictures";
        public ObservableCollection<FolderFilesModel> Folders { get; } = [];
        public ObservableCollection<FolderFilesModel> Images { get; } = [];

        public MainWindowViewModel()
        {
            LoadFolders();
            LoadImagesAsync();
        }

        [RelayCommand]
        private void SelectFolder(FolderFilesModel folder)
        {
            SelectedFolder = folder.Path;
        }

        private void LoadFolders()
        {
            Folders.Clear();
            if (Directory.Exists(localPath))
            {
                foreach (var directory in Directory.GetDirectories(localPath))
                {
                    string folderName = Path.GetFileName(directory);

                    FolderFilesModel folderFilesModel = new() { Name = folderName, Path = directory };
                    Folders.Add(folderFilesModel);
                }
            }
        }

        private async void LoadImagesAsync()
        {
            Images.Clear();
            if (Directory.Exists(localPath))
            {
                foreach (var file in Directory.GetFiles(localPath, "*.*", SearchOption.TopDirectoryOnly))
                {
                    if (IsImageFile(file))
                    {
                        string folderName = Path.GetFileName(file);
                        FolderFilesModel folderFilesModel = new()
                        {
                            Name = folderName,
                            Path = file,
                            ImageSource = await ReduceImageSizeAsync(file)
                        };

                        Images.Add(folderFilesModel);
                    }
                }
            }
        }

        private static async Task<Bitmap> ReduceImageSizeAsync(string inputImagePath)
        {
            using var image = await Image.LoadAsync(inputImagePath);
            int newWidth = image.Width / 5;
            int newHeight = image.Height / 5;

            image.Mutate(x => x.Resize(newWidth, newHeight));

            using var ms = new MemoryStream();
            await image.SaveAsPngAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);

            return new Bitmap(ms);
        }

        private static bool IsImageFile(string file)
        {
            var extensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var extension = Path.GetExtension(file)?.ToLower();
            return extensions.Contains(extension);
        }
    }
}