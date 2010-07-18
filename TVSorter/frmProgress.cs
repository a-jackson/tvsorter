using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TVSorter
{
    public delegate void Increment();
    public delegate void ProgressError(string error);
    public delegate void Complete();

    public partial class frmProgress : Form
    {
        private bool _created;

        public frmProgress(int max)
        {
            InitializeComponent();
            prgProgress.Maximum = max;
            this.HandleCreated += new EventHandler(frmProgress_HandleCreated);
            _created = false;
        }

        void frmProgress_HandleCreated(object sender, EventArgs e)
        {
            lock (this)
            {
                _created = true;
                Monitor.Pulse(this);
            }
        }

        public void Increment()
        {
            if (prgProgress.InvokeRequired)
            {
                prgProgress.Invoke(new Increment(Increment));
            }
            else
            {
                prgProgress.Value++;
                if (prgProgress.Value == prgProgress.Maximum)
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }

        internal void Abort(string error)
        {
            if (InvokeRequired)
            {
                Invoke(new ProgressError(Abort), error);
            }
            else
            {
                DialogResult = DialogResult.Abort;
                base.Close();
                MessageBox.Show(error);
            }
        }

        public new void Close()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(Close));
            }
            else
            {
                lock (this)
                {
                    if (!this.IsAccessible)
                    {
                        return;
                    }
                    while (!_created)
                    {
                        Monitor.Pulse(this);
                    }
                }
                base.Close();
            }
        }
    }
}
