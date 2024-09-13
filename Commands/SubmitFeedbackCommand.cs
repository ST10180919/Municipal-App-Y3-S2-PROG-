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
    internal class SubmitFeedbackCommand : CommandBase
    {
        private readonly Action _clearFeedbackInput;
        public SubmitFeedbackCommand(Action clearFeedbackInput)
        {
            this._clearFeedbackInput = clearFeedbackInput;
        }
        public override void Execute(object parameter)
        {
            AppStore.Instance.BannerMessageStore.SetBanner("Feedback Recieved. Thank you for helping improve our services!", BannerType.Confirmation);
            this._clearFeedbackInput();
        }
    }
}
