using System;
using System.Windows.Forms;


//https://www.caveofprogramming.com/c-sharp-tutorial/c-for-beginners-make-your-own-mp3-player-free.html
//http://stackoverflow.com/questions/1708239/how-can-i-load-a-folders-files-into-a-listview
//http://www.codeproject.com/Articles/63094/Simple-MCI-Player


namespace osuP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
