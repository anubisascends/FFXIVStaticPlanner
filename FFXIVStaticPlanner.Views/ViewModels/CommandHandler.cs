using System;
using System.Windows.Input;

namespace FFXIVStaticPlanner.ViewModels
{
    /// <summary>
    /// Provides a way to handle commands in view models
    /// </summary>
    public class CommandHandler : ICommand
    {
        /// <summary>
        /// Gets predicate that determiens whether or not this handler can actually be executed
        /// </summary>
        private Predicate<object> Executable
        {
            get;
        }

        /// <summary>
        /// Gets the action that is performed when this command is executed
        /// </summary>
        private Action<object> Action
        {
            get;
        }

        /// <summary>
        /// Handles the change of CanExecute for this handler
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandler"/> class
        /// </summary>
        /// <param name="action">The action to execute</param>
        /// <param name="canExecute">The predicate that determines whether execuation is allowed</param>
        public CommandHandler ( Action<object> action , Predicate<object> canExecute )
        {
            Executable = canExecute;
            Action = action;
        }

        /// <summary>
        /// Returns a value that indicates whether or not this command handler can execute
        /// </summary>
        /// <param name="parameter">The parameter that can help with this determination</param>
        /// <returns><see langword="true"/> if the command can be executed, otherwise <see langword="false"/></returns>
        public bool CanExecute ( object parameter ) => Executable ( parameter );

        /// <summary>
        /// Performs the action that is tied to this handler
        /// </summary>
        /// <param name="parameter">The parameter that can help with this execution</param>
        public void Execute ( object parameter ) => Action ( parameter );
    }
}