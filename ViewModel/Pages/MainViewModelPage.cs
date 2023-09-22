using PLANSA.Command;
using PLANSA.Core;
using PLANSA.Model;
using PLANSA.Services;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Threading;
using System.Threading.Tasks;

namespace PLANSA.ViewModel.Pages
{
    internal class MainViewModelPage : ObservableObject
    {
        #region NecessaryProperties
        public static MainViewModelPage? Instance { get; set; }

        private int _selectedPlanIndex;

        public int SelectedPlanIndex
        {
            get => _selectedPlanIndex;
            set => Set(ref _selectedPlanIndex, value);
        }       

        private int _selectedIndexAdvancedPlans;

        public int SelectedIndexAdvancedPlans
        {
            get => _selectedIndexAdvancedPlans;
            set => Set(ref _selectedIndexAdvancedPlans, value);
        }

        private int _selectedIndexFiles;

        public int SelectedIndexFiles
        {
            get => _selectedIndexFiles;
            set => Set(ref _selectedIndexFiles, value);
        }

        public int CurrentIndex { get; set; }
        #endregion

        #region Collections
        public static ObservableCollectionEX<PlanModel>? Plans { get; set; }
        public static ObservableCollectionEX<CurrentDataModel>? CurrentDatas { get; set; }
        public ObservableCollection<FileModel>? Files { get; set; }
        public static ObservableCollection<AdvancedPlanModel>? AdvancedPlans { get; set; }
        #endregion

        #region TextProperties
        private string? _planHeader;

        public string? PlanHeader
        {
            get => _planHeader;
            set => Set(ref _planHeader, value);
        }

        private string? _deadLine;

        public string? DeadLine
        {
            get => _deadLine;
            set => Set(ref _deadLine, value);
        }

        private string? _planContent;

        public string? PlanContent
        {
            get => _planContent;
            set => Set(ref _planContent, value);
        }

        private string? _advancedPlanContent;

        public string? AdvancedPlanContent
        {
            get => _advancedPlanContent;
            set => Set(ref _advancedPlanContent, value);
        }

        private string? _hoursRemained;

        public string? HoursRemained
        {
            get => _hoursRemained;
            set => Set(ref _hoursRemained, value);
        }

        private string? _daysRemained;

        public string? DaysRemained
        {
            get => _daysRemained;
            set => Set(ref _daysRemained, value);
        }
        #endregion

        #region ColorPriority
        public static readonly string normalColor = "#a0d5df";
        public static readonly string lessNormalColor = "#a0dfce";
        public static readonly string warningColor = "#dfdfa0";
        public static readonly string lessWarningColor = "#dfc7a0";
        public static readonly string criticalColor = "#cc8067";
        public static readonly string succesColor = "#77dd77";

        private Brush? _priority;

        public Brush? Priority
        {
            get => _priority;
            set => Set(ref _priority, value);
        }
        #endregion

        #region CommonCommands
        public RelayCommand? OpenMoreInformationOfPlan { get; set; }
        public RelayCommand? RemovePlanCommand { get; set; }
        public RelayCommand? AddFileCommand { get; set; }
        public RelayCommand? OpenFileCommand { get; set; }
        public RelayCommand? FavoriteCommand { get; set; }
        public RelayCommand? RemoveFileCommand { get; set; }
        public RelayCommand? RemoveAdvancedPlanCommand { get; set; }
        public RelayCommand? AddAdvancedPlanCommand { get; set; }
        public RelayCommand? OnLoadCommand { get; set; }
        public RelayCommand? SuccesCommand { get; set; }
        public RelayCommand? CancelCommand { get; set; }
        #endregion

