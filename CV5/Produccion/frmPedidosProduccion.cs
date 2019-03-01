using System;
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

namespace CV5
{
    public partial class frmPedidosProduccion : Form
    {

        Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();
        string code_vendedor;


        public frmPedidosProduccion()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            string query = "SELECT  `CORPORATION NAM` FROM SIST_Parametros_Empresa ";
            fg.LlenarCombo(query, cmbEmpresa, -1);
            string _CORP = "SELECT CORP FROM SIST_PARAMETROS_EMPRESA" +
            " WHERE `CORPORATION NAM` = '" + cmbEmpresa.Text + "'";
            pgb.Style = ProgressBarStyle.Marquee;
            pgb.MarqueeAnimationSpeed = 30;
            // DESHABILITADO PAARA DEBUG
            // cmbLocalidad.SelectedIndex = -1;
            // cmbCodigo.SelectedIndex = -1;
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


        private void FormatoGrid(List<int> list)
        {
            foreach (int cols in list)
            {
                dataGridView1.Columns[cols].DefaultCellStyle.Format = "N2";
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            CleanGrid(dataGridView1);
            pgb.Visible = true;
            ObtenerFacturas();
            var columnas = new List<int>();
            columnas.Add(6);
            columnas.Add(13);
            columnas.Add(14);
            columnas.Add(15);
            FormatoGrid(columnas);
            decimal totalVenta = dataGridView1.Rows.Cast<DataGridViewRow>()
                .Sum(t => Convert.ToDecimal(t.Cells[6].Value));
            decimal NC = dataGridView1.Rows.Cast<DataGridViewRow>()
                .Sum(t => Convert.ToDecimal(t.Cells[14].Value));
            decimal totalValorBruto = dataGridView1.Rows.Cast<DataGridViewRow>()
                .Sum(t => Convert.ToDecimal(t.Cells[6].Value));
            decimal totalValorNeto = dataGridView1.Rows.Cast<DataGridViewRow>()
                .Sum(t => Convert.ToDecimal(t.Cells[15].Value));
        }

        public void StopBarra()
        {
            pgb.Visible = false;
        }


        private void cmbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            string _CORP = "SELECT CORP FROM SIST_PARAMETROS_EMPRESA" +
                          " WHERE `CORPORATION NAM` = '" + cmbEmpresa.Text + "'";
            string query_vendedores = "SELECT VEN.DESCRIPTION_SPN " +
                         " FROM SIST_LISTA_1 VEN WHERE VEN.GROUP_CATEGORY = 'SELLm' AND " +
                         " VEN.CORP='" + fg.EjecutarQuery(_CORP) + "' " +
                         " ORDER BY VEN.DESCRIPTION_SPN ";
            
            
        }



        private void btnPrint_Click(object sender, EventArgs e)
        {
            //Para generar reporte genera un objeto de clase
            Reporte R = new Reporte();
            //Genera un documento horizontal
            Document doc = R.CreaDoc(true);
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
            R.Titulo(doc, "Reportes de Ventas Diarias", font);
            // Inserta imagen EN DESARROLLO
            //Image img = R.Imagen();
            //R.SetImagen(img, doc);
            //Settear anchos de la tabla en base a los encabezados
            //Se debe tener el numero exacto de encabezados que se presentan
            float[] widths = new float[] {1f, 1f, 1.25f, 1f, 1.5f, 1f, 1f,
                                          2f, 1f,2f,2f,3f,2f,1f,1f,1f};
            //Se cambia la fuente para el contenidol reporte
            _standardFont = FontFactory.GetFont(FontFactory.HELVETICA, 6);
            font = R.Fuente(_standardFont);
            // Lista de encabezados y contenido para reporte
            // desde el datagridView 
            R.CreaReport(dataGridView1, font, fontEnc, doc, writer, widths);
        }


