using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Municipal_App.ViewModels
{
    internal class AttachmentViewModel : ViewModelBase
    {
		private string _fileName;
		public string FileName
		{
			get
			{
				return _fileName;
			}
			set
			{
				_fileName = value;
				OnPropertyChanged(nameof(FileName));
			}
		}

		private byte[] _file;
		public byte[] File
		{
			get
			{
				return _file;
			}
			set
			{
				_file = value;
				OnPropertyChanged(nameof(File));
			}
		}

        public AttachmentViewModel()
        {
            this._fileName = string.Empty;
			this._file = new byte[0];
        }

        public AttachmentViewModel(ATTACHMENT attachment)
        {
			this._fileName = attachment.NAME_OF_FILE;
			this._file = attachment.FILE_BINARY;
        }

		public ATTACHMENT ConvertToAttachment()
		{
			return new ATTACHMENT()
			{
				NAME_OF_FILE = this._fileName,
				FILE_BINARY = this._file
			};
		}
    }
}
