using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGallery.Models
{
    public class FolderFilesModel
    {
        public required string Name { get; set; }
        public required string Path { get; set; }
        public Bitmap? ImageSource { get; set; }
    }
}
