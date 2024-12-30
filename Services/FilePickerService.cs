using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using System;
using System.Threading.Tasks;

namespace SimpleGallery.Services
{
    public class FilePickerService
    {
        public static async Task<IStorageFolder?> SaveFolderAsync()
        {
            if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop ||
                desktop.MainWindow?.StorageProvider is not { } _storageProvider)
                throw new NullReferenceException("Missing StorageProvider instance.");

            var folders = await _storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                Title = "Seleccione carpeta para comenzar a leer",
                AllowMultiple = false
            });

            if (folders == null || folders.Count == 0)
                return null;

            return folders[0];
        }
    }
}