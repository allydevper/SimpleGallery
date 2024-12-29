using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using SimpleGallery.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SimpleGallery.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<FolderFilesModel> Folders { get; } = [];
        public ObservableCollection<FolderFilesModel> Images { get; } = [];

        public MainWindowViewModel()
        {
            LoadFolders();
            LoadImages();
        }

        [RelayCommand]
        private void SelectFolder(FolderFilesModel folder)
        {
            SelectedFolder = folder.Path;
        }

        private void LoadFolders()
        {
            Folders.Clear();
            if (Directory.Exists("C:\\Users\\WILMER\\Pictures"))
            {
                foreach (var directory in Directory.GetDirectories("C:\\Users\\WILMER\\Pictures"))
                {
                    string folderName = Path.GetFileName(directory);

                    FolderFilesModel folderFilesModel = new() { Name = folderName, Path = directory };
                    Folders.Add(folderFilesModel);
                }
            }
        }

        private void LoadImages()
        {
            Images.Clear();
            if (Directory.Exists("C:\\Users\\WILMER\\Pictures"))
            {
                foreach (var file in Directory.GetFiles("C:\\Users\\WILMER\\Pictures", "*.*", SearchOption.TopDirectoryOnly))
                {
                    if (IsImageFile(file))
                    {
                        string folderName = Path.GetFileName(file);
                        FolderFilesModel folderFilesModel = new() { Name = folderName, Path = file };
                        using (var stream = File.OpenRead(file))
                        {
                            var bitmap = new Bitmap(stream);
                            folderFilesModel.ImageSource = bitmap;
                        }

                        Images.Add(folderFilesModel);
                    }
                }
            }
        }

        private bool IsImageFile(string file)
        {
            var extensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            var extension = Path.GetExtension(file)?.ToLower();
            return extensions.Contains(extension);
        }
    }
}