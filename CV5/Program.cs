﻿using System;
using System.Windows.Forms;

namespace CV5
{
    static class Program    
    {
      
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmVentasDiarias());
        }
    }
}
