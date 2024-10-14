using System;
using System.Windows.Forms;

namespace MercuryGUI
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Instantiate the updater and check for updates
            MultiRepoUpdater updater = new MultiRepoUpdater();
            updater.CheckAndUpdateRepos();

            // Run your main form
            Application.Run(new Form1()); // Ensure Form1 is your main form class
        }
    }
}
