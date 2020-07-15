using FolderManager.Data.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FolderManager
{
    partial class Form1
    {
        private string baseUrl = "https://localhost:44307/api/";
        private string fileServiceName = "dokumentumok";

        private OpenFileDialog openFileDialog = new OpenFileDialog();
        private List<string> fileList;

        private HttpClient client = new HttpClient();

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
            try
            {
                var response = await client.GetAsync($"{baseUrl}{fileServiceName}");
                string apiResponse =  await response.Content.ReadAsStringAsync();
                var res  = JsonConvert.DeserializeObject<FileListDto>(apiResponse);
                fileList = res.Files.ToList();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error happened, error message: {ex.Message}");
            }
        }

        private async Task DownloadClick(object sender, EventArgs e)
        {
            var curButton = (Button)sender;
            var curFileName = curButton.Name;
            var response =  await client.GetAsync($"{baseUrl}{fileServiceName}/{curFileName}");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Error happened during downloading {curFileName} file");
                return;
            }
            using (var stream =  response.Content.ReadAsStreamAsync())
            {
                var fileInfo = new FileInfo(curFileName);
                using (var fileStream = fileInfo.Create())
                {
                    await stream.Result.CopyToAsync(fileStream);
                }
            }
        }

        private async Task UploadClick(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var form = new MultipartFormDataContent())
                    {
                        using (var fs = File.OpenRead(openFileDialog.FileName))
                        {
                            using (var streamContent = new StreamContent(fs))
                            {
                                using (var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync()))
                                {
                                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                                    form.Add(fileContent, "file", Path.GetFileName(openFileDialog.FileName));
                                    HttpResponseMessage response =
                                        await client.PostAsync($"{baseUrl}{fileServiceName}", form);
                                    if (!response.IsSuccessStatusCode)
                                    {
                                        MessageBox.Show($"Error happened during uploading {openFileDialog.FileName} file");
                                    }
                                }
                            }
                        }
                    }

                    await InitializeGrid();
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
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

