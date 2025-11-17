using System;
using System.Windows.Forms;
using System.Drawing;
namespace Graphing
{
    static class Program
    {

        public static MainForm   f ;

        [STAThread]
        static void Main() // Entry point
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            f = new MainForm();
            Application.Run(f);
        }

 
    }
}
