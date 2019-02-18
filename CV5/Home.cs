using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CV5.Tesoreria;

namespace CV5
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.N))
            {

            }
            if (keyData == (Keys.Control | Keys.Q))
            {
                Application.Exit();
            }
            if (keyData == (Keys.Control | Keys.Space))
            {
               
                frmPagoProveedores fr_report= new frmPagoProveedores();
                fr_report.MdiParent = this;
                fr_report.StartPosition = FormStartPosition.CenterScreen;
                fr_report.Show();                
            }
            if (keyData == (Keys.Control | Keys.C))
            {
                Catalogo fr_report = new Catalogo();
                fr_report.MdiParent = this;
                fr_report.StartPosition = FormStartPosition.CenterScreen;
                fr_report.Show();
            }


            return base.ProcessCmdKey(ref msg, keyData);
        }



        private void Home_Load(object sender, EventArgs e)
        {

        }



        private void nuevoUsuarioCtrlNToolStripMenuItem_Click(object sender, EventArgs e)
        {
 
        }


        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Mientras exista la pizza todo estara bien.");
        }



        private void chequesProtestadosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChequesProtestados fr_report = new frmChequesProtestados();
            fr_report.MdiParent = this;
            fr_report.StartPosition = FormStartPosition.CenterScreen;
            fr_report.Show();
        }
    }
}
