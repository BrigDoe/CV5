﻿using System;
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
    public partial class frmVentasDiarias : Form
    {

        Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();
        string code_vendedor;


        public frmVentasDiarias()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            cmbRegion.Enabled = true;
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
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
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
            Cursor.Current = Cursors.WaitCursor;
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
            txtValorTotal.Text = String.Format("{0:.##}", totalValorNeto);
            txtNC.Text = String.Format("{0:.##}", NC);
            txtValorBruto.Text = String.Format("{0:.##}", totalValorBruto);
            txtValorNeto.Text = String.Format("{0:.##}", totalValorNeto);
            pgb.Visible = false;
            Cursor.Current = Cursors.Default;
        }

        public void StopBarra()
        {
            pgb.Visible = false;
        }


        private void cmbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            fg.ClearCombo(cmbCodigo);
            string _CORP = "SELECT CORP FROM SIST_PARAMETROS_EMPRESA" +
                          " WHERE `CORPORATION NAM` = '" + cmbEmpresa.Text + "'";
            string query_vendedores = "SELECT VEN.DESCRIPTION_SPN " +
                         " FROM SIST_LISTA_1 VEN WHERE VEN.GROUP_CATEGORY = 'SELLm' AND " +
                         " VEN.CORP='" + fg.EjecutarQuery(_CORP) + "' " +
                         " ORDER BY VEN.DESCRIPTION_SPN ";
            fg.LlenarCombo(query_vendedores, cmbCodigo, 0);
            if (chkVendedor.Checked)
                cmbCodigo.Enabled = false; cmbCodigo.SelectedIndex = -1;            
            cmbLocalidad.SelectedIndex = -1;
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
            R.Titulo(doc, "Reportes de Cobros", font);
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
            if (!fg.CheckDatePicker(dtpFechAct, dtpFechFin))
            {
                string Fech1 = dtpFechAct.Value.ToString("dd/MM/yyyy");
                string Fech2 = dtpFechFin.Value.ToString("dd/MM/yyyy");
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
                switch (cmbLocalidad.SelectedIndex)
                {
                    case 0:
                        flag_localidad = "C";
                        break;
                    case 1:
                        flag_localidad = "G";
                        break;
                }


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
                    " AND FECHA_FACTURA >= '" + Fech1 + "' AND FECHA_FACTURA <='" + Fech2 + "' AND  FIP.CLIENT_TYPE IN ('DISTR', 'FARMA')" +
                    " AND VEN.GROUP_CATEGORY = 'SELLm' AND VEN.CORP=FIP.EMPRESA ";
                //" AND FP.EMPRESA = '" + _CORP + "' ";
                //" AND VEN.CODE= '" +  + "' " +
                if (cmbLocalidad.SelectedIndex != -1)
                    cadena += " AND FIP.ZONA= '" + flag_localidad + "' ";
                if (cmbRegion.SelectedIndex != -1)
                    cadena += " AND FP.ORIGEN='" + CheckRegion(cmbRegion, flag_region, _CORP) + "' ";
                if (cmbEmpresa.SelectedIndex != -1)
                    cadena += " AND FP.EMPRESA ='" + _CORP + "' ";
                if (cmbCodigo.SelectedIndex != -1)
                    cadena += " AND VEN.CODE ='" + CheckCodeVendedor() + "' ";
                if (_CORP == "" && cmbRegion.Text.Length > 0 && flag_region.Length == 0) 
                    MessageBox.Show("Es necesario seleccionar una empresa para mostrar su region", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);   
                fg.FillDataGrid(cadena, dataGridView1);
            }
        }



        private string CheckCodeVendedor()  {
            string _CORP = "SELECT CORP FROM SIST_PARAMETROS_EMPRESA" +
            " WHERE `CORPORATION NAM` = '" + cmbEmpresa.Text + "'";
            string query_cod_vendedor = "SELECT  CODE FROM SIST_LISTA_1 " +
            "WHERE GROUP_CATEGORY= 'SELLm' and CORP='" + fg.EjecutarQuery(_CORP) + "' " +
            "AND DESCRIPTION_SPN='" + cmbCodigo.Text + "' ";
            code_vendedor = fg.EjecutarQuery(query_cod_vendedor);
            return code_vendedor;
        }


        private string CheckRegion(ComboBox cb, string flag_region, String _CORP)
        {
            if (_CORP == "LABOV")
                switch (cmbRegion.SelectedIndex)
                {
                    case 0:
                        flag_region = "PRI";
                        break;
                    case 1:
                        flag_region = "LA2";
                        break;
                    case 2:
                        flag_region = "LA3";
                        break;
                }

            if (_CORP == "DANI")
                switch (cmbRegion.SelectedIndex)
                {
                    case 0:
                        flag_region = "PRI";
                        break;
                    case 1:
                        flag_region = "DA2";
                        break;
                    case 2:
                        flag_region = "DA3";
                        break;
                }

            if (_CORP == "LEBEN")
                switch (cmbRegion.SelectedIndex)
                {
                    case 0:
                        flag_region = "PRI";
                        break;
                    case 1:
                        flag_region = "LE2";
                        break;
                    case 2:
                        flag_region = "LE3";
                        break;
                }

            if (_CORP == "MEDIT")
                switch (cmbRegion.SelectedIndex)
                {
                    case 0:
                        flag_region = "PRI";
                        break;
                    case 1:
                        flag_region = "ME2";
                        break;
                    case 2:
                        flag_region = "ME3";
                        break;
                }

            if (_CORP == "ANYUP")
                switch (cmbRegion.SelectedIndex)
                {
                    case 0:
                        flag_region = "PRI";
                        break;
                    case 1:
                        flag_region = "AN2";
                        break;
                    case 2:
                        flag_region = "AN3";
                        break;
                }

            if (_CORP == "FARMA")
                switch (cmbRegion.SelectedIndex)
                {
                    case 0:
                        flag_region = "PRI";
                        break;
                    case 1:
                        flag_region = "FA2";
                        break;
                    case 2:
                        flag_region = "FA3";
                        break;
                }

            return flag_region;
        }


        private void btnNuevo_Click(object sender, EventArgs e)
        {
            fg.LimpiarGrid(dataGridView1);
            fg.ClearCombo(cmbCodigo);
            fg.ClearCombo(cmbEmpresa);
            CargarDatos();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEmpresa.Checked)
            {
                cmbEmpresa.Enabled = false;
                cmbEmpresa.SelectedIndex = -1;
                chkVendedor.Enabled = false;
            } else
            {
                cmbEmpresa.Enabled = true;
                cmbEmpresa.SelectedIndex = 0;
                chkVendedor.Enabled = true;
            }
        }

        private void chkVendedor_CheckedChanged(object sender, EventArgs e)
        {
            if (chkVendedor.Checked)
            {
                cmbCodigo.Enabled = false;
                cmbCodigo.SelectedIndex = -1;
            }
            else
            {
                cmbCodigo.Enabled = true;
                cmbCodigo.SelectedIndex = 0;
            }
        }

        private void chkRegion_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRegion.Checked)
            {
                cmbRegion.Enabled = false;
                cmbRegion.SelectedIndex = -1;
            }
            else
            {
                cmbRegion.Enabled = true;
                cmbRegion.SelectedIndex = 0;
            }
        }

        private void chkLocalidad_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLocalidad.Checked)
            {
                cmbLocalidad.Enabled = false;
                cmbLocalidad.SelectedIndex = -1;
            }
            else
            {
                cmbLocalidad.Enabled = true;
                cmbLocalidad.SelectedIndex = 0;
            }
        }
    }
}
