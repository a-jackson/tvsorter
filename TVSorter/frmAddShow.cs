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
    public partial class frmAddShow : Form
    {
        public TVShow newShow;

        public frmAddShow()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int showid = int.Parse(txtTvdbId.Text);
                newShow = TVDB.GetShow(showid);
                if (newShow == null)
                    return;
                txtShowName.Text = newShow.Name;
                txtFolderName.Text = newShow.FolderName;
            }
            catch { }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (newShow == null)
            {
                MessageBox.Show("No new show found");
            }
            else
            {
                newShow.FolderName = txtFolderName.Text;
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            this.Close();
        }
    }
}
