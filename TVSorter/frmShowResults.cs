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
    public partial class frmShowResults : Form
    {
        List<TVShow> _results;
        private string _show;
        TVShow _selectedItem;

        public frmShowResults(List<TVShow> results, string show)
        {
            InitializeComponent();
            _results = results;
            _show = show;
        }

        private void frmShowResults_Load(object sender, EventArgs e)
        {
            lblResults.Text = "Results for show: " + _show;
            foreach (TVShow show in _results)
            {
                lstResults.Items.Add(new ListViewItem(new string[] { show.Name, show.TvdbId.ToString() }));
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(txtTvdbId.Text);
                TVShow show = TVDB.Instance.GetShow(id);
                lstResults.Items.Clear();
                _results.Clear();
                _results.Add(show);
                lstResults.Items.Add(new ListViewItem(new string[] { show.Name, id.ToString() }));
                lstResults.View = View.Details;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured " + ex.Message);
            }
        }

        internal TVShow GetSelected()
        {
            return _selectedItem;
        }

        private void btnUseSelected_Click(object sender, EventArgs e)
        {
            if (lstResults.SelectedItems.Count == 0)
            {
                MessageBox.Show("Nothing selected");
                return;
            }
                _selectedItem = _results[lstResults.SelectedIndices[0]];
            this.Close();
            this.Dispose();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            _selectedItem = null;
            this.Close();
            this.Dispose();
        }
    }
}
