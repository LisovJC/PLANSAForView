using Microsoft.Win32;

namespace PLANSA.Services
{
    public class DefaultDialogService
    {
        public static string FilePath { get; set; }

        public static bool OpenFileDialog()
        {
            FilePath = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                return true;
            }
            return false;
        }
    }
}
