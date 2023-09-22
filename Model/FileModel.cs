using PLANSA.Core;

namespace PLANSA.Model
{
    internal class FileModel : ObservableObject
    {
        private string _filesContent;

        public string FilesContent
        {
            get => _filesContent;
            set => Set(ref _filesContent, value);
        }

        private string _fileName;

        public string FileName
        {
            get => _fileName;
            set => Set(ref _fileName, value);
        }

        private string _iconKind;

        public string IconKind
        {
            get => _iconKind;
            set => Set(ref _iconKind, value);
        }
    }
}
