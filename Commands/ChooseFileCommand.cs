using Microsoft.Win32;
using Municipal_App.Stores;
using Municipal_App.ViewModels;
using System.IO;
using System.Windows;

namespace Municipal_App.Commands
{
    //---------------------------------------------------------------------------------
    /// <summary>
    /// Command used to show a file dialog allowing users to choose files to attach
    /// to their issue report
    /// </summary>
    internal class ChooseFileCommand : CommandBase
    {
        //-----------------------------------------------------------------------------
        /// <summary>
        /// ReportViewModel for which the files are being attached to
        /// </summary>
        private readonly ReportViewModel _reportViewModel;

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Creates a new ChooseFileCommand
        /// </summary>
        /// <param name="report">
        /// The ViewModel containing the report to be added
        /// </param>
        public ChooseFileCommand(ReportViewModel report)
        {
            this._reportViewModel = report;
        }

        //-----------------------------------------------------------------------------
        /// <summary>
        /// Code run when the command is executed
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a file",
                // These are the file types the user can choose from
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Document Files (*.pdf;*.doc;*.docx;*.txt)|*.pdf;*.doc;*.docx;*.txt",
                Multiselect = true,
            };

            // Showing the dialog to the user
            bool? result = openFileDialog.ShowDialog();

            // handle the selected files
            if (result == true)
            {
                foreach (string filePath in openFileDialog.FileNames)
                {
                    try
                    {
                        // Read the file as a byte array
                        byte[] fileBytes = File.ReadAllBytes(filePath);

                        // Add the file bytes to the ReportViewModel's attachments
                        var attachment = new ATTACHMENT()
                        {
                            NAME_OF_FILE = Path.GetFileName(filePath),
                            FILE_BINARY = fileBytes
                        };
                        this._reportViewModel.Attachments.Add(new AttachmentViewModel(attachment));

                        // Notify UI
                        this._reportViewModel.NotifyUIofAttachmentsChange();
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Error reading file: " + ex.Message);
                    }
                }
            }
            else
            {
                AppStore.Instance.BannerMessageStore.SetBanner("No file was chosen", BannerType.Validation);
            }
        }
    }
}
//---------------------------------------EOF-------------------------------------------