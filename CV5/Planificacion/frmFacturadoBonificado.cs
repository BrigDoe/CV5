using System;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using clsConectaMBA;
using System.Drawing;
using System.Collections.Generic;

namespace CV5.Planificacion
{
    public partial class frmFacturadoBonificado : Form
    {
        Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();


        public frmFacturadoBonificado()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            string query = "SELECT  `CORPORATION NAM` FROM SIST_Parametros_Empresa ";
            fg.LlenarCombo(query, cmbEmpresa);

        }

        private void frmPagoProveedores_Load(object sender, EventArgs e)
        {
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

        private void CleanGrid(DataGridView dg)
        {
            dg.DataSource = null;
            dg.Refresh();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //flag para chequear si existen un Acreedor en particular

            CargarDataGrid();
            var columnas_formato = new List<int>();
            columnas_formato.Add(16);
            fg.FormatoGrid(columnas_formato,dataGridView1);
        }

        private void CargarDataGrid()
        {
            ConexionMba cs = new ConexionMba();
            CleanGrid(dataGridView1);
            Boolean flag;
            if (!fg.CheckDatePicker(dtpFechAct, dtpFechFin))
            {
                string Fech1 = dtpFechAct.Value.ToString("dd/MM/yyyy");
                string Fech2 = dtpFechFin.Value.ToString("dd/MM/yyyy");
                string CORP = "SELECT CORP FROM SIST_Parametros_Empresa  WHERE `CORPORATION NAM`= '" + cmbEmpresa.Text + "' ";
                OdbcCommand DbCommand = new OdbcCommand(CORP, cs.getConexion());
                OdbcDataReader reader = DbCommand.ExecuteReader();
                string _CORP = "";
                string empresa = "";
                while (reader.Read())
                {
                    _CORP = reader.GetString(0);
                }
                if (!chkAllProv.Checked) { empresa = " AND FP.EMPRESA = '" + _CORP + "'"; } else { empresa = " "; }
                cs.cerrarConexion();
                string cadena = "SELECT  INV.PRODUCT_ID `CODIGO`, " +
                    " INV.PRODUCT_NAME `PRODUCTO`, IFP.TIPO_ORIGEN `SEGMENTO`, INV.QUANTITY CANTIDAD , CASE " +
                    " WHEN INV.LINE_TOTAL > 0 THEN 'FACTURADO' " +
                    " WHEN INV.LINE_TOTAL = 0 THEN 'BONIFICADO'" +
                    " ELSE 'N/A'" +
                    " END AS `TIPO DE VENTA`, " +
                    " FIP.NOMBRE_CLIENTE `CLIENTE`, FIP.CLIENT_TYPE `TIPO CLIENTE`, FP.CODIGO_FACTURA `FACTURA`, DATE_TO_CHAR(FP.FECHA_FACTURA, 'dd[/]mm[/]yyyy') AS `FECHA FACTURA`, " +
                    " CASE FP.ORIGEN  " +
                        " WHEN 'PRI' THEN 'COSTA' WHEN 'LA2' THEN 'SIERRA' WHEN 'LE2' THEN 'SIERRA' " +
                        " WHEN 'DA2' THEN 'SIERRA'  WHEN 'FA2' THEN 'SIERRA'  WHEN 'AN2' THEN 'SIERRA' " +
                        " WHEN 'ME2' THEN 'SIERRA' WHEN 'LA3' THEN 'AUSTRO' WHEN 'LE3' THEN 'AUSTRO'  " +
                        " WHEN 'DA3' THEN 'AUSTRO' WHEN 'ME3' THEN 'AUSTRO' WHEN 'FA3' THEN 'AUSTRO'  " +
                        " WHEN 'AN3' THEN 'AUSTRO' END AS REGION," +
                    " FP.VENDEDOR `CODIGO VENDEDOR`, VEN.DESCRIPTION_SPN `VENDEDOR`, FIP.ZONA `CIUDAD O GIRA`,FIP.ESTADO `PROVINCIA`, FIP.CIUDAD_PRINCIPAL `CIUDAD`, " +
                    " FIP.NOMBRE_SECTOR `PARROQUIA`, INV.LINE_TOTAL `VALOR LINEA`  " +
                    " FROM CLNT_FACTURA_PRINCIPAL FP INNER JOIN " +
                    " INVT_PRODUCTO_MOVIMIENTOS INV ON FP.CODIGO_FACTURA = INV.DOC_ID_CORP2, " +
                    " CLNT_FICHA_PRINCIPAL FIP, SIST_LISTA_1 VEN, INVT_FICHA_PRINCIPAL IFP " +
                    " WHERE FP.ANULADA = CAST('FALSE' AS BOOLEAN) AND  " +
                    " FIP.CODIGO_CLIENTE_EMPRESA=FP.CODIGO_CLIENTE_EMPRESA AND " +
                    " VEN.CODE = FP.VENDEDOR AND " +
                    " VEN.GROUP_CATEGORY='SELLm'AND " +
                    " VEN.CORP = FP.EMPRESA AND " +
                    " IFP.PRODUCT_ID_CORP = INV.PRODUCT_ID_CORP AND" +
                    " FP.CONFIRMADO = CAST('TRUE' AS BOOLEAN) AND " +
                    " FP.FECHA_FACTURA >= '" + Fech1 + "' AND " +
                    " FP.FECHA_FACTURA <= '" + Fech2 + "' AND" +
                    " INV.PRODUCT_ID IS NOT NULL " + empresa + "  " +
                    " ORDER BY FP.CODIGO_FACTURA";
                fg.FillDataGrid(cadena, dataGridView1);

            }
        }

        private void cmbAcreedor_Leave(object sender, EventArgs e)
        {
            CheckCombo(cmbEmpresa, "empresa");
        }

        private void cmbEmpresa_Leave(object sender, EventArgs e)
        {
            CheckCombo(cmbEmpresa, "empresa");
        }

        private void dtpFechFin_Leave(object sender, EventArgs e)
        {
            fg.CheckDatePicker(dtpFechAct, dtpFechFin);
        }




        private void cmbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void btnPrint_Click(object sender, EventArgs e)
        {
            //Para generar reporte genera un objeto de clase
            Reporte R = new Reporte();
            //Genera un documento horizontal
            Document doc = R.CreaDoc(false);
            //Usa la fuente segun se requiera
            //Fuente para titulo
            iTextSharp.text.Font _standardFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            iTextSharp.text.Font font = R.Fuente(_standardFont);
            //Fuente para encabezados
            iTextSharp.text.Font _EncstandardFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 6);
            iTextSharp.text.Font fontEnc = R.Fuente(_EncstandardFont);
            //Generar un writer para el reporte
            var writer = R.CreaWriter(doc);
            //Inicia la apertura del documento y escritura
            R.Iniciar(doc);
            //Titulo
            R.Titulo(doc, "Reportes de productos facturados y bonificados", font);
            // Inserta imagen EN DESARROLLO
            //Image img = R.Imagen();
            //R.SetImagen(img, doc);
            //Settear anchos de la tabla en base a los encabezados
            //Se debe tener el numero exacto de encabezados que se presentan
            float[] widths = new float[] {2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f,
                                          2f, 2f, 2f, 2f,2f, 2f, 2f};
            //Se cambia la fuente para el contenidol reporte
            _standardFont = FontFactory.GetFont(FontFactory.HELVETICA, 6);
            font = R.Fuente(_standardFont);
            // Lista de encabezados y contenido para reporte
            // desde el datagridView 
            R.CreaReport(dataGridView1, font, fontEnc, doc, writer, widths);
        }


