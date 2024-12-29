using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO;

namespace SimpleGallery.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<string> Folders { get; } = [];

        public MainWindowViewModel()
        {
            LoadFolders();
        }

        [RelayCommand]
        private void SelectFolder(string folder)
        {
            SelectedFolder = folder;
        }

        private void LoadFolders()
        {
            Folders.Clear();
            if (Directory.Exists("C:\\Users\\WILMER\\Pictures"))
            {
                foreach (var directory in Directory.GetDirectories("C:\\Users\\WILMER\\Pictures"))
                {
                    string folderName = Path.GetFileName(directory);
                    Folders.Add(folderName);
                }
            }
        }
    }
}