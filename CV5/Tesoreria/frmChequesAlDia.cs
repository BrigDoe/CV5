using System;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using clsConectaMBA;

namespace CV5.Roles
{
    public partial class frmChequesAlDia : Form
    {
        Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();

        public frmChequesAlDia()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            cmbAcreedor.Enabled = false;
            string query = "SELECT  `CORPORATION NAM` FROM SIST_Parametros_Empresa ";
            fg.LlenarCombo(query, cmbEmpresa);
            string _CORP = "SELECT CORP FROM SIST_PARAMETROS_EMPRESA" +
                         " WHERE `CORPORATION NAM` = '" + cmbEmpresa.Text + "'";
            query = "SELECT CFP.NOMBRE_CLIENTE  FROM CLNT_FICHA_PRINCIPAL CFP, " +
                " CLNT_COBRO_PRINCIPAL CLNT WHERE CFP.CODIGO_CLIENTE_EMPRESA = CLNT.CODIGO_CLIENTE_EMPRESA " +
                " AND CFP.EMPRESA='" + fg.EjecutarQuery(_CORP) + "' ";
            fg.LlenarCombo(query, cmbAcreedor, -1);

        }

        private void frmPagoProveedores_Load(object sender, EventArgs e)
        {
            cmbAcreedor.Enabled = false;
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
                string _Acree = "";
                while (reader.Read())
                {
                    _CORP = reader.GetString(0);
                }
                cs.cerrarConexion();

                if (!chkAllProv.Checked)
                {
                    string Acree = "SELECT  CODIGO_CLIENTE_EMPRESA " +
                        " FROM CLNT_FICHA_PRINCIPAL WHERE" +
                        " NOMBRE_CLIENTE = '" + cmbAcreedor.Text + "'" +
                        " and EMPRESA = '" + _CORP + "'";
                    DbCommand = new OdbcCommand(Acree, cs.getConexion());
                    reader = DbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        _Acree = reader.GetString(0);
                    }
                    cs.cerrarConexion();
                    flag = true;
                }
                else
                {
                    flag = false;
                }


                string cadena = "SELECT CLNT.empresa, CLNT.CODIGO_CLIENTE_EMPRESA, CLNT.CHEQUE_NUMERO, CLNT.VALOR_COBRO," +
                        " DATE_TO_CHAR(CLNT.FECHA_COBRO, 'dd[/]mm[/]yyyy') AS `Fecha cobro` , " +
                        " CLNT.CODIGO_COBRO, CLNT.ID_FISCAL, CBR.`BANK O CC TYPE`, " +
                        " DATE_TO_CHAR( CBR.FECHA_CHEQUE_O_EXPIRA_TC, 'dd[/]mm[/]yyyy') AS `Fecha cheque` , CASE CBR.ORIGEN  " +
                        " WHEN 'PRI' THEN 'COSTA' WHEN 'LA2' THEN 'SIERRA' WHEN 'LE2' THEN 'SIERRA' " +
                        " WHEN 'DA2' THEN 'SIERRA'  WHEN 'FA2' THEN 'SIERRA'  WHEN 'AN2' THEN 'SIERRA' " +
                        " WHEN 'ME2' THEN 'SIERRA' WHEN 'LA3' THEN 'AUSTRO' WHEN 'LE3' THEN 'AUSTRO'  " +
                        " WHEN 'DA3' THEN 'AUSTRO' WHEN 'ME3' THEN 'AUSTRO' WHEN 'FA3' THEN 'AUSTRO'  " +
                        " WHEN 'AN3' THEN 'AUSTRO' END AS REGION, CFP.NOMBRE_CLIENTE  " +
                        " FROM  CLNT_COBRO_FORMADEPAGO CBR, CLNT_COBRO_PRINCIPAL CLNT,  CLNT_FICHA_PRINCIPAL CFP " +
                        " WHERE CBR.CODIGO_COBRO = CLNT.CODIGO_COBRO AND " +
                        " CLNT.CODIGO_CLIENTE_EMPRESA = CFP.CODIGO_CLIENTE_EMPRESA AND " +
                        " CLNT.ANULADO = cast('False' as Boolean) AND " +
                        " CLNT.FORMA_DE_PAGO='CHEQUE' AND CBR.FORMA_DE_PAGO='Cheque' AND" +
                        " CLNT.EMPRESA = '" + _CORP + "' AND" + 
                        " CBR.FECHA_CHEQUE_O_EXPIRA_TC >= '" + Fech1 + "' AND " +
                        " CBR.FECHA_CHEQUE_O_EXPIRA_TC <= '" + Fech2 + "' " ;

                if (flag)
                    cadena += " AND CLNT.CODIGO_CLIENTE_EMPRESA = '" + _Acree + "'";
                fg.FillDataGrid(cadena, dataGridView1);

            }


            return;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            CargarDataGrid();           

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
            }
            else
            {
                cmbAcreedor.Enabled = true;
            }
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
            R.Titulo(doc, "Reportes de cheques al dia", font);
            // Inserta imagen EN DESARROLLO
            //Image img = R.Imagen();
            //R.SetImagen(img, doc);
            //Settear anchos de la tabla en base a los encabezados
            //Se debe tener el numero exacto de encabezados que se presentan
            float[] widths = new float[] {2f, 2f, 2f, 2f, 2f, 2f, 2f, 2f,
                                          2f, 2f};
            //Se cambia la fuente para el contenidol reporte
            _standardFont = FontFactory.GetFont(FontFactory.HELVETICA, 6);
            font = R.Fuente(_standardFont);
            // Lista de encabezados y contenido para reporte
            // desde el datagridView 
            R.CreaReport(dataGridView1, font, fontEnc, doc, writer, widths);
        }


       

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void cmbAcreedor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            fg.LimpiarGrid(dataGridView1);
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            fg.BuscarGrid(dataGridView1, txtBuscar);
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDataGrid();
        }
    }
}
