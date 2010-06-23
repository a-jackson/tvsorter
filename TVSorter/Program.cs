using System;
using System.Windows.Forms;

namespace TVSorter
{
    public enum RunMode { Gui, Cli };
    static class Program
    {
        public static string VersionNumber = "0.3";
        public static RunMode CurrentMode;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {                
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length == 0)
            {
                CurrentMode = RunMode.Gui;
                Application.Run(new frmMain());
            }
            else
            {
                CurrentMode = RunMode.Cli;
                frmCmdLog log = new frmCmdLog();
                new CommandLine(args, log);
                Application.Run(log);
            }
        }
    }
}