        void ObtenerFacturas()
        {
                ConexionMba cs = new ConexionMba();
                string CORP = "SELECT CORP FROM SIST_Parametros_Empresa  WHERE `CORPORATION NAM`= '" + cmbEmpresa.Text + "' ";
                OdbcCommand DbCommand = new OdbcCommand(CORP, cs.getConexion());
                OdbcDataReader reader = DbCommand.ExecuteReader();
                string _CORP = "";
                string flag_localidad = "";
                string flag_region = "";
                while (reader.Read())
                {
                    _CORP = reader.GetString(0);
                }
                cs.cerrarConexion();          


                string cadena = " SELECT FP.EMPRESA `EMPRESA `, CASE FP.ORIGEN  WHEN 'PRI' THEN 'COSTA'" +
                    " WHEN 'LA2' THEN 'SIERRA' WHEN 'LE2' THEN 'SIERRA'  " +
                    " WHEN 'DA2' THEN 'SIERRA'  WHEN 'FA2' THEN 'SIERRA'  WHEN 'AN2' THEN 'SIERRA' " +
                    " WHEN 'ME2' THEN 'SIERRA' WHEN 'LA3' THEN 'AUSTRO' WHEN 'LE3' THEN 'AUSTRO'  " +
                    " WHEN 'DA3' THEN 'AUSTRO' WHEN 'ME3' THEN 'AUSTRO' WHEN 'FA3' THEN 'AUSTRO' " +
                    " WHEN 'AN3' THEN 'AUSTRO' END AS REGION, " +
                    " FIP.ESTADO `PROVINCIA`, FIP.CIUDAD_PRINCIPAL `CIUDAD`, FIP.NOMBRE_SECTOR `SECTOR`, " +
                    // " FP.codigo_cliente, " +
                    " DATE_TO_CHAR(FP.fecha_factura, 'dd[/]mm[/]yyyy') AS `FECHA FACT`, " +
                    " CAST(FP.VALOR_FACTURA AS float) `VALOR TOTAL`, FP.codigo_factura `FACTURA`, " +
                    " DATE_TO_CHAR(FP.FECHA_EMBARQUE, 'dd[/]mm[/]yyyy') AS `DESPACHO`, " +
                    // " FP.numero_factura, FP.origen, FP.total_devolucion, valor_factura, " +
                    " FIP.IDENTIFICACION_FISCAL `IDENTIFICACION`, FIP.NOMBRE_CLIENTE `CLIENTE`, FIP.DIRECCION_PRINCIPAL_1 `DIRECCION`," +
                    " VEN.DESCRIPTION_SPN `VENDEDOR`, FP.VALOR_FACTURA `VALOR BRUTO`, FP.TOTAL_DEVOLUCION `NOTA DE CREDITO`, " +
                    " (FP.VALOR_FACTURA - FP.TOTAL_DEVOLUCION) `VALOR NETO`" +
                //    "FIP.CLIENT_TYPE, FIP.SALESMAN, VEN.DESCRIPTION_SPN " +
                    " FROM " +
                    " CLNT_FACTURA_PRINCIPAL FP " +
                    " INNER JOIN " +
                    " (CLNT_FICHA_PRINCIPAL FIP INNER JOIN SIST_LISTA_1 VEN ON FIP.SALESMAN = VEN.CODE) " +
                    " ON FP.CODIGO_CLIENTE_EMPRESA = FIP.CODIGO_CLIENTE_EMPRESA " +
                    " WHERE ANULADA = CAST('FALSE' AS BOOLEAN) AND CONFIRMADO = CAST('TRUE' AS BOOLEAN) " +
                    " AND VEN.GROUP_CATEGORY = 'SELLm' AND VEN.CORP=FIP.EMPRESA ";
                //" AND FP.EMPRESA = '" + _CORP + "' ";
                //" AND VEN.CODE= '" +  + "' " +
                    fg.FillDataGrid(cadena, dataGridView1);
            }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            fg.LimpiarGrid(dataGridView1);
            fg.ClearCombo(cmbEmpresa);
            CargarDatos();
        }
    }
          
   }
