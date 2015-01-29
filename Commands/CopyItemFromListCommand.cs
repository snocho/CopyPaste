namespace CopyPaste.Commands
{
    using System.Windows.Input;
    using System;
    using Models;

    internal class CopyItemFromListCommand : ICommand
    {
        public CopyItemFromListCommand(bool canExecute, CopyPasteItem cpi)
        {
            _cpi = cpi;
            _canExecute = canExecute;
        }

        private readonly bool _canExecute;
        private readonly CopyPasteItem _cpi;

        #region ICommand Members
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public void Execute(object parameter)
        {
            _cpi.CopyItemFromList((string)parameter);
        }
        #endregion
    }
}
