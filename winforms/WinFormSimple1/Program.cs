using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormSimple1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Environment.SetEnvironmentVariable("FONTCONFIG_PATH", @"d:\projects\mono-winforms-netcore\etc\");
            //Environment.SetEnvironmentVariable("FC_DEBUG", "15");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
