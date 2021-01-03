namespace LessSharp.EFHelper
{
    partial class MainForm
    {
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtxtResult = new System.Windows.Forms.RichTextBox();
            this.TxtCmd = new System.Windows.Forms.TextBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.txtProject = new System.Windows.Forms.TextBox();
            this.btnSelectProject = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnToolInfo = new System.Windows.Forms.Button();
            this.btnUpdateTool = new System.Windows.Forms.Button();
            this.btnDbContextInfo = new System.Windows.Forms.Button();
            this.btnSql = new System.Windows.Forms.Button();
            this.btnDbContextList = new System.Windows.Forms.Button();
            this.btnList = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.BtnDelData = new System.Windows.Forms.Button();
            this.btnSetupTool = new System.Windows.Forms.Button();
            this.btnUpdateData = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.listBox = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtOther = new System.Windows.Forms.TextBox();
            this.btnSelectStartupProject = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.chbJson = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtStartupProject = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtxtResult
            // 
            this.rtxtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtResult.Location = new System.Drawing.Point(0, 0);
            this.rtxtResult.Name = "rtxtResult";
            this.rtxtResult.ReadOnly = true;
            this.rtxtResult.Size = new System.Drawing.Size(538, 276);
            this.rtxtResult.TabIndex = 1;
            this.rtxtResult.Text = "";
            // 
            // TxtCmd
            // 
            this.TxtCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtCmd.Location = new System.Drawing.Point(52, 28);
            this.TxtCmd.Name = "TxtCmd";
            this.TxtCmd.Size = new System.Drawing.Size(524, 23);
            this.TxtCmd.TabIndex = 2;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRun.Location = new System.Drawing.Point(580, 28);
            this.btnRun.Margin = new System.Windows.Forms.Padding(1);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(55, 23);
            this.btnRun.TabIndex = 3;
            this.btnRun.Text = "运行";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // txtProject
            // 
            this.txtProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProject.Location = new System.Drawing.Point(76, 31);
            this.txtProject.Name = "txtProject";
            this.txtProject.PlaceholderText = "DbContext派生类所在项目";
            this.txtProject.Size = new System.Drawing.Size(188, 23);
            this.txtProject.TabIndex = 4;
            // 
            // btnSelectProject
            // 
            this.btnSelectProject.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSelectProject.Location = new System.Drawing.Point(267, 31);
            this.btnSelectProject.Margin = new System.Windows.Forms.Padding(1);
            this.btnSelectProject.Name = "btnSelectProject";
            this.btnSelectProject.Size = new System.Drawing.Size(44, 23);
            this.btnSelectProject.TabIndex = 5;
            this.btnSelectProject.Text = "选择";
            this.btnSelectProject.UseVisualStyleBackColor = true;
            this.btnSelectProject.Click += new System.EventHandler(this.BtnSelectProject_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "命令";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(15, 31);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(90, 23);
            this.btnAdd.TabIndex = 11;
            this.btnAdd.Text = "新的迁移";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnToolInfo);
            this.groupBox1.Controls.Add(this.btnUpdateTool);
            this.groupBox1.Controls.Add(this.btnDbContextInfo);
            this.groupBox1.Controls.Add(this.btnSql);
            this.groupBox1.Controls.Add(this.btnDbContextList);
            this.groupBox1.Controls.Add(this.btnList);
            this.groupBox1.Controls.Add(this.btnRemove);
            this.groupBox1.Controls.Add(this.BtnDelData);
            this.groupBox1.Controls.Add(this.btnSetupTool);
            this.groupBox1.Controls.Add(this.btnUpdateData);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Location = new System.Drawing.Point(338, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(395, 150);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "功能";
            // 
            // btnToolInfo
            // 
            this.btnToolInfo.Location = new System.Drawing.Point(15, 109);
            this.btnToolInfo.Margin = new System.Windows.Forms.Padding(1);
            this.btnToolInfo.Name = "btnToolInfo";
            this.btnToolInfo.Size = new System.Drawing.Size(95, 23);
            this.btnToolInfo.TabIndex = 21;
            this.btnToolInfo.Text = "dotnet-ef信息";
            this.btnToolInfo.UseVisualStyleBackColor = true;
            this.btnToolInfo.Click += new System.EventHandler(this.btnToolInfo_Click);
            // 
            // btnUpdateTool
            // 
            this.btnUpdateTool.Location = new System.Drawing.Point(209, 109);
            this.btnUpdateTool.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnUpdateTool.Name = "btnUpdateTool";
            this.btnUpdateTool.Size = new System.Drawing.Size(95, 23);
            this.btnUpdateTool.TabIndex = 20;
            this.btnUpdateTool.Text = "更新dotnet-ef";
            this.btnUpdateTool.UseVisualStyleBackColor = true;
            this.btnUpdateTool.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // btnDbContextInfo
            // 
            this.btnDbContextInfo.Location = new System.Drawing.Point(198, 60);
            this.btnDbContextInfo.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnDbContextInfo.Name = "btnDbContextInfo";
            this.btnDbContextInfo.Size = new System.Drawing.Size(90, 23);
            this.btnDbContextInfo.TabIndex = 19;
            this.btnDbContextInfo.Text = "上下文详情";
            this.btnDbContextInfo.UseVisualStyleBackColor = true;
            this.btnDbContextInfo.Click += new System.EventHandler(this.BtnDataInfo_Click);
            // 
            // btnSql
            // 
            this.btnSql.Location = new System.Drawing.Point(290, 31);
            this.btnSql.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnSql.Name = "btnSql";
            this.btnSql.Size = new System.Drawing.Size(90, 23);
            this.btnSql.TabIndex = 18;
            this.btnSql.Text = "生成迁移SQL";
            this.btnSql.UseVisualStyleBackColor = true;
            this.btnSql.Click += new System.EventHandler(this.BtnSql_Click);
            // 
            // btnDbContextList
            // 
            this.btnDbContextList.Location = new System.Drawing.Point(290, 60);
            this.btnDbContextList.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnDbContextList.Name = "btnDbContextList";
            this.btnDbContextList.Size = new System.Drawing.Size(90, 23);
            this.btnDbContextList.TabIndex = 17;
            this.btnDbContextList.Text = "上下文列表";
            this.btnDbContextList.UseVisualStyleBackColor = true;
            this.btnDbContextList.Click += new System.EventHandler(this.BtnDataList_Click);
            // 
            // btnList
            // 
            this.btnList.Location = new System.Drawing.Point(198, 31);
            this.btnList.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnList.Name = "btnList";
            this.btnList.Size = new System.Drawing.Size(90, 23);
            this.btnList.TabIndex = 16;
            this.btnList.Text = "迁移列表";
            this.btnList.UseVisualStyleBackColor = true;
            this.btnList.Click += new System.EventHandler(this.BtnList_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(107, 31);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(90, 23);
            this.btnRemove.TabIndex = 15;
            this.btnRemove.Text = "删除最近迁移";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // BtnDelData
            // 
            this.BtnDelData.Location = new System.Drawing.Point(107, 60);
            this.BtnDelData.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.BtnDelData.Name = "BtnDelData";
            this.BtnDelData.Size = new System.Drawing.Size(90, 23);
            this.BtnDelData.TabIndex = 14;
            this.BtnDelData.Text = "删除数据库";
            this.BtnDelData.UseVisualStyleBackColor = true;
            this.BtnDelData.Click += new System.EventHandler(this.BtnDelData_Click);
            // 
            // btnSetupTool
            // 
            this.btnSetupTool.Location = new System.Drawing.Point(112, 109);
            this.btnSetupTool.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnSetupTool.Name = "btnSetupTool";
            this.btnSetupTool.Size = new System.Drawing.Size(95, 23);
            this.btnSetupTool.TabIndex = 13;
            this.btnSetupTool.Text = "安装dotnet-ef";
            this.btnSetupTool.UseVisualStyleBackColor = true;
            this.btnSetupTool.Click += new System.EventHandler(this.BtnSetup_Click);
            // 
            // btnUpdateData
            // 
            this.btnUpdateData.Location = new System.Drawing.Point(15, 60);
            this.btnUpdateData.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnUpdateData.Name = "btnUpdateData";
            this.btnUpdateData.Size = new System.Drawing.Size(90, 23);
            this.btnUpdateData.TabIndex = 12;
            this.btnUpdateData.Text = "更新数据库";
            this.btnUpdateData.UseVisualStyleBackColor = true;
            this.btnUpdateData.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(637, 28);
            this.btnClear.Margin = new System.Windows.Forms.Padding(1);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(67, 23);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "清空记录";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // listBox
            // 
            this.listBox.DisplayMember = "cmd";
            this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 17;
            this.listBox.Location = new System.Drawing.Point(0, 0);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(148, 276);
            this.listBox.TabIndex = 14;
            this.listBox.SelectedValueChanged += new System.EventHandler(this.listBox_SelectedValueChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(14, 58);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox);
            this.splitContainer1.Panel1MinSize = 10;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rtxtResult);
            this.splitContainer1.Size = new System.Drawing.Size(690, 276);
            this.splitContainer1.SplitterDistance = 148;
            this.splitContainer1.TabIndex = 16;
            this.splitContainer1.Text = "splitContainer1";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtOther);
            this.groupBox2.Controls.Add(this.btnSelectStartupProject);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.chbJson);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtStartupProject);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtProject);
            this.groupBox2.Controls.Add(this.btnSelectProject);
            this.groupBox2.Location = new System.Drawing.Point(14, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(318, 150);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "配置";
            // 
            // txtOther
            // 
            this.txtOther.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOther.Location = new System.Drawing.Point(76, 109);
            this.txtOther.Name = "txtOther";
            this.txtOther.Size = new System.Drawing.Size(188, 23);
            this.txtOther.TabIndex = 14;
            // 
            // btnSelectStartupProject
            // 
            this.btnSelectStartupProject.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSelectStartupProject.Location = new System.Drawing.Point(268, 60);
            this.btnSelectStartupProject.Margin = new System.Windows.Forms.Padding(1);
            this.btnSelectStartupProject.Name = "btnSelectStartupProject";
            this.btnSelectStartupProject.Size = new System.Drawing.Size(43, 23);
            this.btnSelectStartupProject.TabIndex = 13;
            this.btnSelectStartupProject.Text = "选择";
            this.btnSelectStartupProject.UseVisualStyleBackColor = true;
            this.btnSelectStartupProject.Click += new System.EventHandler(this.BtnSelectStartupProject_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "其它参数";
            // 
            // chbJson
            // 
            this.chbJson.AutoSize = true;
            this.chbJson.Location = new System.Drawing.Point(76, 89);
            this.chbJson.Name = "chbJson";
            this.chbJson.Size = new System.Drawing.Size(15, 14);
            this.chbJson.TabIndex = 11;
            this.chbJson.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "JSON输出";
            // 
            // txtStartupProject
            // 
            this.txtStartupProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStartupProject.Location = new System.Drawing.Point(76, 60);
            this.txtStartupProject.Name = "txtStartupProject";
            this.txtStartupProject.Size = new System.Drawing.Size(188, 23);
            this.txtStartupProject.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "启动项目";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "目标项目";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.TxtCmd);
            this.groupBox3.Controls.Add(this.splitContainer1);
            this.groupBox3.Controls.Add(this.btnClear);
            this.groupBox3.Controls.Add(this.btnRun);
            this.groupBox3.Location = new System.Drawing.Point(14, 165);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox3.Size = new System.Drawing.Size(719, 348);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "命令运行";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 523);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(748, 22);
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(733, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "LessSharp EF助手";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(748, 545);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "LessSharp EF助手";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox rtxtResult;
        private System.Windows.Forms.TextBox TxtCmd;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.TextBox txtProject;
        private System.Windows.Forms.Button btnSelectProject;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnUpdateData;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button BtnDelData;
        private System.Windows.Forms.Button btnSetupTool;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chbJson;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtStartupProject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectStartupProject;
        private System.Windows.Forms.TextBox txtOther;
        private System.Windows.Forms.Button btnList;
        private System.Windows.Forms.Button btnSql;
        private System.Windows.Forms.Button btnDbContextList;
        private System.Windows.Forms.Button btnDbContextInfo;
        private System.Windows.Forms.Button btnUpdateTool;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnToolInfo;
    }
}

