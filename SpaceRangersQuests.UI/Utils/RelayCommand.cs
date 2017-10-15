using System;
using System.Windows.Input;

namespace SpaceRangersQuests.UI.Utils
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _onExecute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> onExecute, Func<object, bool> canExecute = null)
        {
            if (onExecute == null)
                throw new ArgumentNullException("onExecute");
            _onExecute = onExecute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action onExecute, Func<bool> canExecute = null)
            : this(onExecute != null ? o => onExecute() : (Action<object>)null, canExecute != null ? o => canExecute() : (Func<object, bool>)null)
        { }


        public event EventHandler CanExecuteChanged;


        public void Execute(object parameter)
        {
            _onExecute(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            var e = CanExecuteChanged;
            if (e != null)
                e(this, EventArgs.Empty);
        }
    }
}
