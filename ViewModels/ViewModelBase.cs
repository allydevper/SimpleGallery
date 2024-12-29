using CommunityToolkit.Mvvm.ComponentModel;

namespace SimpleGallery.ViewModels
{
    public partial class ViewModelBase : ObservableObject
    {
        [ObservableProperty] private string _selectedFolder = "";

        protected ViewModelBase()
        {
        }
    }
}