        public MainViewModelPage()
        {
            StartPage();

            #region Commands
            OpenMoreInformationOfPlan = new(o =>
            {
                if(SelectedPlanIndex != -1 && (SelectedPlanIndex != CurrentDatas[0].InfoIndex))
                {
                    Plans[CurrentDatas[0].InfoIndex].ThisOnPlanIcon = Visibility.Collapsed;
                    LoadDatas(SelectedPlanIndex);
                    CurrentIndex = SelectedPlanIndex;
                    CurrentDatas[0].InfoIndex = CurrentIndex;
                }
            });

            RemovePlanCommand = new(o =>
            {
                if (SelectedPlanIndex > -1)
                {
                    if(SelectedPlanIndex == CurrentIndex)
                    {
                        if (CurrentIndex > 0)
                        {
                            CurrentIndex--;
                            CurrentDatas[0].InfoIndex = CurrentIndex;
                        }
                        LoadDatas(CurrentIndex);
                    }
                    Plans.RemoveAt(SelectedPlanIndex);                    
                }
            });

            AddFileCommand = new(o =>
            {
                DefaultDialogService.OpenFileDialog();
                if (DefaultDialogService.FilePath == null)
                {

                }
                else
                {
                    string iconKind;
                    if (Path.GetExtension(DefaultDialogService.FilePath).Equals(".txt") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".doc") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".rtf") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".docx") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".pdf"))
                    {
                        iconKind = "FileDocument";
                    }
                    else if (Path.GetExtension(DefaultDialogService.FilePath).Equals(".jpg") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".png") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".tif") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".gif") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".psd") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".psb") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".kra"))
                    {
                        iconKind = "Image";
                    }
                    else if (Path.GetExtension(DefaultDialogService.FilePath).Equals(".mp3") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".aac") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".wav") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".flac") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".alac") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".dsd"))
                    {
                        iconKind = "Music";
                    }
                    else
                    {
                        iconKind = "Folder";
                    }
                    Files?.Add(new()
                    {
                        FilesContent = DefaultDialogService.FilePath,
                        FileName = Path.GetFileNameWithoutExtension(DefaultDialogService.FilePath),
                        IconKind = iconKind
                    });
                    Plans[CurrentIndex].Files.Add(new()
                    {
                        FilesContent = DefaultDialogService.FilePath,
                        FileName = Path.GetFileNameWithoutExtension(DefaultDialogService.FilePath),
                        IconKind = iconKind
                    });
                    DataSaveLoad.SaveDatas(DataSaveLoad.JsonPathPlans, Plans);

                }
            });

            OpenFileCommand = new(o =>
            {
                try
                {
                    var fileProcces = new Process();
                    fileProcces.StartInfo = new ProcessStartInfo(Files[SelectedIndexFiles].FilesContent)
                    {
                        UseShellExecute = true
                    };
                    fileProcces.Start();
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }                
            });

            FavoriteCommand = new(o =>
            {
                try
                {
                    if(Plans[CurrentDatas[0].InfoIndex].Favorite == Visibility.Visible)
                    {
                        Plans[CurrentDatas[0].InfoIndex].Favorite = Visibility.Collapsed;
                    }
                    else
                    {
                        Plans[CurrentDatas[0].InfoIndex].Favorite = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }
            });

            RemoveFileCommand = new(o =>
            {
                try
                {
                    Files.RemoveAt(SelectedIndexFiles);
                    Plans[CurrentDatas[0].InfoIndex].Files.Clear();
                    for (int i = 0; i < Files.Count; i++)
                    {
                        Plans[CurrentDatas[0].InfoIndex].Files.Add(Files[i]);
                    }                   
                    DataSaveLoad.SaveDatas(DataSaveLoad.JsonPathPlans, Plans);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }
            });

            AddAdvancedPlanCommand = new(o =>
            {
                try
                {
                    foreach (var item in AdvancedPlans)
                    {
                        item.IsPressed -= Item_IsPressed;
                        item.IsTexted -= Item_IsTexted;
                    }
                    AdvancedPlans.Add(new ());
                    foreach (var item in AdvancedPlans)
                    {
                        item.IsPressed += Item_IsPressed;
                        item.IsTexted += Item_IsTexted;
                    }
                    Plans[CurrentDatas[0].InfoIndex].AdvancedPlans.Clear();
                    for (int i = 0; i < AdvancedPlans.Count; i++)
                    {
                        Plans[CurrentDatas[0].InfoIndex].AdvancedPlans.Add(AdvancedPlans[i]);
                    }
                    DataSaveLoad.SaveDatas(DataSaveLoad.JsonPathPlans, Plans);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }
            });

            RemoveAdvancedPlanCommand = new(o =>
            {
                try
                {
                    foreach (var item in AdvancedPlans)
                    {
                        item.IsPressed -= Item_IsPressed;
                        item.IsTexted -= Item_IsTexted;
                    }
                    AdvancedPlans.RemoveAt(SelectedIndexAdvancedPlans);
                    foreach (var item in AdvancedPlans)
                    {
                        item.IsPressed += Item_IsPressed;
                        item.IsTexted += Item_IsTexted;
                    }
                    Plans[CurrentDatas[0].InfoIndex].AdvancedPlans.Clear();
                    for (int i = 0; i < AdvancedPlans.Count; i++)
                    {
                        Plans[CurrentDatas[0].InfoIndex].AdvancedPlans.Add(AdvancedPlans[i]);
                    }
                    DataSaveLoad.SaveDatas(DataSaveLoad.JsonPathPlans, Plans);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }
            });

            SuccesCommand = new(o =>
            {
                try
                {
                    Plans[CurrentDatas[0].InfoIndex].PlanStatus = "Выполнен";
                    Plans[CurrentDatas[0].InfoIndex].Failed = Visibility.Collapsed;
                    AddPrioiry(CurrentDatas[0].InfoIndex);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }
            });

            CancelCommand = new(o =>
            {
                try
                {
                    Plans[CurrentDatas[0].InfoIndex].PlanStatus = "Выполняется";
                    Plans[CurrentDatas[0].InfoIndex].Failed = Visibility.Collapsed;
                    AddPrioiry(CurrentDatas[0].InfoIndex);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }
            });

            OnLoadCommand = new(o =>
            {
                try
                {
                    //CheckFailedPlansAsync(); - Ломает всё
                    FailedCheck();
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }
            });
            #endregion
        }

        #region Methods
        #region StartOperation
        public void StartPage()
        {
            try
            {
                #region LoadCollections
                try
                {
                    if(NullChecker(Plans))
                    {
                        Plans = new();
                        try
                        {
                            Plans = DataSaveLoad.LoadData<PlanModel>(DataSaveLoad.JsonPathPlans);
                        }
                        catch (Exception ex)
                        {

                            Debug.WriteLine("Неудалось загрузить файлы коллекции Plans.\nКод ошибки: " + ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            Plans = DataSaveLoad.LoadData<PlanModel>(DataSaveLoad.JsonPathPlans);
                        }
                        catch (Exception ex)
                        {

                            Debug.WriteLine("Неудалось загрузить файлы коллекции Plans.\nКод ошибки: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {

                    Debug.WriteLine("Что-то пошло не так в методе загрузки.\nКод ошибки: " + ex.Message);
                }

                try
                {
                    if (NullChecker(CurrentDatas))
                    {
                        CurrentDatas = new();
                        try
                        {
                            CurrentDatas = DataSaveLoad.LoadData<CurrentDataModel>(DataSaveLoad.JsonPathCurrentData);
                            if(CurrentDatas.Count == 0)
                            {
                                CurrentDatas.Add(new()
                                {
                                    InfoIndex = 0,
                                    EditIndex = 0
                                });
                            }
                        }
                        catch (Exception ex)
                        {

                            Debug.WriteLine("Неудалось загрузить файлы коллекции CurrentData.\nКод ошибки: " + ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            CurrentDatas = DataSaveLoad.LoadData<CurrentDataModel>(DataSaveLoad.JsonPathCurrentData);
                            if (CurrentDatas.Count == 0)
                            {
                                CurrentDatas.Add(new()
                                {
                                    InfoIndex = 0,
                                    EditIndex = 0
                                });
                            }
                        }
                        catch (Exception ex)
                        {

                            Debug.WriteLine("Неудалось загрузить файлы коллекции CurrentData.\nКод ошибки: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {

                    Debug.WriteLine("Что-то пошло не так в методе загрузки.\nКод ошибки: " + ex.Message);
                }

                try
                {
                    if (NullChecker(Files))
                    {
                        Files = new();
                        try
                        {
                            for (int i = 0; i < Plans[CurrentDatas[0].InfoIndex].Files.Count; i++)
                            {
                                Files.Add(Plans[CurrentDatas[0].InfoIndex].Files[i]);
                            }                           
                        }
                        catch (Exception ex)
                        {

                            Debug.WriteLine("Неудалось загрузить файлы коллекции Files.\nКод ошибки: " + ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            for (int i = 0; i < Plans[CurrentDatas[0].InfoIndex].Files.Count; i++)
                            {
                                Files.Add(Plans[CurrentDatas[0].InfoIndex].Files[i]);
                            }
                        }
                        catch (Exception ex)
                        {

                            Debug.WriteLine("Неудалось загрузить файлы коллекции Files.\nКод ошибки: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {

                    Debug.WriteLine("Что-то пошло не так в методе загрузки.\nКод ошибки: " + ex.Message);
                }

                try
                {
                    if (NullChecker(AdvancedPlans))
                    {
                        AdvancedPlans = new ObservableCollection<AdvancedPlanModel>();
                        try
                        {
                            AdvancedPlans.Clear();
                            for (int i = 0; i < Plans[CurrentDatas[0].InfoIndex].AdvancedPlans.Count; i++)
                            {
                                AdvancedPlans.Add(Plans[CurrentDatas[0].InfoIndex].AdvancedPlans[i]);
                            }
                            foreach (var item in AdvancedPlans)
                            {
                                item.IsPressed += Item_IsPressed;
                                item.IsTexted += Item_IsTexted;
                            }
                        }
                        catch (Exception ex)
                        {

                            Debug.WriteLine("Неудалось загрузить файлы коллекции AdvancedPlans.\nКод ошибки: " + ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            AdvancedPlans.Clear();
                            for (int i = 0; i < Plans[CurrentDatas[0].InfoIndex].AdvancedPlans.Count; i++)
                            {
                                AdvancedPlans.Add(Plans[CurrentDatas[0].InfoIndex].AdvancedPlans[i]);
                            }
                            foreach (var item in AdvancedPlans)
                            {
                                item.IsPressed += Item_IsPressed;
                                item.IsTexted += Item_IsTexted;
                            }
                        }
                        catch (Exception ex)
                        {

                            Debug.WriteLine("Неудалось загрузить файлы коллекции AdvancedPlans.\nКод ошибки: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {

                    Debug.WriteLine("Что-то пошло не так в методе загрузки.\nКод ошибки: " + ex.Message);
                }
                #endregion

                #region LoadInformations
                try
                {
                    if (Plans.Count > 0)
                    {
                        CurrentIndex = CurrentDatas[0].InfoIndex;
                        PlanHeader = Plans[CurrentIndex].PlanHeader;
                        DeadLine = Plans[CurrentIndex].DeadLine.ToString("d"); ;
                        HoursRemained = Math.Round((Plans[CurrentIndex].DeadLine - DateTime.Now).TotalHours, 1).ToString() + " Ч";
                        DaysRemained = Math.Round((Plans[CurrentIndex].DeadLine - DateTime.Now).TotalDays, 1).ToString() + " Д";
                        PlanContent = Plans[CurrentIndex].PlanContent;
                        Plans[CurrentIndex].ThisOnPlanIcon = Visibility.Visible;
                        AddPrioiry(CurrentIndex);
                    }
                    else
                    {
                        PlanHeader = "Нет данных";
                        DeadLine = "Нет данных";
                        HoursRemained = "Нет данных";
                        DaysRemained = "Нет данных";
                        PlanContent = "Нет данных";
                        Files.Clear();
                        AdvancedPlans.Clear();
                        Priority = (Brush)new BrushConverter().ConvertFrom("#FFFFF");
                    }
                }
                catch (Exception ex)
                {

                    Debug.WriteLine("Что-то пошло не так в загрузке информации текста.\nКод ошибки: " + ex.Message);
                }
                #endregion
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Что-то пошло не так в методе загрузки.\nКод ошибки: " + ex.Message);
            }
        }
        #endregion

        #region CheckOperations
        public static bool NullChecker<T>(IEnumerable<T>? collection)
        {
            bool isNull = false;

            if (collection is null)
                isNull = true;

            return isNull;
        }

        public void FailedCheck()
        {
            try
            {
                for (int i = 0; i < Plans.Count; i++)
                {
                    if (((Plans[i].DeadLine - DateTime.Now).TotalMinutes < 1) && (Plans[i].PlanStatus != "Выполнен"))
                    {
                        Plans[i].Failed = Visibility.Visible;
                        Plans[i].PlanStatus = "Просрочен";
                    }
                }                
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Events
        private void Item_IsTexted(AdvancedPlanModel obj)
        {
            Plans[CurrentDatas[0].InfoIndex].AdvancedPlans.Clear();
            for (int i = 0; i < AdvancedPlans.Count; i++)
            {
                Plans[CurrentDatas[0].InfoIndex].AdvancedPlans.Add(AdvancedPlans[i]);
            }
            DataSaveLoad.SaveDatas(DataSaveLoad.JsonPathPlans, Plans);
        }

        private void Item_IsPressed(AdvancedPlanModel obj)
        {
            Plans[CurrentDatas[0].InfoIndex].AdvancedPlans.Clear();
            for (int i = 0; i < AdvancedPlans.Count; i++)
            {
                Plans[CurrentDatas[0].InfoIndex].AdvancedPlans.Add(AdvancedPlans[i]);
            }
            DataSaveLoad.SaveDatas(DataSaveLoad.JsonPathPlans, Plans);
        }
        #endregion

        #region ColorPriority
        public void AddPrioiry(int index)
        {
            if (index != -1)
            {
                if ((Plans[index].DeadLine - DateTime.Now).TotalHours >= 96)
                    Priority = (Brush)new BrushConverter().ConvertFrom(normalColor);

                if (((Plans[index].DeadLine - DateTime.Now).TotalHours < 96) && ((Plans[index].DeadLine - DateTime.Now).TotalHours >= 72))
                    Priority = (Brush)new BrushConverter().ConvertFrom(lessNormalColor);

                if (((Plans[index].DeadLine - DateTime.Now).TotalHours < 72) && ((Plans[index].DeadLine - DateTime.Now).TotalHours >= 48))
                    Priority = (Brush)new BrushConverter().ConvertFrom(warningColor);

                if (((Plans[index].DeadLine - DateTime.Now).TotalHours < 48) && ((Plans[index].DeadLine - DateTime.Now).TotalHours >= 32))
                    Priority = (Brush)new BrushConverter().ConvertFrom(lessWarningColor);

                if ((Plans[index].DeadLine - DateTime.Now).TotalHours < 32)
                    Priority = (Brush)new BrushConverter().ConvertFrom(criticalColor);
                if (Plans[index].PlanStatus == "Выполнен")
                    Priority = (Brush)new BrushConverter().ConvertFrom(succesColor);               

            }

            foreach (var item in Plans)
            {
                if ((item.DeadLine - DateTime.Now).TotalHours >= 96)
                    item.PriorityFlat = (Brush)new BrushConverter().ConvertFrom(normalColor);

                if (((item.DeadLine - DateTime.Now).TotalHours < 96) && ((item.DeadLine - DateTime.Now).TotalHours >= 72))
                    item.PriorityFlat = (Brush)new BrushConverter().ConvertFrom(lessNormalColor);

                if (((item.DeadLine - DateTime.Now).TotalHours < 72) && ((item.DeadLine - DateTime.Now).TotalHours >= 48))
                    item.PriorityFlat = (Brush)new BrushConverter().ConvertFrom(warningColor);

                if (((item.DeadLine - DateTime.Now).TotalHours < 48) && ((item.DeadLine - DateTime.Now).TotalHours >= 32))
                    item.PriorityFlat = (Brush)new BrushConverter().ConvertFrom(lessWarningColor);

                if ((item.DeadLine - DateTime.Now).TotalHours < 32)
                    item.PriorityFlat = (Brush)new BrushConverter().ConvertFrom(criticalColor);
                if(item.PlanStatus == "Выполнен")
                {
                    item.PriorityFlat = (Brush)new BrushConverter().ConvertFrom(succesColor);
                    item.Complete = Visibility.Visible;
                }
                else
                {
                    item.Complete = Visibility.Collapsed;
                }
            }
        }
        #endregion

        #region LoadOperation
        public void LoadDatas(int index)
        {
            try
            {
                if (Plans.Count > 0)
                {                    
                    PlanHeader = Plans[index].PlanHeader;
                    DeadLine = Plans[index].DeadLine.ToString("d"); ;
                    HoursRemained = Math.Round((Plans[index].DeadLine - DateTime.Now).TotalHours, 1).ToString() + " Ч";
                    DaysRemained = Math.Round((Plans[index].DeadLine - DateTime.Now).TotalDays, 1).ToString() + " Д";
                    PlanContent = Plans[index].PlanContent;
                    Plans[index].ThisOnPlanIcon = Visibility.Visible;
                    AddPrioiry(index);

                    try
                    {
                        Files.Clear();
                        for (int i = 0; i < Plans[index].Files.Count; i++)
                        {
                            Files.Add(Plans[index].Files[i]);
                        }

                        foreach (var item in AdvancedPlans)
                        {
                            item.IsPressed -= Item_IsPressed;
                            item.IsTexted -= Item_IsTexted;
                        }
                        AdvancedPlans.Clear();
                        for (int i = 0; i < Plans[index].AdvancedPlans.Count; i++)
                        {
                            AdvancedPlans.Add(Plans[index].AdvancedPlans[i]);
                        }
                        foreach (var item in AdvancedPlans)
                        {
                            item.IsPressed += Item_IsPressed;
                            item.IsTexted += Item_IsTexted;
                        }
                    }
                    catch (Exception ex)
                    {

                        Debug.WriteLine("Что-то пошло не так в загрузке коллекций.\nКод ошибки: " + ex.Message);
                    }                   
                }
                else
                {
                    PlanHeader = "Нет данных";
                    DeadLine = "Нет данных";
                    HoursRemained = "Нет данных";
                    DaysRemained = "Нет данных";
                    PlanContent = "Нет данных";
                    Files.Clear();
                    AdvancedPlans.Clear();
                    Priority = (Brush)new BrushConverter().ConvertFrom("#FFFFF");
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine("Что-то пошло не так в загрузке информации текста.\nКод ошибки: " + ex.Message);
            }
        }
        #endregion

        #region AsyncOperations
        
        //public async void CheckFailedPlansAsync()
        //{
        //    await Task.Run(() => CheckFailedPlans());
        //}

        //public void CheckFailedPlans()
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            for (int i = 0; i < Plans.Count; i++)
        //            {
        //                if ((Plans[i].DeadLine - DateTime.Now).TotalMinutes < 1)
        //                {
        //                    Plans[i].Failed = Visibility.Visible;
        //                    Plans[i].PlanStatus = "Просрочен";
        //                }
        //            }
        //            Debug.WriteLine("Worked...");
        //            Thread.Sleep(8000);
        //        }
        //        catch (Exception ex)
        //        {

        //            Debug.WriteLine(ex.Message);
        //        }
        //    }
        //}
        #endregion
        #endregion
    }
}
