using Microsoft.Win32;
using Municipal_App.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Municipal_App.Commands
{
    internal class ChooseFileCommand : CommandBase
    {
        private readonly ReportViewModel _reportViewModel;
        public ChooseFileCommand(ReportViewModel report)
        {
            this._reportViewModel = report;
        }

        public override void Execute(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select a file",
                Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|Document Files (*.pdf;*.doc;*.docx;*.txt)|*.pdf;*.doc;*.docx;*.txt",
                Multiselect = true,
            };

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

                        // Add the file bytes to the Attachments list
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
                MessageBox.Show("No file was selected."); // Handle if no file was selected
            }
        }
    }
}
