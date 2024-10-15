using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Municipal_App.Commands
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Command that allows for the method executed to provided when constructed.
    /// Used to allow commands to execute in the ViewModel that makes use of them.
    /// Disclosure of AI: This Class was made with the help of ChatGPT 4o Model
    /// </summary>
    public class RelayCommand : ICommand
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// The action to execute when the command is invoked.
        /// </summary>
        private readonly Action<object> _execute;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// The function to determine whether the command can execute.
        /// If null, the command can always execute.
        /// </summary>
        private readonly Func<object, bool> _canExecute;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to execute when the command is invoked.</param>
        /// <param name="canExecute">The function to determine if the command can execute. Defaults to null.</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command. Can be null.</param>
        /// <returns>True if the command can execute; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Executes the command logic.
        /// </summary>
        /// <param name="parameter">Data used by the command. Can be null.</param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
//---------------------------------------EOF-------------------------------------------