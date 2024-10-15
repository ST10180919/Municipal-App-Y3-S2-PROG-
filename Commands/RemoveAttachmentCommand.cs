using Municipal_App.Stores;
using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.Commands
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Command used to remove attached files added by the user
    /// </summary>
    internal class RemoveAttachmentCommand : CommandBase
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// List of attachments from which the selected one will be removed
        /// </summary>
        private readonly ObservableCollection<AttachmentViewModel> _attachments;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new RemoveAttachmentCommand
        /// </summary>
        /// <param name="attachments">
        /// Collection of AttachmentViewModels from which a selected one will be removed
        /// </param>
        public RemoveAttachmentCommand(ObservableCollection<AttachmentViewModel> attachments)
        {
            this._attachments = attachments;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Code run when the command is executed
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            if (parameter is AttachmentViewModel attachmentVM)
            {
                // Notifying user
                AppStore.Instance.BannerMessageStore.SetBanner($" File \"{attachmentVM.FileName}\" removed", BannerType.Confirmation);

                // Removing attachment (Observable collection will notify UI)
                this._attachments.Remove(attachmentVM);
            }
        }
    }
}
//---------------------------------------EOF-------------------------------------------