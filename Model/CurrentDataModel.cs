using PLANSA.Core;

namespace PLANSA.Model
{
    internal class CurrentDataModel : ObservableObject
    {
        private int _infoIndex;

        public int InfoIndex
        {
            get => _infoIndex;
            set => Set(ref _infoIndex, value);
        }

        private int _editIndex;

        public int EditIndex
        {
            get => _editIndex;
            set => Set(ref _editIndex, value);
        }

    }
}
