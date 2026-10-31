using System;
using System.Windows.Forms;
using StudentEmailApp.Forms;

namespace StudentEmailApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the Student Email System application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new frmLogin());
        }
    }
}