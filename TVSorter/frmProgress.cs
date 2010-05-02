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
            prgProgress.Value++;
            if (prgProgress.Value == prgProgress.Maximum)
            {
                DialogResult = DialogResult.OK;
            }
        }

        internal void Abort()
        {
            MethodInvoker mi = new MethodInvoker(delegate() { 
                DialogResult = DialogResult.Abort; 
                base.Close(); 
            });
            this.Invoke(mi);
        }

        public new void Close()
        {
            MethodInvoker mi = new MethodInvoker(delegate() {
                base.Close(); 
            });
            lock (this)
            {
                if (!this.IsAccessible)
                {
                    return;
                }
                while (!_created)
                {
                    Monitor.Wait(this);
                }
            }
            this.BeginInvoke(mi);
        }
    }
}
