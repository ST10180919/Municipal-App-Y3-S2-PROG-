using Municipal_App.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Municipal_App.Commands
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Command used to submit feedback provided by the user
    /// </summary>
    internal class SubmitFeedbackCommand : CommandBase
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Action used to clear the Feedback input, wherever it exists
        /// </summary>
        private readonly Action _clearFeedbackInput;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a SubmitFeedbackCommand
        /// </summary>
        /// <param name="clearFeedbackInput">
        /// Action provided to clear the feedback input once it has been submited
        /// </param>
        public SubmitFeedbackCommand(Action clearFeedbackInput)
        {
            this._clearFeedbackInput = clearFeedbackInput;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Code run when the command is executed
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            // Notifying user
            AppStore.Instance.BannerMessageStore.SetBanner("Feedback Recieved. Thank you for helping improve our services!", BannerType.Confirmation);

            // Clearing feedback
            this._clearFeedbackInput();
        }
    }
}
//---------------------------------------EOF-------------------------------------------