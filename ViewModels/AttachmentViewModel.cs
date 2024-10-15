
namespace Municipal_App.ViewModels
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Represents an ATTACHMENT, but implements the INotifyPropertyChanged interface to 
    /// allow for more effective UI binding. Also stores all of the Info for 
    /// Any UI elements to bind to.
    /// </summary>
    internal class AttachmentViewModel : ViewModelBase
    {

		//-----------------------------------------------------------------------------
		/// <summary>
		/// Backing field for FileName
		/// </summary>
		private string _fileName;

        //-----------------------------------------------------------------------------
		/// <summary>
		/// FileName for the UI to bind to
		/// </summary>
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

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Backing field for the File
		/// </summary>
        private byte[] _file;

        //-----------------------------------------------------------------------------
		/// <summary>
		/// File field for the UI to bind to
		/// </summary>
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

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Creates an emtpy AttachmentViewModel
		/// </summary>
        public AttachmentViewModel()
        {
            this._fileName = string.Empty;
			this._file = new byte[0];
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates an AttachmentViewModel from an ATTACHMENT object
        /// </summary>
        /// <param name="attachment"> 
		/// Attachment from which this AttachmentViewModel is created
		/// </param>
        public AttachmentViewModel(ATTACHMENT attachment)
        {
			this._fileName = attachment.NAME_OF_FILE;
			this._file = attachment.FILE_BINARY;
        }

        //-----------------------------------------------------------------------------
		/// <summary>
		/// Converts this AttachmentViewModel to an ATTACHMENT
		/// </summary>
		/// <returns></returns>
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
//---------------------------------------EOF-------------------------------------------