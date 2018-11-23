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
    public partial class frmConciliacionBancaria : Form
    {
        OdbcConnection DbConnection = new OdbcConnection("Dsn=MBA3 PRUEBA12;Driver={4D v12 ODBC Driver};server=192.168.1.2;port=19819;");
        Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();
        

        public frmConciliacionBancaria()
        {
            InitializeComponent();
            CargarDatos();
            dtpAnio.ShowUpDown = true;
        }

        private void CargarDatos()
        {
            string query = "SELECT  `CORPORATION NAM` FROM SIST_Parametros_Empresa ";
            fg.LlenarCombo(query, cmbEmpresa);
            query = "SELECT NOMBRE_CUENTA_CONTABLE FROM BANC_Ficha_Principal";
            fg.LlenarCombo(query, cmbCBancaria, -1);

        }

        private void frmPagoProveedores_Load(object sender, EventArgs e)
        {
            cmbCBancaria.Enabled = true;
            dtpAnio.Enabled = true;
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
            CleanGrid(dataGridView1);
            if (true)
            {
                string Fech1 = dtpAnio.Value.ToString("dd/MM/yyyy");
                string CORP = "SELECT CORP FROM SIST_Parametros_Empresa  WHERE `CORPORATION NAM`= '" + cmbEmpresa.Text + "' ";
                DbConnection.Open();
                OdbcCommand DbCommand = new OdbcCommand(CORP, DbConnection);
                OdbcDataReader reader = DbCommand.ExecuteReader();
                string _CORP = "";
                string _Acree = "";
                while (reader.Read())
                {
                    _CORP = reader.GetString(0);
                }
                DbConnection.Close();
                string cadena = "SELECT `INVOICE ID`, CORP, CON_DATOS, REFERENCIA_3, COBRADOR, PEDIDO_N, DATE_TO_CHAR(PEDIDO_FECHA, 'dd[/]mm[/]yyyy') AS `Fecha factura`, PEDIDO_POR, TELEFONO_GENERAL, DIRECCION_GENERAL, IDENTIFICACION_GENERAL, ORDEN_COMPRA_N,  DATE_TO_CHAR(ORDEN_COMPRA_FECHA, 'dd[/]mm[/]yyyy') AS `Fecha f`, BOLEANO_PROYECTO_1," +
             "CODIGO_SUBCLIENTE, IMPORTACION_TEMPORAL, PROD_ORDEN_N, PROD_FECHA, PROD_POR, BOLEANO_PROYECTO_5, BOLEANO_PROYECTO_4, BOLEANO_PROYECTO_3, REFERENCIA_D4, PAGARE_DIA, PAGARE_MES, PAGARE_ANO, PAGARE_INTERES, " +
             "PAGARE_FORMA_TEXTO, REFERENCIA_D3, abc_REFERENCIA_4, REFERENCIA_D5, REFERENCIA_1, REFERENCIA_2, REFERENCIA_V1, REFERENCIA_V2, REFERENCIA_D1, REFERENCIA_D2, REFERENCIA_V5, REFERENCIA_V4, REFERENCIA_V3, DESPACHO_N,  " +
             "DESPACHO_FECHA, DESPACHO_POR, DESPACHO_TIPO, BOLEANO_PROYECTO_2, REVISADO_POR, VERIFICADO_POR, DIRECCION_TRANSPORTE, TELEFONO_TRANSPORTE, TRANSPORTE, PERSONA, ID_PLACAS, BULTOS_Cajas, PESO, VOLUMEN, FECHA_RECEPCION,  " +
             "PESO_NETO, DIRECCION_FACTURA, DIRECCION_DESPACHO, INSTRUCCIONES_DESPACHO, REFERENCIA_5, EMBARQUE_DOC_GUIA, EMBARQUE_FECHA, CODIGO_PROYECTO_1, CODIGO_PROYECTO_2, CODIGO_PROYECTO_3, CODIGO_PROYECTO_4,  " +
             "CODIGO_PROYECTO_5, CODIGO_SUBPROYECTO_1, CODIGO_SUBPROYECTO_2, CODIGO_SUBPROYECTO_3, CODIGO_SUBPROYECTO_4, CODIGO_SUBPROYECTO_5, MEMO_PROYECTO_1, MEMO_PROYECTO_2, MEMO_PROYECTO_3, MEMO_PROYECTO_4,  " +
             "MEMO_PROYECTO_5, REFERENCIA_PROYECTO_1, REFERENCIA_PROYECTO_2, REFERENCIA_PROYECTO_3, REFERENCIA_PROYECTO_4, REFERENCIA_PROYECTO_5, DEPARTMET_CODE_1, DEPARTMET_CODE_2, DEPARTMET_CODE_3, DEPARTMET_CODE_4,  " +
             "DEPARTMET_CODE_5, PROFIT_CENTER_CODE_1, PROFIT_CENTER_CODE_2, PROFIT_CENTER_CODE_3, PROFIT_CENTER_CODE_4, PROFIT_CENTER_CODE_5, ANALISIS1_1, ANALISIS1_2, ANALISIS1_3, ANALISIS1_4, ANALISIS1_5, ANALISIS2_1, ANALISIS2_2, ANALISIS2_3,  " +
             "ANALISIS2_4, ANALISIS2_5, ANALISIS3_1, ANALISIS3_2, ANALISIS3_3, ANALISIS3_4, ANALISIS3_5, Booleano_Multidimension_1, Booleano_Multidimension_2, Booleano_Multidimension_3, Booleano_Multidimension_4, Booleano_Multidimension_5, Def_text_1, Def_text_2, Def_text_3,  " +
             "Email_Fiscal_SubCliente, inf_xml_Codigo_Tipo_Relacion, Codigo_Sucursal, ORIGIN, DB_NumReg, inf_xml_Codigo_Documento, Bolsas, Atados, Barricas, Total_Productos_Factura, IDE_CLIENTE_DE_SUBCLIENTE, Total_Impuesto_Val_Adic, Saldo_Impuesto_Val_Adic,  " +
             "Codigo_Impuesto_Val_Adic, inf_xml_documentoremplazo, inf_folio_fiscal_original, FechaVctoManual, FechaRemisionOriginal, FechaRecepcionMercaderia, inf_xml_Addenda_Adicional_01, inf_xml_Addenda_Adicional_02, inf_xml_Addenda_Adicional_03,  " +
             "inf_xml_Addenda_Adicional_04, inf_xml_Addenda_Adicional_05, inf_xml_Addenda_Adicional_06, inf_xml_Addenda_Adicional_07, pkUUID, inf_xml_Addenda_Adicional_08, inf_xml_Addenda_Adicional_09, inf_xml_Addenda_Adicional_10, inf_xml_Codigo_Confirmacion,  " +
             "inf_xml_Codigo_Uso_CFDI, inf_xml_Complemento_01, inf_xml_Complemento_02, inf_xml_Complemento_03, inf_xml_Complemento_04, inf_xml_Complemento_05, inf_xml_Complemento_06, inf_xml_Complemento_07, inf_xml_Complemento_08, inf_xml_Complemento_09,  " +
             "inf_xml_Complemento_10 FROM   CLNT_Factura_Principal_Adiciona";
                // string cadena = "SELECT `INVOICE ID`, CORP, CON_DATOS, REFERENCIA_3, COBRADOR, PEDIDO_N FROM   CLNT_Factura_Principal_Adiciona";
                fg.FillDataGrid(cadena, dataGridView1, DbConnection);
                
            }


            return;

        }



        private void cmbAcreedor_Leave(object sender, EventArgs e)
        {
            CheckCombo(cmbCBancaria, "empresa");
        }

        private void cmbEmpresa_Leave(object sender, EventArgs e)
        {
            CheckCombo(cmbEmpresa, "empresa");
        }

        private void dtpFechFin_Leave(object sender, EventArgs e)
        {
        
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

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dataGridView1.Columns[e.ColumnIndex].Index == 4 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 5 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 6 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 7 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index == 8 ||
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
