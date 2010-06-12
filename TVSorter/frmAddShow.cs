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
        private List<TVShow> results;

        public frmAddShow()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string showName = txtName.Text;
                results = TVDB.SearchShow(showName);
                foreach (TVShow show in results)
                    lstResults.Items.Add(
                        new ListViewItem(new string[] { show.Name, show.TvdbId.ToString() }));
            }
            catch { }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (lstResults.SelectedItems.Count == 0)
            {
                MessageBox.Show("You must select a show");
                return;
            }
            newShow = results[lstResults.SelectedIndices[0]];
            if (newShow == null)
            {
                MessageBox.Show("No new show found");
            }
            else
            {
                newShow.FolderName = txtName.Text;
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
