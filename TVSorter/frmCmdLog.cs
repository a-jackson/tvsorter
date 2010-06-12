using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TVSorter
{
    public partial class frmCmdLog : Form
    {
        public frmCmdLog()
        {
            InitializeComponent();
            Text = "TVSorter " + Program.VersionNumber;
        }

        public void Append(string item)
        {
            lstLog.Items.Add(item);
            lstLog.SelectedIndex = lstLog.Items.Count - 1;
        }
    }
}
