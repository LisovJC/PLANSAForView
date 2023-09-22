using PLANSA.Command;
using System.Windows;
using System.Windows.Controls;
using PLANSA.Core;
using PLANSA.View.Pages;
using PLANSA.ViewModel.Pages;

namespace PLANSA.ViewModel.Windows
{
    internal class MainViewModel : ObservableObject
    {
        #region HotCommands
        public RelayCommand? CloseAppCommand { get; set; }
        public RelayCommand? MaximizeCommand { get; set; }
        public RelayCommand? MinimizeCommand { get; set; }
        #endregion

        #region NavigationsCommand
        public RelayCommand? OpenMainPageCommand { get; set; }
        public RelayCommand? OpenCreatePageCommand { get; set; }
        public RelayCommand? OpenReviewPageCommand { get; set; }
        public RelayCommand? OpenEditPageCommand { get; set; }
        public RelayCommand? OpenWarningCommand { get; set; }
        public RelayCommand? CancelCommand { get; set; }
        #endregion

        #region Page
        private Page _choicePage;
        public Page ChoicePage
        {
            get => _choicePage;
            set => Set(ref _choicePage, value);
        }
        #endregion

        #region VisibleProperties
        private Visibility _warningVisibility = Visibility.Hidden;

        public Visibility WarningVisibility
        {
            get => _warningVisibility;
            set => Set(ref _warningVisibility, value);
        }

        private int _blurRadius = 0;

        public int BlurRadius
        {
            get => _blurRadius;
            set => Set(ref _blurRadius, value);
        }

        private string _textWarning = "Вы собираетесь войти в режим редактора.\nЗдесь вы сможете изменять любые параметры плана.\nПосле этого все изменения будет необходимо сохранить!\nВы уверены, что желаете перейти в этот режим?";

        public string TextWarning
        {
            get => _textWarning = "Вы собираетесь войти в режим редактора.\nЗдесь вы сможете изменять любые параметры плана.\nПосле этого все изменения будет необходимо сохранить!\nВы уверены, что желаете перейти в этот режим?";
            set => Set(ref _textWarning, value);
        }



        #endregion

        public MainViewModel()
        {                     
            #region Starting
            StartApp();
            #endregion

            #region HotCommands
            CloseAppCommand = new RelayCommand(o =>
            {
                Application.Current.Shutdown();
            });

            MaximizeCommand = new RelayCommand(o =>
            {
                if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
                {
                    Application.Current.MainWindow.WindowState = WindowState.Normal;
                }
                else
                {
                    Application.Current.MainWindow.WindowState = WindowState.Maximized;
                }
            });

            MinimizeCommand = new RelayCommand(o =>
            {               
                    Application.Current.MainWindow.WindowState = WindowState.Minimized;               
            });
            #endregion

            #region NavigationsCommand
            OpenMainPageCommand = new (o =>
            {
                ChoicePage = new MainPage();
            });

            OpenCreatePageCommand = new (o =>
            {
                ChoicePage = new CreatePage();
            });

            OpenReviewPageCommand = new(o =>
            {                
                ChoicePage = new ReviewPage();
            });

            OpenEditPageCommand = new(o =>
            {
                WarningVisibility = Visibility.Hidden;
                BlurRadius = 0;
                ChoicePage = new EditPage();
            });

            OpenWarningCommand = new(o =>
            {
                WarningVisibility = Visibility.Visible;
                BlurRadius = 30;
            });

            CancelCommand = new(o =>
            {
                WarningVisibility = Visibility.Hidden;
                BlurRadius = 0;
            });
            #endregion           
        }

        #region Methods
        #region Load&StartOperations
        private void StartApp()
        {
            #region CurrentPage
            ChoicePage = new MainPage();
            #endregion
        }
        #endregion
        #endregion
    }
}
