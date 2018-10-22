using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using iTextSharp.text.pdf;

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

        private void btnOk_Click(object sender, EventArgs e)
        {
            //flag para chequear si existen un Acreedor en particular
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
                string cadena = "SELECT pfp.VENDOR_NAME AS Proveedor , pfp.`TERMS ALFA` AS `Dias Credito`, " +
                "pcu.`VEND INV REF` AS Factura, DATE_TO_CHAR(fp.INVOICE_DATE, 'dd[/]mm[/]yyyy') AS `Fecha factura`," +
                "fp.`RETENTION BASIS` AS Subtotal, fp.`AMOUNT TAX2` AS Total_CR_Tributario, fp.INVOICE_TOTAL AS" +
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


        private List<string> Encabezados()
        {
            List<string> lista1 = new List<string>()
            {
                "Proveedor",
                "Dias cred",
                "No. Fac.",
                "Fecha factura",
                "Subtotal factura",
                "Total Trib",
                "Total factura",
                "Total Pagos",
                "Total Nc/Db",
                "Total dcto",
                "Total Retenciones",
                "Saldos actual",
                "Cheque"
            };
            return lista1;
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            //
            Reporte R = new Reporte();
            Document doc = R.CreaDoc(true);
            iTextSharp.text.Font font = R.Fuente();
            PdfWriter writer = R.CreaWriter(doc);
            R.Iniciar(doc);
            R.Titulo(doc, "Reportes de Cuentas por pagar");
            iTextSharp.text.Image img = R.Imagen();
            R.SetImagen(img, doc);
            // Lista de encabezados para reporte
            List<string> lista1 = Encabezados();

            // Se crea objetos en base a la lista de encabezados
            var celdas = new List<PdfPCell>();
            var Proveedor = new PdfPCell();
            var DiasCredito = new PdfPCell();
            var NumFactura = new PdfPCell();
            var Fecha_factura = new PdfPCell();
            var Subtotal_factura = new PdfPCell();
            var Total_cr_trib = new PdfPCell();
            var TotalFac = new PdfPCell();
            var TotalPag = new PdfPCell();
            var TotalNcDb = new PdfPCell();
            var TotalDcto = new PdfPCell();
            var TotalReten = new PdfPCell();
            var SaldosACT = new PdfPCell();
            var Chq = new PdfPCell();

            //Se adhieren a las celdas 
            celdas.Add(Proveedor);
            celdas.Add(DiasCredito);
            celdas.Add(NumFactura);
            celdas.Add(Fecha_factura);
            celdas.Add(Subtotal_factura);
            celdas.Add(Total_cr_trib);
            celdas.Add(TotalFac);
            celdas.Add(TotalPag);
            celdas.Add(TotalNcDb);
            celdas.Add(TotalDcto);
            celdas.Add(TotalReten);
            celdas.Add(SaldosACT);
            celdas.Add(Chq);

            //Genera las columnas de la tabla en base a la lista
            PdfPTable Tabla = R.TablaPDF(lista1.Count);
            R.Contenido(lista1.Count, lista1, dataGridView1, Tabla, font);
            R.detalleContenido(dataGridView1, Tabla, font, doc, celdas);
            R.Cerrar(doc, writer);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
