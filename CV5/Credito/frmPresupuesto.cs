using System;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using clsConectaMBA;

namespace CV5
{
    public partial class frmPresupuesto : Form
    {
        Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();
        string ID_Presupuesto;

        public frmPresupuesto()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {

            string query = "SELECT  `CORPORATION NAM` FROM SIST_Parametros_Empresa ";
            fg.LlenarCombo(query, cmbEmpresa);

           
            query = "SELECT INVT_Presupuesto_Principal.PRES_CODIGO_ID " +
                "  FROM INVT_Presupuesto_Principal INVT_Presupuesto_Principal";
            fg.LlenarCombo(query,cmbCodigo,0);

            cmbLocalidad.SelectedIndex = 0;
            cmbMes.SelectedIndex = 0;

        }

        private void CambiarRef()
        {

            string query = "SELECT INVT_Presupuesto_Principal.PRES_REFERENCIA, " +
                " INVT_Presupuesto_Principal.PRES_ANIO, INVT_Presupuesto_Principal.PRES_CODIGO_ID_CORP" +
              " FROM INVT_Presupuesto_Principal INVT_Presupuesto_Principal WHERE " +
              " INVT_Presupuesto_Principal.PRES_CODIGO_ID = '" + cmbCodigo.Text + "'";
            //flag para chequear si existen un Acreedor en particular
            ConexionMba cs = new ConexionMba();
            OdbcCommand DbCommand = new OdbcCommand(query, cs.getConexion());
            OdbcDataReader reader = DbCommand.ExecuteReader();
            string _REF = "";
            string _ANIO = "";
            while (reader.Read())
            {
                _REF = reader.GetString(0);
                _ANIO = reader.GetString(1);
                ID_Presupuesto = reader.GetString(2);
            }
            txtReferencia.Text = _REF;
            txtAnio.Text = _ANIO;
        }

        private void frmPagoProveedores_Load(object sender, EventArgs e)
        {
            
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
            if (!string.IsNullOrEmpty(cmbEmpresa.Text))
            {
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


                string flag_localidad;
                if(cmbLocalidad.SelectedIndex == 0)
                {
                    flag_localidad = "C";
                } else
                {
                    flag_localidad = "G";
                }

                //SWITCH PARA ORGANIZAR POR MES
                var valor = check_Mes();







                string cadena = "SELECT " +
                " Vend.DESCRIPTION_SPN as `VENDEDOR`, " +
                " INVT_Presupuesto_Detalle.PRES_VALOR_" + check_Mes() + " as CUOTA, " +
                " Vend.Origin as `SUCURSAL` " +
                " FROM INVT_Presupuesto_Detalle, " +
                " SIST_Lista_1 Vend WHERE(INVT_Presupuesto_Detalle.PRES_CODIGO_ID_CORP = 'PR02-LABOV') " +
                " AND Vend.GROUP_CATEGORY = 'SELLm' AND INVT_Presupuesto_Detalle.PRES_VENDEDOR = Vend.CODE " +
                " AND Vend.CORP = INVT_Presupuesto_Detalle.CORP AND INVT_Presupuesto_Detalle.PRES_CODIGO_ZONA = '" + flag_localidad + "'";    






                //string cadena = "SELECT  Vend.DESCRIPTION_SPN as `VENDEDOR`, " +
                //    " INVT_Presupuesto_Detalle.PRES_VALOR_1 as `ENERO`," +
                //    " INVT_Presupuesto_Detalle.PRES_VALOR_2 as `FEBRERO`, INVT_Presupuesto_Detalle.PRES_VALOR_3 as `MARZO`, " +
                //    " INVT_Presupuesto_Detalle.PRES_VALOR_4 as `ABRIL`, INVT_Presupuesto_Detalle.PRES_VALOR_5 as `MAYO`, " +
                //    " INVT_Presupuesto_Detalle.PRES_VALOR_6 as `JUNIO`, INVT_Presupuesto_Detalle.PRES_VALOR_7 AS `JULIO` , " +
                //    " INVT_Presupuesto_Detalle.PRES_VALOR_8 AS `AGOSTO`, INVT_Presupuesto_Detalle.PRES_VALOR_9 AS `SEPTIEMBRE`, " +
                //    " INVT_Presupuesto_Detalle.PRES_VALOR_10 AS `OCTUBRE`, INVT_Presupuesto_Detalle.PRES_VALOR_11 AS `NOVIEMBRE`, " + 
                //    " INVT_Presupuesto_Detalle.PRES_VALOR_12 AS `DICIEMBRE`  " +
                //    " FROM INVT_Presupuesto_Detalle " +
                //    " INVT_Presupuesto_Detalle, SIST_Lista_1 Vend WHERE(INVT_Presupuesto_Detalle.PRES_CODIGO_ID_CORP = 'PR02-LABOV') " +
                //    " AND Vend.GROUP_CATEGORY = 'SELLm' AND INVT_Presupuesto_Detalle.PRES_VENDEDOR = Vend.CODE " +
                //    " AND Vend.CORP = 'LABOV' AND INVT_Presupuesto_Detalle.PRES_CODIGO_ZONA = '" + flag_localidad + "'";



                fg.FillDataGrid(cadena, dataGridView1);
                double[] totales = new double[1];
                for (int a = 1; a <= 1; a++)
                {
                    double sum = 0.00;
                    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                    {
                        sum += Convert.ToDouble(dataGridView1.Rows[i].Cells[a].Value);
                    }                  
                    totales[0] = sum;

                }
                              

                txtTotal.Text = Math.Round(totales[0], 2).ToString(); 

            }


            return;

        }

        private string check_Mes()
        {
            string Mes = "";
            switch (cmbMes.SelectedIndex)
            {
                case 0:
                    Mes = "1 ";
                    break;
                case 1:
                    Mes = "2 ";
                    break;
                case 2:
                    Mes = "3 ";
                    break;
                case 3:
                    Mes = "4 ";
                    break;
                case 4:
                    Mes = "5 ";
                    break;
                case 5:
                    Mes = "6 ";
                    break;
                case 6:
                    Mes = "7 ";
                    break;
                case 7:
                    Mes = "8 ";
                    break;
                case 8:
                    Mes = "9 ";
                    break;
                case 9:
                    Mes = "10 ";
                    break;
                case 10:
                    Mes = "11 ";
                    break;
                case 11:
                    Mes = "12 ";
                    break;
                default:
                    Mes = "12 ";
                    break;
            }
            return Mes;
        }

        private void cmbAcreedor_Leave(object sender, EventArgs e)
        {
            
        }

        private void cmbEmpresa_Leave(object sender, EventArgs e)
        {
            CheckCombo(cmbEmpresa, "empresa");
        }

        //private void dtpFechFin_Leave(object sender, EventArgs e)
        //{
        //    fg.CheckDatePicker(dtpFecha, dtpFechFin);
        //}




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
            if (this.dataGridView1.Columns[e.ColumnIndex].Index == 1 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 2 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 3 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 4 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 5 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 6 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 7 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 8 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 9 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 10 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 11 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 12 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 13)
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

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void cmbCodigo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CambiarRef();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
