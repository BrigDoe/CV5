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
            string _CORP = "SELECT CORP FROM SIST_PARAMETROS_EMPRESA" +
                          " WHERE `CORPORATION NAM` = '" + cmbEmpresa.Text + "'";
            query = "SELECT INVT_Presupuesto_Principal.PRES_CODIGO_ID " +
                "  FROM INVT_Presupuesto_Principal INVT_Presupuesto_Principal WHERE " +
                " CORP='" + fg.EjecutarQuery(_CORP) + "'";
            fg.LlenarCombo(query, cmbCodigo, 0);

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
            if (!string.IsNullOrEmpty(cmbEmpresa.Text))
            {
                string CORP = "SELECT CORP FROM SIST_Parametros_Empresa  WHERE `CORPORATION NAM`= '" + cmbEmpresa.Text + "' ";
                OdbcCommand DbCommand = new OdbcCommand(CORP, cs.getConexion());
                OdbcDataReader reader = DbCommand.ExecuteReader();
                string _CORP = "";
                string flag_localidad;


                while (reader.Read())
                {
                    _CORP = reader.GetString(0);
                }
                cs.cerrarConexion();

                if (cmbLocalidad.SelectedIndex == 0)
                {
                    flag_localidad = "C";
                }
                else
                {
                    flag_localidad = "G";
                }


                string cadena = "SELECT " +
                " Vend.DESCRIPTION_SPN as `VENDEDOR`, " +
                " INVT_Presupuesto_Detalle.PRES_VALOR_" + (cmbMes.SelectedIndex + 1) + " as CUOTA, " +
                " CASE Vend.Origin  WHEN 'PRI' THEN 'COSTA' WHEN 'LA2' THEN 'SIERRA' WHEN 'LA3' THEN 'AUSTRO' END AS REGION  " +
                " FROM INVT_Presupuesto_Detalle, " +
                " SIST_Lista_1 Vend WHERE(INVT_Presupuesto_Detalle.PRES_CODIGO_ID_CORP = 'PR01-" + _CORP + "') " +
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

                #region Valores totales
                //double[] totales = new double[1];
                //for (int a = 1; a <= 1; a++)
                //{
                //    double sum = 0.00;
                //    for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                //    {
                //        sum += Convert.ToDouble(dataGridView1.Rows[i].Cells[a].Value);
                //    }                  
                //    totales[0] = sum;
                //}
                //txtTotal.Text = Math.Round(totales[0], 2).ToString(); 
                #endregion

            }


            return;

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
            string _CORP = "SELECT CORP FROM SIST_PARAMETROS_EMPRESA" +
                          " WHERE `CORPORATION NAM` = '" + cmbEmpresa.Text + "'";
            string query = "SELECT INVT_Presupuesto_Principal.PRES_CODIGO_ID " +
                "  FROM INVT_Presupuesto_Principal INVT_Presupuesto_Principal WHERE " +
                " CORP='" + fg.EjecutarQuery(_CORP) + "'";
            fg.LlenarCombo(query, cmbCodigo, 0);
            cmbLocalidad.SelectedIndex = 0;
            cmbMes.SelectedIndex = 0;
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
            R.Titulo(doc, "Reportes de Presupuesto", font);
            // Inserta imagen EN DESARROLLO
            //Image img = R.Imagen();
            //R.SetImagen(img, doc);
            //Settear anchos de la tabla en base a los encabezados
            //Se debe tener el numero exacto de encabezados que se presentan
            float[] widths = new float[] {2f, 1f, 2f, 1f, 1f, 1.25f, 1.25f, 1f,
                                          1f, 1f,2f};
            //Se cambia la fuente para el contenidol reporte
            _standardFont = FontFactory.GetFont(FontFactory.HELVETICA, 6);
            font = R.Fuente(_standardFont);
            // Lista de encabezados y contenido para reporte
            // desde el datagridView 
            R.CreaReport(dataGridView1, font, fontEnc, doc, writer, widths);
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

        void Pruebita()
        {
            ConexionMba cs = new ConexionMba();
            CleanGrid(dataGridView1);
            if (!string.IsNullOrEmpty(cmbEmpresa.Text))
            {
                string CORP = "SELECT CORP FROM SIST_Parametros_Empresa  WHERE `CORPORATION NAM`= '" + cmbEmpresa.Text + "' ";
                OdbcCommand DbCommand = new OdbcCommand(CORP, cs.getConexion());
                OdbcDataReader reader = DbCommand.ExecuteReader();
                string _CORP = "";
                string flag_localidad;


                while (reader.Read())
                {
                    _CORP = reader.GetString(0);
                }
                cs.cerrarConexion();

                if (cmbLocalidad.SelectedIndex == 0)
                {
                    flag_localidad = "C";
                }
                else
                {
                    flag_localidad = "G";
                }

                string cadena = "SELECT FP.codigo_factura, FP.codigo_cliente_empresa, FP.empresa," +
                    " DATE_TO_CHAR(FP.fecha_factura, 'dd[/]mm[/]yyyy') AS `fecha fact`, " +
                    "FP.numero_factura, FP.origen, FP.total_devolucion, valor_factura, " +
                    "FIP.CLIENT_TYPE, FIP.SALESMAN, VEN.DESCRIPTION_SPN FROM CLNT_FACTURA_PRINCIPAL FP " +
                    "INNER JOIN ( CLNT_FICHA_PRINCIPAL FIP INNER JOIN SIST_LISTA_1 VEN " +
                    "ON FIP.SALESMAN = VEN.CODE) ON FP.CODIGO_CLIENTE_EMPRESA = FIP.CODIGO_CLIENTE_EMPRESA " +
                    " WHERE ANULADA = CAST('FALSE' AS BOOLEAN) AND CONFIRMADO = CAST('TRUE' AS BOOLEAN) " +
                    "AND FECHA_FACTURA >= '01/06/2018' AND  FIP.CLIENT_TYPE IN ('DISTR', 'FARMA')" +
                    " AND VEN.GROUP_CATEGORY = 'SELLm' AND VEN.CORP=FIP.EMPRESA";

                //string cadena = "SELECT " +
                //" FP.codigo_factura, FP.codigo_cliente_empresa, " +
                //" FP.empresa, DATE_TO_CHAR(FP.fecha_factura, 'dd[/]mm[/]yyyy') AS `Fecha factura`, " +
                //" FP.numero_factura, FP.origen, FP.total_devolucion, " +
                //" FP.valor_factura, FIP.CLIENT_TYPE, FIP.SALESMAN, VEN.DESCRIPTION_SPN " +
                //" FROM " +
                //" CLNT_FACTURA_PRINCIPAL FP " +
                //" INNER JOIN " +
                //" (CLNT_FICHA_PRINCIPAL FIP INNER JOIN SIST_LISTA_1 VEN ON FIP.SALESMAN = VEN.CODE) " +
                //" WHERE " +
                //" ANULADA = CAST('FALSE' AS BOOLEAN) AND " +
                //" CONFIRMADO = CAST('TRUE' AS BOOLEAN) AND " +
                //" FECHA_FACTURA >= '01/06/2018' AND " +
                //" FIP.CLIENT_TYPE IN ('DISTR', 'FARMA') AND " +
                //" VEN.GROUP_CATEGORY = 'SELLm' AND " +
                //" VEN.CORP = FIP.EMPRESA";

                fg.FillDataGrid(cadena, dataGridView1);

            }

        }

        private void btnCheckVen_Click(object sender, EventArgs e)
        {
            //using (frmLoading frm = new frmLoading(new Action() => Pruebita()))
            //{
            //    frm.ShowDialog(this);
            //}

        }
    }
}
