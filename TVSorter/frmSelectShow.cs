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
    public partial class frmSelectShow : Form
    {
        private TVShow _selectedShow;
        private object[] _shows;
        public frmSelectShow(ListBox.ObjectCollection shows)
        {
            InitializeComponent();
            _shows = new object[shows.Count];
            shows.CopyTo(_shows, 0);
        }

        private void frmSelectShow_Load(object sender, EventArgs e)
        {
            lstTvShows.Items.AddRange(_shows);
        }

        public TVShow SelectedShow
        {
            get { return _selectedShow; }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            _selectedShow = (TVShow)lstTvShows.SelectedItem;
            if (_selectedShow == null)
            {
                return;
            }
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            this.Close();
        }
    }
}
