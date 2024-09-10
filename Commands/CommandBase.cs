using System;
using System.Windows.Input;

namespace Municipal_App.Commands
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Base command class which implements the ICommand interface. Intended for each
    /// specific command to extend this class. Contains basic functionallity required
    /// for each command.
    /// </summary>
    public abstract class CommandBase : ICommand
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Event which notifies subscribers when the abillity to execute the command 
        /// has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Determines whether the command can execute or not. Intended to be optional
        /// to the commands that extend the CommandBase. This method should be 
        /// overrided if a command needs this functionallity.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns> true </returns>
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Needs to be implemented to determine what happens when the command is 
        /// executed.
        /// </summary>
        /// <param name="parameter"></param>
        public abstract void Execute(object parameter);

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Raises the CanExecuteChanged event and notifies subscribers.
        /// </summary>
        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}
//---------------------------------------EOF-------------------------------------------
