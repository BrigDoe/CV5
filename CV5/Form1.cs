using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace CV5
{
    public partial class Form1 : Form
    {
        OdbcConnection DbConnection = new OdbcConnection("Dsn=MBA3 PRUEBA12;Driver={4D v12 ODBC Driver};server=192.168.1.2;port=19819;");

        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.MaxDropDownItems = 5;
            comboBox1.IntegralHeight = false;
            comboBox2.MaxDropDownItems = 6;
            comboBox1.IntegralHeight = false;
            try
            {
                DbConnection.Open();
                string query = "SELECT DISTINCT VENDOR_NAME FROM PROV_Ficha_Principal";
                LlenarCombo(query,comboBox1,0);
                query = "SELECT  `CORPORATION NAM` FROM SIST_Parametros_Empresa ";
                LlenarCombo(query, comboBox2, 0);
                comboBox1.SelectedIndex=1;
            }
            catch (OdbcException ex)
            {
                MessageBox.Show("Existe un problema con la conexion ODBC." + 
                    System.Environment.NewLine + "Favor verifique " +
                    "que existe concordancia con el DSN. " + System.Environment.NewLine + ex.Message,
                    "Error detectado en el DSN");
                Application.Exit();
            }
          
        }




        //Checkea si el sistema es de 32 o 64 bits
        public int Bits()
        {
            return IntPtr.Size * 8;
        }


        //Llena combobox en base a select  
        //recoge query, el combo y el indice del valor de texto 
        private void LlenarCombo(string query, ComboBox cb, int a)
        {
            OdbcCommand DbCommand = new OdbcCommand(query,DbConnection);
            OdbcDataAdapter adp1 = new OdbcDataAdapter();
            DataTable dt = new DataTable();
            DbCommand = new OdbcCommand(query,DbConnection);
            adp1 = new OdbcDataAdapter();
            dt = new DataTable();
            adp1.SelectCommand = DbCommand;
            adp1.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cb.Items.Add(dt.Rows[i][a].ToString());
            }
            DbConnection.Close();
        }

        

        private void FillDataGrid(string cadena, DataGridView dataGridView1,
                                  OdbcConnection DbConnection)
        {
            
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            
            OdbcCommand DbCommand = new OdbcCommand(cadena, DbConnection);
            OdbcDataAdapter adp1 = new OdbcDataAdapter();
            DataTable dt = new DataTable();
            adp1.SelectCommand = DbCommand;
            adp1.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
            }
            


            //DbCommand = new OdbcCommand(cadena, DbConnection);
            //adp1 = new OdbcDataAdapter();
            //dt = new DataTable();
            //adp1.SelectCommand = DbCommand;
            //adp1.Fill(dt);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    comboBox1.Items.Add(dt.Rows[i]["NOMBRE_CUENTA_ACTIVO"].ToString());
            //}
            DbConnection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string cadena = "SELECT NOMBRE, CODIGO_ACTIVO FROM ACTV_Ficha_Principal";
            string cadena = "SELECT pfp.VENDOR_NAME AS Proveedor , pfp.`TERMS ALFA` AS Dias_Credito, " +
                "pcu.`VEND INV REF` AS Factura, DATE_TO_CHAR(fp.INVOICE_DATE, 'dd[/]mm[/]yyyy') AS FECHA_FACTURA," +
                "fp.`RETENTION BASIS` AS Subtotal_Fac, fp.`AMOUNT TAX2` AS Total_CR_Tributario, fp.INVOICE_TOTAL AS" +
                " TOTAL_FACTURA, pcu.`PAYMENT AMOUNT` AS TOTAL_Pagos, pcu.`PAYMENT AMOUNT` AS TOTAL_Ncprv_Ndprv," +
                " pcu.`DISCOUNT AMOUNT` AS Total_dcto, pcu.`RETENTION AMNT` AS Total_Retenciones," +
                " pcu.`AMOUNT DUE` AS Saldos_Actual, pcu.`CHECK NUMBER` AS Chq " +
                "FROM PROV_Cobros_Cuotas pcu, PROV_Factura_Principal fp, PROV_Ficha_Principal pfp " +
                "WHERE PROV_Cobros_Cuotas.`VEND INV REF` = fp.DOC_REFERENCE AND fp.VENDOR_ID_CORP =" +
                "pfp.CODIGO_PROVEEDOR_EMPRESA AND (PROV_Cobros_Cuotas.CORP = 'LABOV') AND " +
                "fp.VOID = cast('False' as Boolean) ORDER BY fp.INVOICE_DATE";
            FillDataGrid(cadena, dataGridView1,DbConnection);
            //string cadena = "SELECT   FROM PROV_Ficha_Principal " +
            //    "WHERE(CODIGO_PROVEEDOR_EMPRESA = 'P0140-LABOV')";
            //string cadena3 = "SELECT DISTINCT NOMBRE_CUENTA_ACTIVO FROM ACTV_Ficha_Principal";
            
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private List<string> Encabezados (){
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

        

        private void button2_Click(object sender, EventArgs e)
        {
            //
            Reporte R = new Reporte();
            Document doc = R.CreaDoc(false);
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

            //


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource=null;
            DbConnection.Open();
            string cadena2_1 = "SELECT EMPRESA, CODIGO_ACTIVO, NOMBRE_CUENTA_ACTIVO FROM ACTV_Ficha_Principal WHERE NOMBRE_CUENTA_ACTIVO LIKE ('";
            string cadena2_2 = "%')";
            string cadena2 = cadena2_1 + comboBox1.Text + cadena2_2;
            OdbcCommand DbCommand = new OdbcCommand(cadena2, DbConnection);
            OdbcDataAdapter adp1 = new OdbcDataAdapter();
            DataTable dt = new DataTable();
            adp1.SelectCommand = DbCommand;
            adp1.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
            }
            DbConnection.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Reporte R = new Reporte();
            Document doc = R.CreaDoc(true);
            iTextSharp.text.Font font = R.Fuente();
            PdfWriter writer = R.CreaWriter(doc);
            R.Iniciar(doc);
            R.Titulo(doc,"Reportes prueba");
            iTextSharp.text.Image img = R.Imagen();
            R.SetImagen(img,doc);

            // Lista de encabezados para reporte
            List<string> lista1 = new List<string>()
            {
                "Empresa",
                "Codigo",
                "Nombre"
            };

            // Se crea objetos en base a la lista de encabezados
            var celdas = new List<PdfPCell>();
            var Empresa = new PdfPCell();
            celdas.Add(Empresa);
            var Codigo = new PdfPCell();
            celdas.Add(Codigo);
            var Nombre = new PdfPCell();
            celdas.Add(Nombre);

            //Genera las columnas de la tabla en base a la lista
            PdfPTable Tabla = R.TablaPDF(lista1.Count);
            R.Contenido(lista1.Count, lista1, dataGridView1, Tabla, font);
            R.detalleContenido(dataGridView1, Tabla, font, doc, celdas);
            R.Cerrar(doc,writer);            
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            List<string> lista = Encabezados();
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].HeaderText = lista[i];
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void copyAlltoClipboard()
        {
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            copyAlltoClipboard();
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
