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
        private OpenFileDialog openFileDialog1 = new OpenFileDialog();
        private List<string> fileList;

        private HttpClient client = new HttpClient();

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
                var response = await client.GetAsync("https://localhost:44307/api/dokumentumok");
                string apiResponse =  await response.Content.ReadAsStringAsync();
                var res  = JsonConvert.DeserializeObject<FileListDto>(apiResponse);
                fileList = res.Files.ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine();
            }
        }

        private async Task DownloadClick(object sender, EventArgs e)
        {
            var curButton = (Button)sender;
            var curFileName = curButton.Name;
            var response =  await client.GetAsync($"https://localhost:44307/api/dokumentumok/{curFileName}");
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
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //var sr = new StreamReader(openFileDialog1.FileName);

                    using (var form = new MultipartFormDataContent())
                    {
                        using (var fs = File.OpenRead(openFileDialog1.FileName))
                        {
                            using (var streamContent = new StreamContent(fs))
                            {
                                using (var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync()))
                                {
                                    fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                                    // "file" parameter name should be the same as the server side input parameter name
                                    form.Add(fileContent, "file", Path.GetFileName(openFileDialog1.FileName));
                                    HttpResponseMessage response = await client.PostAsync("https://localhost:44307/api/dokumentumok", form);
                                }
                            }
                        }
                    }
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
            client.BaseAddress = new Uri("http://localhost:44307/");
            await DownloadFileNames();

            //var panel1 = new TableLayoutPanel() { };
            //panel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            //panel1.RowCount = 2;
            //panel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            //panel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            //panel1.AutoSize = true;
            //panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            //var groupBox1 = new GroupBox() { Text = "GroupBox" };
            //groupBox1.AutoSize = true;
            //groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            //var panel2 = new TableLayoutPanel() { Top = 24, Left = 5 };
            //panel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            //panel2.AutoSize = true;
            //panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            //groupBox1.Controls.Add(panel2);

            //panel1.Controls.Add(new Label() { Text = "Label" });
            //panel1.Controls.Add(groupBox1);
            ////var uploadButton = new Button() { Text = "Add new file" };
            ////uploadButton.Click += new EventHandler(async (s, e) => await UploadClick(s, e));
            //panel1.Controls.Add(new Button() { Text = "Add new file" });

            //this.AutoScroll = true;
            //for (int i = 0; i < fileList.Count; i++)
            //{
            //    panel2.RowCount += 1;
            //    panel2.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            //    panel2.Controls.Add(new GroupBox()
            //    {
            //        Text = string.Format($"{fileList[i]}")
            //    });
            //    var button = new Button() { Text = "Download File", Name = fileList[i] };
            //    button.Click += new EventHandler(async (s, e) => await DownloadClick(s,e));
            //    panel2.Controls.Add(button);
            //}
            //this.ResumeLayout(true);

            var panel1 = new TableLayoutPanel();
            panel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            panel1.RowCount = 2;
            panel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            panel1.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel1.AutoSize = true;
            panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            var groupBox1 = new GroupBox() { Text = "GroupBox" };
            groupBox1.AutoSize = true;
            groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            var panel2 = new TableLayoutPanel() { Top = 24, Left = 5 };
            panel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            panel2.AutoSize = true;
            panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            groupBox1.Controls.Add(panel2);

            panel1.Controls.Add(new Label() { Text = "Label" });
            panel1.Controls.Add(groupBox1);
            var uploadButton = new Button() { Text = "Add new file" };
            uploadButton.Click += new EventHandler(async (s, e) => await UploadClick(s, e));
            panel1.Controls.Add(uploadButton);
            this.SuspendLayout();
            this.Controls.Add(panel1);
            this.AutoScroll = true;
            for (int i = 0; i < fileList.Count; i++)
            {
                panel2.RowCount += 1;
                panel2.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                panel2.Controls.Add(new GroupBox()
                {
                    Text = string.Format($"{fileList[i]}")
                });
                var button = new Button() { Text = "Download File", Name = fileList[i] };
                button.Click += new EventHandler(async (s, e) => await DownloadClick(s, e));
                panel2.Controls.Add(button);
            }
            this.ResumeLayout(true);

        }

        #endregion
    }
   
}

