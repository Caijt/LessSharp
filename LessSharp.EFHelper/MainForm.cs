using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LessSharp.EFHelper
{
    public partial class MainForm : Form
    {
        private IConfiguration _configuration;

        public MigrationSqlForm MigrationSqlForm { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnSelectProject_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog.SelectedPath = this.txtProject.Text;
            if (!string.IsNullOrWhiteSpace(this.folderBrowserDialog.SelectedPath))
            {
                this.folderBrowserDialog.SelectedPath += "\\";
            }
            DialogResult a = this.folderBrowserDialog.ShowDialog();
            if (a == DialogResult.OK)
            {
                this.txtProject.Text = this.folderBrowserDialog.SelectedPath;
            }
        }

        private string BuildCmd(string cmd)
        {
            string targetProject = string.IsNullOrWhiteSpace(this.txtProject.Text) ? "" : " --project " + this.txtProject.Text;
            string startupProject = string.IsNullOrWhiteSpace(this.txtStartupProject.Text) ? "" : " --startup-project " + this.txtStartupProject.Text;
            string json = chbJson.Checked ? " --json" : "";
            string other = string.IsNullOrWhiteSpace(this.txtOther.Text) ? "" : " " + this.txtOther.Text;
            return cmd + targetProject + startupProject + json + other;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form2 = new MigrationNameForm();
            if (form2.ShowDialog() == DialogResult.OK)
            {
                this.TxtCmd.Text = BuildCmd("dotnet ef migrations add " + (string.IsNullOrWhiteSpace(form2.MigrationName) ? Guid.NewGuid().ToString() : form2.MigrationName));
            }

        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            string cmd = this.TxtCmd.Text.Trim().TrimEnd('&') + "&exit";
            btnRun.Enabled = false;
            toolStripProgressBar1.Visible = true;
            using Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(cmd);
            p.StandardInput.AutoFlush = true;
            string errorResult = await p.StandardError.ReadToEndAsync();
            string result = await ReadFromStreamAsync(p.StandardOutput.BaseStream);
            //删掉输出前4行内容
            if (!string.IsNullOrWhiteSpace(result))
            {
                var lines = result.Split("\r\n").ToList();
                lines.RemoveRange(0, 4);
                result = string.Join("\r\n", lines);
            }
            var item = new CmdResult()
            {
                Cmd = this.TxtCmd.Text,
                Result = "运行命令：\r\n" + this.TxtCmd.Text + "\r\n\r\n运行结果：\r\n" + errorResult + result
            };
            listBox.Items.Add(item);
            listBox.SelectedIndex = listBox.Items.Count - 1;
            p.WaitForExit();
            p.Close();
            TxtCmd.Text = "";
            btnRun.Enabled = true;
            toolStripProgressBar1.Visible = false;
        }
        private async Task<string> ReadFromStreamAsync(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (stream)
                {
                    await stream.CopyToAsync(ms);
                }
                return Encoding.UTF8.GetString(ms.GetBuffer());
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            listBox.Items.Clear();
            rtxtResult.Text = "";
        }

        private void listBox_SelectedValueChanged(object sender, EventArgs e)
        {
            CmdResult a = (CmdResult)listBox.SelectedItem;
            rtxtResult.Text = a.Result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var jsonFile = "config.json";
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile(jsonFile);
            _configuration = builder.Add<WritableJsonConfigurationSource>(
            (Action<WritableJsonConfigurationSource>)(s =>
            {
                s.FileProvider = null;
                s.Path = jsonFile;
                s.Optional = false;
                s.ReloadOnChange = true;
                s.ResolveFileProvider();
            })).Build();
            txtProject.Text = _configuration.GetSection("project").Value;
            txtStartupProject.Text = _configuration.GetSection("startupProject").Value;
            chbJson.Checked = _configuration.GetSection("json").Value == "True";
            txtOther.Text = _configuration.GetSection("other").Value;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _configuration.GetSection("project").Value = txtProject.Text;
            _configuration.GetSection("startupProject").Value = txtStartupProject.Text;
            _configuration.GetSection("json").Value = chbJson.Checked.ToString();
            _configuration.GetSection("other").Value = txtOther.Text;
        }

        private void BtnSelectStartupProject_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog.SelectedPath = this.txtStartupProject.Text;
            if (!string.IsNullOrWhiteSpace(this.folderBrowserDialog.SelectedPath))
            {
                this.folderBrowserDialog.SelectedPath += "\\";
            }
            DialogResult a = this.folderBrowserDialog.ShowDialog();
            if (a == DialogResult.OK)
            {
                this.txtStartupProject.Text = this.folderBrowserDialog.SelectedPath;
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            TxtCmd.Text = BuildCmd("dotnet ef migrations remove --force");
        }

        private void BtnSql_Click(object sender, EventArgs e)
        {
            if (MigrationSqlForm == null)
            {
                MigrationSqlForm = new MigrationSqlForm();
                MigrationSqlForm.MainForm = this;
                MigrationSqlForm.Show();
            }
            else
            {
                MigrationSqlForm.Focus();
            }

        }

        public void CreateSql(string from, string to)
        {
            string fileName = from + "_" + to;
            if (fileName == "_")
            {
                fileName = "all";
            }
            string output = "";
            if (!string.IsNullOrWhiteSpace(this.txtProject.Text))
            {
                output = " --output " + this.txtProject.Text + "\\" + fileName + ".sql";
            }
            TxtCmd.Text = BuildCmd("dotnet ef migrations script " + from + " " + to + output);
        }

        private void BtnList_Click(object sender, EventArgs e)
        {
            TxtCmd.Text = BuildCmd("dotnet ef migrations list");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TxtCmd.Text = BuildCmd("dotnet ef database update");
        }

        private void BtnDelData_Click(object sender, EventArgs e)
        {
            TxtCmd.Text = BuildCmd("dotnet ef database drop --force");
        }

        private void BtnSetup_Click(object sender, EventArgs e)
        {
            TxtCmd.Text = "dotnet tool install --global dotnet-ef --version 3.1.5";
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            TxtCmd.Text = "dotnet tool update--global dotnet-ef--version 3.1.5";
        }

        private void BtnDataInfo_Click(object sender, EventArgs e)
        {
            TxtCmd.Text = BuildCmd("dotnet ef dbcontext info");
        }

        private void BtnDataList_Click(object sender, EventArgs e)
        {
            TxtCmd.Text = BuildCmd("dotnet ef dbcontext list");
        }

        private void btnToolInfo_Click(object sender, EventArgs e)
        {
            TxtCmd.Text = "dotnet ef --info";
        }
    }



}
