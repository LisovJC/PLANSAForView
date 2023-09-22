using PLANSA.Command;
using PLANSA.Core;
using System;

namespace PLANSA.Model
{
    public class AdvancedPlanModel : ObservableObject
    {
        private string _content;

        public string Content
        {
            get => _content;
            set => Set(ref _content, value);
        }

        private bool _isCheck;

        public bool IsCheck
        {
            get => _isCheck;
            set => Set(ref _isCheck, value);
        }

        public event Action<AdvancedPlanModel> IsPressed;

        private RelayCommand _pressCommand = null;

        public RelayCommand PressCommand => _pressCommand ?? (_pressCommand = new RelayCommand(Handler));

        public void Handler(object o)
        {            
            IsPressed?.Invoke(this);
        }

        public event Action<AdvancedPlanModel> IsTexted;

        private RelayCommand _textedCommand = null;

        public RelayCommand TextedCommand => _textedCommand ?? (_textedCommand = new RelayCommand(HandlerText));

        public void HandlerText(object o)
        {
            IsTexted?.Invoke(this);
        }
    }
}
