using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LessSharp.EFHelper
{
    public partial class MigrationSqlForm : Form
    {
        public MainForm MainForm { get; set; }
        public MigrationSqlForm()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            MainForm.CreateSql(txtFrom.Text, txtTo.Text);
            this.Close();
        }

        private void MigrationSqlForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MainForm.MigrationSqlForm = null;
        }
    }
}
