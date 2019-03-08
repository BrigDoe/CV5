using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using clsConectaMBA;
using CV5.Roles;
using System.Data;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace CV5
{

    public partial class frmMonitorNC : Form
    {

        Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();
        static System.Windows.Forms.Timer Timer1 = new System.Windows.Forms.Timer();
        


        public frmMonitorNC()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
        }




        private void frmPagoProveedores_Load(object sender, EventArgs e)
        {
            DatosVivosNC();
            dataGridView1.Font = new System.Drawing.Font("Segoe UI", 9);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            Timer1.Interval = 300000;
            Timer1.Tick += new EventHandler(Timer1_Tick);

            Timer1.Enabled = true;
        }


        private void Timer1_Tick(object Sender, EventArgs e)
        {
            // Set the caption to the current time.  
            ObtenerFacturas();
        }


        void DatosVivosNC()
        {
            CleanGrid(dataGridView1);
            ObtenerFacturas();
            var columnas = new List<int>();
            columnas.Add(10);
            FormatoGrid(columnas);
            decimal totalValorNeto = dataGridView1.Rows.Cast<DataGridViewRow>()
                .Sum(t => Convert.ToDecimal(t.Cells[10].Value));
            txtValorTotal.Text = String.Format("{0:.##}", totalValorNeto);
        }



        private void btnExcel_Click(object sender, EventArgs e)
        {
            fg.ExcelClick(dataGridView1);
        }

        private void CheckCombo(ComboBox cb, string control)
        {
            if (cb.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor seleccione un valor en " + control, "Informacion",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void CleanGrid(DataGridView dg)
        {
            dg.DataSource = null;
            dg.Refresh();
        }


        private void FormatoGrid(List<int> list)
        {
            foreach (int cols in list)
            {
                dataGridView1.Columns[cols].DefaultCellStyle.Format = "N2";
            }
        }



        public void StopBarra()
        {
            pgb.Visible = false;
        }


        void ObtenerFacturas()
        {
                ConexionMba cs = new ConexionMba();           
                string cadena = "SELECT CCP.EMPRESA `EMPRESA`, FP.ORIGEN `SUCURSAL` ,CASE CAST(CCP.ANULADO AS INT) WHEN 0 THEN 'ACTIVO' WHEN 1 THEN 'ANULADO' END AS VALIDEZ ," +
                    " DATE_TO_CHAR(FP.FECHA_FACTURA, 'dd[/]mm[/]yyyy') AS `FECHA FACTURA`," +
                    " CCD.CODIGO_FACTURA  `FACTURA`, CCD.CODIGO_COBRO, " +
                    " CFP.NOMBRE_CLIENTE `CLIENTE`, DATE_TO_CHAR(CCP.FECHA_COBRO, 'dd[/]mm[/]yyyy') AS `FECHA PAGO`," +
                    " DATE_TO_CHAR(CCP.FECHA_COBRO, 'dd[/]mm[/]yyyy') AS `FECHA DOCUMENTO`, " +
                    " DATE_TO_CHAR(FP.FECHA_VENCIMIENTO, 'dd[/]mm[/]yyyy') AS `FECHA VENCIMIENTO` ," +
                    " CCP.VALOR_COBRO `VALOR COBRO`, CCP.CHEQUE_NUMERO `NUMERO DOCUMENTO`, CCP.ID_FISCAL `CUENTA BANCARIA`," +
                    " CBFP.FECHA_CHEQUE_O_EXPIRA_TC `FECHA CHEQUE`, CCP.FORMA_DE_PAGO `FORMA DE PAGO`," +
                    " CCP.CODIGO_COBRADOR `CODIGO COBRADOR`, CCP.INFO_CREACION `INFO CREACION`, CCP.INFO_CONFIRMACION `INFO CONFIRMACION` " +
                    " FROM CLNT_FICHA_PRINCIPAL CFP, CLNT_COBRO_DETALLE CCD," +
                    " CLNT_COBRO_PRINCIPAL CCP,CLNT_FACTURA_PRINCIPAL FP, CLNT_COBRO_FORMADEPAGO CBFP" +
                    " WHERE " +
                    " CCD.CODIGO_COBRO = CCP.CODIGO_COBRO AND " +
                    " CFP.CODIGO_CLIENTE_EMPRESA = CCD.CODIGO_CLIENTE AND " +
                    " CCD.CODIGO_FACTURA = FP.CODIGO_FACTURA  AND " +
                    " CBFP.CODIGO_COBRO = CCD.CODIGO_COBRO AND" +
                    " CCP.CODIGO_COBRADOR = '' AND " +
                    " CCP.FECHA_COBRO  >= '28/02/2019' ";
                fg.FillDataGrid(cadena, dataGridView1);
            
        }

        
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            fg.LimpiarGrid(dataGridView1);
            DatosVivosNC();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            DatosVivosNC();
        }

        private void txtValorTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            notifyIcon1.Text = "CV5";
            notifyIcon1.BalloonTipText = "Existen " + dataGridView1.ColumnCount.ToString()
                + " registros sin cobrador, favor reversarlos. ";
            notifyIcon1.ShowBalloonTip(10000);
           
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }
    }
}
