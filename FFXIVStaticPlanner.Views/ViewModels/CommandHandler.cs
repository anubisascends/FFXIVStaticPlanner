using System;
using System.Windows.Input;

namespace FFXIVStaticPlanner.ViewModels
{
    public class CommandHandler : ICommand
    {
        private Predicate<object> Executable
        {
            get;
        }

        private Action<object> Action
        {
            get;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public CommandHandler ( Action<object> action , Predicate<object> canExecute )
        {
            Executable = canExecute;
            Action = action;
        }

        public bool CanExecute ( object parameter ) => Executable ( parameter );

        public void Execute ( object parameter ) => Action ( parameter );
    }
}