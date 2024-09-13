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
    internal class RemoveAttachmentCommand : CommandBase
    {
        private readonly ObservableCollection<AttachmentViewModel> _attachments;

        public RemoveAttachmentCommand(ObservableCollection<AttachmentViewModel> attachments)
        {
            this._attachments = attachments;
        }

        public override void Execute(object parameter)
        {
            if (parameter is AttachmentViewModel attachmentVM)
            {
                AppStore.Instance.BannerMessageStore.SetBanner($" File \"{attachmentVM.FileName}\" removed", BannerType.Confirmation);
                this._attachments.Remove(attachmentVM);
            }
        }
    }
}
