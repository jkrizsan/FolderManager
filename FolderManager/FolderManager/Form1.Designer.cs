using FolderManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderManager
{
    partial class Form1
    {
        

        private OpenFileDialog openFileDialog = new OpenFileDialog();
        private List<string> fileList = new List<string>();

        private HttpClient client = new HttpClient();
        private readonly IDownloadService downloadService;
        private View view = new View();

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task DownloadFileNames()
        {
            fileList = await downloadService.DownloadFileNames();
            if (fileList == null)
            {
                MessageBox.Show($"Error happened during download the file names");
            }
        }

        private async Task DownloadClick(object sender, EventArgs e)
        {
            var curButton = (Button)sender;
            var curFileName = curButton.Name;
            var ret = await downloadService.Download(curFileName);
            if (!ret)
            {
                MessageBox.Show($"Error happened during downloading {curFileName} file");
            }
        }

        private async Task UploadClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                await downloadService.Upload(openFileDialog.FileName);
            }
            await InitializeGrid();
        }

        #region Windows Form Designer generated code

        private async void InitializeComponent()
        {
            view.InitPanel1();

            view.InitGroupBox();

            view.InitPanel2();

            view.groupBox.Controls.Add(view.panel2);

            view.panel1.Controls.Add(new Label() { Text = "Files" });
            view.panel1.Controls.Add(view.groupBox);
            view.panel1.AutoSize = true;
            view.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            view.panel1.Size = new System.Drawing.Size(100,100);
            var uploadButton = new Button() { Text = "Add new file", AutoSize = true };
            uploadButton.Click += new EventHandler(async (s, e) => await UploadClick(s, e));
            view.panel1.Controls.Add(uploadButton);
            this.SuspendLayout();
            this.Controls.Add(view.panel1);
            this.AutoScroll = true;

            await InitializeGrid();

            this.ResumeLayout(true);
        }

        private async Task InitializeGrid()
        {
            await DownloadFileNames();
            view.panel2.Controls.Clear();  

            for (int i = 0; i < fileList.Count; i++)
            {
                view.panel2.RowCount += 1;
                view.panel2.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var label = new Label() { Text = string.Format($"{fileList[i]}") };

                var button = new Button() { Text = "Download File", Name = fileList[i], AutoSize = true };
                button.Click += new EventHandler(async (s, e) => await DownloadClick(s, e));

                view.panel2.Controls.Add(label);

                view.panel2.Controls.Add(button);
                view.panel2.SetRow(label, view.panel2.RowCount); view.panel2.SetColumn(label, 0);
                view.panel2.SetRow(button, view.panel2.RowCount); view.panel2.SetColumn(button, 1);
            }
        }

        #endregion
    }
   
}

