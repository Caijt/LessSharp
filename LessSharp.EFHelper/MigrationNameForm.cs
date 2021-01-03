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
    public partial class MigrationNameForm : Form
    {
        public string MigrationName { get; set; }
        public MigrationNameForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.MigrationName = txtFileName.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}