        //Formateo de celdas decimales para el datagrid 
        //private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if (this.dataGridView1.Columns[e.ColumnIndex].Index == 4 ||
        //        this.dataGridView1.Columns[e.ColumnIndex].Index == 5 ||
        //        this.dataGridView1.Columns[e.ColumnIndex].Index == 6 ||
        //        this.dataGridView1.Columns[e.ColumnIndex].Index == 7 ||
        //        this.dataGridView1.Columns[e.ColumnIndex].Index == 8 ||
        //        this.dataGridView1.Columns[e.ColumnIndex].Index == 9)
        //    {
        //        if (e.Value != null)
        //        {
        //            ConvertirFloat(e);
        //        }
        //    }
        //}


        //SE REALIZA EL FORMATEO PARA DECIMALES 
        //private void ConvertirFloat(DataGridViewCellFormattingEventArgs formatting)
        //{
        //    if (formatting.Value != null)
        //    {
        //        try
        //        {
        //            decimal e;
        //            e = decimal.Parse(formatting.Value.ToString());
        //            // Convierte a decimales
        //            formatting.Value = e.ToString("N2");
        //        }
        //        catch (FormatException)
        //        {
        //            formatting.FormattingApplied = false;

        //        }
        //    }
        //}

        private void chkAllProv_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDataGrid();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {            
            if (txtBuscar.Visible == false) {txtBuscar.Visible = true; } else { txtBuscar.Visible = false; }
            txtBuscar.Text = "Texto a buscar...";
            txtBuscar.SelectAll();
            txtBuscar.Focus();
            txtBuscar.KeyDown += new KeyEventHandler(tb_KeyDown);
        }

        void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                fg.BuscarDataGrid(dataGridView1,txtBuscar.Text.ToUpper() );
            }
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Estoy hecho de 3609 lineas de codigo. " + Environment.NewLine +
            //                " Mi creador es " +  System.Environment.UserName + Environment.NewLine +
            //                " Fue concebido en " + System.Environment.MachineName + Environment.NewLine +
            //                " El creador me mantiene alojado en " + System.Environment.CurrentDirectory + Environment.NewLine +
            //                "Oh Salve creador mio, hacedor de codigo.  ");
        }
    }
}
