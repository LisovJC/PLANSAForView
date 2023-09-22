using PLANSA.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace PLANSA.Model
{
    internal class PlanModel : ObservableObject
    {
     
        private string? _planHeader;

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

        private DateTime _dateAdd;

        public DateTime DateAdd
        {
            get => _dateAdd;
            set => Set(ref _dateAdd, value);
        }

        private List<FileModel>? _files;

        public List<FileModel>? Files
        {
            get =>_files;
            set => Set(ref _files, value);
        }

        private List<AdvancedPlanModel>? _advancedPlans;

        public List<AdvancedPlanModel>? AdvancedPlans
        {
            get => _advancedPlans;
            set => Set(ref _advancedPlans, value);
        }

        private string? _planStatus;

        public string PlanStatus
        {
            get => _planStatus;
            set => Set(ref _planStatus, value);
        }

        private Brush _priorityFlat;

        public Brush PriorityFlat
        {
            get => _priorityFlat;
            set => Set(ref _priorityFlat, value);
        }

        private Visibility _thisOnPlanIcon = Visibility.Collapsed;

        public Visibility ThisOnPlanIcon
        {
            get => _thisOnPlanIcon;
            set => Set(ref _thisOnPlanIcon, value);
        }

        private Visibility _favorite = Visibility.Collapsed;

        public Visibility Favorite
        {
            get => _favorite;
            set => Set(ref _favorite, value);
        }

        private Visibility _failed = Visibility.Collapsed;

        public Visibility Failed
        {
            get => _failed;
            set => Set(ref _failed, value);
        }

        private Visibility _complete = Visibility.Collapsed;

        public Visibility Complete
        {
            get => _complete;
            set => Set(ref _complete, value);
        }
    }
}
