using System;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using clsConectaMBA;

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
                string _Acree = "";
                while (reader.Read())
                {
                    _CORP = reader.GetString(0);
                }
                cs.cerrarConexion();

                string cadena = "SELECT  INV.PRODUCT_ID_CORP, " +
                    " INV.PRODUCT_NAME, INV.QUANTITY CANTIDAD , CASE " +
                    " WHEN INV.LINE_TOTAL > 0 THEN 'FACTURADO' " +
                    " WHEN INV.LINE_TOTAL = 0 THEN 'BONIFICADO'" +
                    " ELSE 'N/A'" +
                    " END AS `TIPO DE VENTA`, " +
                    " INV.DISCOUNT DESCUENTO FROM CLNT_FACTURA_PRINCIPAL FP INNER JOIN " +
                    " INVT_PRODUCTO_MOVIMIENTOS INV ON FP.CODIGO_FACTURA = INV.DOC_ID_CORP2 " +
                    " WHERE FP.ANULADA = CAST('FALSE' AS BOOLEAN) AND  " +
                    " FP.CONFIRMADO = CAST('TRUE' AS BOOLEAN) " +
                    " AND FP.FECHA_FACTURA >= '" + Fech1 + "' AND FP.FECHA_FACTURA <= '" + Fech2 + "' and" +
                    " INV.PRODUCT_ID IS NOT NULL";

                fg.FillDataGrid(cadena, dataGridView1);

            }


            return;

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
            Document doc = R.CreaDoc(true);
            //Usa la fuente segun se requiera
            //Fuente para titulo
            Font _standardFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            Font font = R.Fuente(_standardFont);
            //Fuente para encabezados
            Font _EncstandardFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 6);
            Font fontEnc = R.Fuente(_EncstandardFont);
            //Generar un writer para el reporte
            var writer = R.CreaWriter(doc);
            //Inicia la apertura del documento y escritura
            R.Iniciar(doc);
            //Titulo
            R.Titulo(doc, "Reportes de Cuentas por pagar", font);
            // Inserta imagen EN DESARROLLO
            //Image img = R.Imagen();
            //R.SetImagen(img, doc);
            //Settear anchos de la tabla en base a los encabezados
            //Se debe tener el numero exacto de encabezados que se presentan
            float[] widths = new float[] {2f, 1f, 2f, 1f, 1f, 1.25f, 1.25f, 1f,
                                          1f, 1f,2f, 2f,1f, 3f, 3f,
                                          0.5f};
            //Se cambia la fuente para el contenidol reporte
            _standardFont = FontFactory.GetFont(FontFactory.HELVETICA, 6);
            font = R.Fuente(_standardFont);
            // Lista de encabezados y contenido para reporte
            // desde el datagridView 
            R.CreaReport(dataGridView1, font, fontEnc, doc, writer, widths);
        }


        //Formateo de celdas decimales para el datagrid 
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dataGridView1.Columns[e.ColumnIndex].Index == 4 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 5 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 6 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 7 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 8 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 9)
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
