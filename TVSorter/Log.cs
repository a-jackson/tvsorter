using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TVSorter
{
    class Log
    {
        private static ListBox _listBox;
        private static bool logToGui;
        private static frmCmdLog _cmdLog;
        private static List<string> _log;
        private static FileInfo _saveFile;

        public static void Init(ListBox listBox)
        {
            _listBox = listBox;
            logToGui = true;
            _log = new List<string>();
            _saveFile = null;
        }

        public static void Init(frmCmdLog cmdLog)
        {
            logToGui = false;
            _cmdLog = cmdLog;
            _log = new List<string>();
            _saveFile = null;
        }


        public static void Add(string message)
        {
            string entry = DateTime.Now.ToLongTimeString() + " - " + message;
            _log.Add(entry);
            if (logToGui)
            {
                MethodInvoker log = new MethodInvoker(delegate() { _listBox.Items.Add(entry); });
                _listBox.Invoke(log);
            }
            else
            {
                MethodInvoker log = new MethodInvoker(delegate() { _cmdLog.Append(entry); });
                _cmdLog.Invoke(log);
            }
        }

        public static void SetSaveLocation(FileInfo file)
        {
            _saveFile = file;
        }

        public static void Save()
        {
            if (_saveFile == null)
            {
                return;
            }
            try
            {
                StreamWriter stream = _saveFile.CreateText();
                foreach (string line in _log)
                {
                    stream.WriteLine(line);
                }
                stream.Flush();
                stream.Close();
            }
            catch (Exception e)
            {
                Log.Add("Error saving log file to " + _saveFile.FullName);
                Log.Add(e.Message);
            }
        }
    }
}
