using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;

namespace CV5
{
    public partial class frmPagoProveedores : Form
    {
        OdbcConnection DbConnection = new OdbcConnection("Dsn=MBA3 PRUEBA12;Driver={4D v12 ODBC Driver};server=192.168.1.2;port=19819;");
        Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();

        public frmPagoProveedores()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            string query = "SELECT  `CORPORATION NAM` FROM SIST_Parametros_Empresa ";
            fg.LlenarCombo(query, cmbEmpresa);
            query = "SELECT DISTINCT VENDOR_NAME FROM PROV_Ficha_Principal";
            fg.LlenarCombo(query, cmbAcreedor,-1);
           
        }

        private void frmPagoProveedores_Load(object sender, EventArgs e)
        {
            cmbAcreedor.Enabled = true;
            dtpFechAct.Enabled = true;
            dtpFechFin.Enabled = true;
            btnOk.Enabled = true;
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

        private void CleanGrid(DataGridView dg) {
            dg.DataSource=null;
            dg.Refresh();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //flag para chequear si existen un Acreedor en particular
            CleanGrid(dataGridView1);
            Boolean flag;
            if (!fg.CheckDatePicker(dtpFechAct, dtpFechFin))
            {
                string Fech1 = dtpFechAct.Value.ToString("dd/MM/yyyy");
                string Fech2 = dtpFechFin.Value.ToString("dd/MM/yyyy");
                string CORP = "SELECT CORP FROM SIST_Parametros_Empresa  WHERE `CORPORATION NAM`= '" + cmbEmpresa.Text + "' ";
                DbConnection.Open();
                OdbcCommand DbCommand = new OdbcCommand(CORP, DbConnection);
                OdbcDataReader reader = DbCommand.ExecuteReader();
                string _CORP="";
                string _Acree = "";
                while (reader.Read()) { 
                    _CORP = reader.GetString(0);
                }
                DbConnection.Close();
                if (!chkAllProv.Checked)
                {
                    string Acree = "SELECT CODIGO_PROVEEDOR_EMPRESA FROM PROV_FICHA_PRINCIPAL" +
                                    " WHERE VENDOR_NAME ='" + cmbAcreedor.Text + "'" +
                                    " AND CODIGO_PROVEEDOR_EMPRESA LIKE '%" + _CORP + "'";
                    DbConnection.Open();
                    DbCommand = new OdbcCommand(Acree, DbConnection);
                    reader = DbCommand.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        _Acree = reader.GetString(0);
                    }
                    DbConnection.Close();
                    flag = true;
                } else
                {
                    flag = false;
                }
                string cadena = "SELECT pfp.VENDOR_NAME AS Proveedor , TRIM(' DIAS' FROM pfp.`TERMS ALFA`) AS `Dias Credito`, " +
                "pcu.`VEND INV REF` AS Factura, DATE_TO_CHAR(fp.INVOICE_DATE, 'dd[/]mm[/]yyyy') AS `Fecha factura`," +
                "(fp.`RETENTION BASIS` + 0.00) AS Subtotal, fp.`AMOUNT TAX2` AS Total_CR_Tributario, fp.INVOICE_TOTAL AS" +
                " TOTAL_FACTURA, pcu.`PAYMENT AMOUNT` AS TOTAL_Pagos, pcu.`PAYMENT AMOUNT` AS TOTAL_Ncprv_Ndprv," +
                " pcu.`DISCOUNT AMOUNT` AS Total_dcto, pcu.`RETENTION AMNT` AS Total_Retenciones," +
                " pcu.`AMOUNT DUE` AS Saldos_Actual, pcu.`CHECK NUMBER` AS Chq " +
                "FROM PROV_Cobros_Cuotas pcu, PROV_Factura_Principal fp, PROV_Ficha_Principal pfp " +
                "WHERE PROV_Cobros_Cuotas.`VEND INV REF` = fp.DOC_REFERENCE AND fp.VENDOR_ID_CORP = pfp.CODIGO_PROVEEDOR_EMPRESA " +
                "AND (PROV_Cobros_Cuotas.CORP = '" +  _CORP +  "') AND " +
                "fp.VOID = cast('False' as Boolean) and fp.INVOICE_DATE >= '"+  Fech1 + "' and fp.INVOICE_DATE <= '" + Fech2 + "'";
                if (flag)
                    cadena+=" AND fp.VENDOR_ID_CORP = '" + _Acree + "'";
                fg.FillDataGrid(cadena, dataGridView1, DbConnection);
            }

        
            return;
    
        }

        

        private void cmbAcreedor_Leave(object sender, EventArgs e)
        {
            CheckCombo(cmbAcreedor, "empresa");
        }

        private void cmbEmpresa_Leave(object sender, EventArgs e)
        {
            CheckCombo(cmbEmpresa, "empresa");
        }

        private void dtpFechFin_Leave(object sender, EventArgs e)
        {
            fg.CheckDatePicker(dtpFechAct, dtpFechFin);
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllProv.Checked)
            {
                cmbAcreedor.Enabled = false;
                btnSearch.Enabled = false;
            } else
            {
                cmbAcreedor.Enabled = true;
                btnSearch.Enabled = true;
            }
        }

        private void cmbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void btnPrint_Click(object sender, EventArgs e)
        {
            // SE GENERA EL PDF CON LA CLASE REPORTE
            Reporte R = new Reporte();
            // ES HORIZONTAL
            Document doc = R.CreaDoc(true);
            //Se genera fuente
            Font font = R.Fuente();
            //Generar un writer para el reporte
            var writer = R.CreaWriter(doc);
            //Inicia la apertura del documento y escritura
            R.Iniciar(doc);
            //Titulo
            R.Titulo(doc, "Reportes de Cuentas por pagar");
            Image img = R.Imagen();
            R.SetImagen(img, doc);
            // Lista de encabezados para reporte
            R.CreaReport(dataGridView1,font,doc,writer);   
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dataGridView1.Columns[e.ColumnIndex].Index == 4 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index ==  5 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 6 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 7 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 8 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 9 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 10 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 11)
            {
                if (e.Value != null)
                {
                    ConvertirFloat(e);
                }
            }
        }


        //SE REALIZA EL FORMATEO PARA DECIMALES 
        private void ConvertirFloat(DataGridViewCellFormattingEventArgs formatting)
        {
            if (formatting.Value != null)
            {
                try
                {
                    decimal e;
                    e = decimal.Parse(formatting.Value.ToString());
                    // Convierte a decimales
                    formatting.Value = e.ToString("N2");
                }
                catch (FormatException)
                {
                    formatting.FormattingApplied = false;
                  
                }
            }
        }

        
    }
}
