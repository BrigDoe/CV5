using CV5.Tesoreria;
using CV5.Planificacion;
using CV5.Credito;
using System;
using System.Windows.Forms;
using CV5.Roles;
using CV5.Produccion;

namespace CV5
{
    static class Program    
    {
      
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Home());
        }
    }
}
