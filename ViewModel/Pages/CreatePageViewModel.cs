using PLANSA.Command;
using PLANSA.Core;
using PLANSA.Model;
using PLANSA.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace PLANSA.ViewModel.Pages
{
    internal class CreatePageViewModel : ObservableObject
    {
        #region Collections
        public static ObservableCollection<FileModel>? Files { get; set; }
        public static ObservableCollectionEX<PlanModel>? Plans { get; set; }
        public ObservableCollection<AdvancedPlanModel>? AdvancedPlans { get; set; }
        #endregion

        #region Commands
        public RelayCommand? AddFileCommand { get; set; }
        public RelayCommand? AddPlanCommand { get; set; }
        public RelayCommand? AddAdvancedPlanTemplateCommand { get; set; }
        public RelayCommand? RemoveAdvancedPlanTemplateCommand { get; set; }
        #endregion

        #region VisibleProperties
        private Visibility _visibleFileIcon = Visibility.Visible;

        public Visibility VisibleFileIcon
        {
            get => _visibleFileIcon;
            set => Set(ref _visibleFileIcon, value);
        }
        #endregion

        #region TextProperties
        private string _planHeader;

        public string PlanHeader
        {
            get => _planHeader;
            set => Set(ref _planHeader, value);
        }

        private DateTime _deadLine;

        public DateTime DeadLine
        {
            get => _deadLine;
            set => Set(ref _deadLine, value);
        }

        private string? _planContent;

        public string PlanContent
        {
            get => _planContent;
            set => Set(ref _planContent, value);
        }

        private string? _advancedPlanContent;

        public string AdvancedPlanContent
        {
            get => _advancedPlanContent;
            set => Set(ref _advancedPlanContent, value);
        }
        #endregion

        #region Indexes
        private int _selectedIndexAdvancedPlans;

        public int SelectedIndexAdvancedPlans
        {
            get => _selectedIndexAdvancedPlans;
            set => Set(ref _selectedIndexAdvancedPlans, value);
        }

        #endregion

        public CreatePageViewModel()
        {
            #region Commands
            AddFileCommand = new(o =>
            {
                DefaultDialogService.OpenFileDialog();
                if (DefaultDialogService.FilePath == null)
                {

                }
                else
                {
                    string iconKind;
                    if(Path.GetExtension(DefaultDialogService.FilePath).Equals(".txt") ||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".doc")||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".rtf")||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".docx")||
                        Path.GetExtension(DefaultDialogService.FilePath).Equals(".pdf"))
                    {
                        iconKind = "FileDocument";
                    }
                    else if(Path.GetExtension(DefaultDialogService.FilePath).Equals(".jpg") ||
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
                    VisibleFileIcon = Visibility.Hidden;
                }
            });

            AddAdvancedPlanTemplateCommand = new(o =>
            {
                AdvancedPlans.Add(new()
                {
                    Content = AdvancedPlanContent,
                    IsCheck = false
                });
            });

            RemoveAdvancedPlanTemplateCommand = new(o =>
            {
                if(SelectedIndexAdvancedPlans != -1)
                    AdvancedPlans.RemoveAt(SelectedIndexAdvancedPlans);
            });

            AddPlanCommand = new(o =>
            {
                List<AdvancedPlanModel> advancedPlans = new();
                List<FileModel> files = new();
                if (Files != null && AdvancedPlans != null && ((PlanHeader != null) || PlanHeader != string.Empty))
                {
                    for (int i = 0; i < AdvancedPlans.Count; i++)
                    {
                        advancedPlans.Add(AdvancedPlans[i]);
                    }
                    for (int i = 0; i < Files.Count; i++)
                    {
                       files.Add(Files[i]);
                    }
                    Plans?.Add(new()
                    {
                        PlanHeader = PlanHeader,
                        PlanContent = PlanContent,
                        PlanStatus = "Выполняется",
                        Files = files,
                        DeadLine = DeadLine,
                        DateAdd = DateTime.UtcNow,
                        AdvancedPlans = advancedPlans
                    });                   
                    PlanHeader = String.Empty;
                    PlanContent = String.Empty;
                    DeadLine = DateTime.UtcNow;
                    Files.Clear();
                    VisibleFileIcon = Visibility.Visible;
                    DataSaveLoad.SaveDatas(DataSaveLoad.JsonPathPlans, Plans);
                    AdvancedPlans.Clear();
                }
            });
            #endregion

            StartPage();
        }

        #region Methods
        private void StartPage()
        {
            if(Files is null || Plans is null || AdvancedPlans is null)
            {
                Debug.Write("Одна из коллекций - NULL, Были загружены данные и созданы новые коллекции.");
                
                Files = new();
                AdvancedPlans = new();
                Plans = new();
                Plans = DataSaveLoad.LoadData<PlanModel>(DataSaveLoad.JsonPathPlans);                
            }
            else
            {

            }
        }
        #endregion

    }
}
