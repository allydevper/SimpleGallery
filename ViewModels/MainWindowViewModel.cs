using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using SimpleGallery.Enum;
using SimpleGallery.Models;
using SimpleGallery.Services;
using SimpleGallery.Utils;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
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
            if (!string.IsNullOrEmpty(localPath))
            {
                UpdateFolderImage(localPath);
            }
        }

        [RelayCommand]
        private void SelectFolder(FolderFilesModel folder)
        {
            UpdateFolderImage(folder.Path);
        }

        [RelayCommand]
        private async Task OpenFolder()
        {
            try
            {
                var folder = await FilePickerService.SaveFolderAsync();
                if (folder is null) return;

                string path = Uri.UnescapeDataString(folder.Path.AbsolutePath);
                UpdateFolderImage(path);
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard(MessageType.Error.ToString(), ex.Message).ShowAsync();
            }
        }

        [RelayCommand]
        private async Task Back()
        {
            try
            {
                string? path = Path.GetDirectoryName(SelectedFolder);
                if (!string.IsNullOrEmpty(path))
                {
                    UpdateFolderImage(path);
                };
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard(MessageType.Error.ToString(), ex.Message).ShowAsync();
            }
        }

        private void UpdateFolderImage(string path)
        {
            SelectedFolder = path;
            LoadFolders();
            LoadImagesAsync();
        }

        private void LoadFolders()
        {
            Folders.Clear();
            if (Directory.Exists(SelectedFolder))
            {
                foreach (var directory in Directory.GetDirectories(SelectedFolder).OrderBy(folder => folder, new NaturalStringComparer()))
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
            if (Directory.Exists(SelectedFolder))
            {
                foreach (var file in Directory.GetFiles(SelectedFolder, "*.*", SearchOption.TopDirectoryOnly).OrderBy(file => file, new NaturalStringComparer()))
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