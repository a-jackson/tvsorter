// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Log.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The log tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TVSorter.Controller;

namespace TVSorter.View
{
    /// <summary>
    ///     The log tab.
    /// </summary>
    public partial class Log : UserControl
    {
        /// <summary>
        ///     The log controller.
        /// </summary>
        private readonly LogController controller;

        /// <summary>
        ///     Initialises a new instance of the <see cref="Log" /> class.
        /// </summary>
        public Log()
        {
            InitializeComponent();
            controller = CompositionRoot.Get<LogController>();
        }

        /// <summary>
        ///     Handles a log message being received.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        public void LogMessageReceived(object sender, LogMessageEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => LogMessageReceived(sender, e)));
            }
            else
            {
                logList.Items.Add(e);
                logList.SelectedIndex = logList.Items.Count - 1;
            }
        }

        /// <summary>
        ///     Handles the Log tab loading.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void LogLoad(object sender, EventArgs e)
        {
            controller.Initialise(null);
            controller.LogMessageReceived += LogMessageReceived;
            logList.Items.AddRange(controller.Log.ToArray<object>());
        }

        /// <summary>
        ///     Handles drawing a list item.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void LogListDrawItem(object sender, DrawItemEventArgs e)
        {
            var log = (LogMessageEventArgs)logList.Items[e.Index];

            e.DrawBackground();
            var g = e.Graphics;

            if (log.Type == LogType.Error)
            {
                g.FillRectangle(new SolidBrush(Color.Red), e.Bounds);
            }

            g.DrawString(
                logList.Items[e.Index].ToString(),
                e.Font,
                new SolidBrush(e.ForeColor),
                new PointF(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }
    }
}
