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
    public partial class frmActivosFijos : Form
    {
        OdbcConnection DbConnection = new OdbcConnection("Dsn=MBA3 PRUEBA12;Driver={4D v12 ODBC Driver};server=192.168.1.2;port=19819;");
        Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();

        public frmActivosFijos()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            string query = "SELECT  `CORPORATION NAM` FROM SIST_Parametros_Empresa ";
            fg.LlenarCombo(query, cmbEmpresa);
            query = "SELECT DISTINCT VENDOR_NAME FROM PROV_Ficha_Principal";
            fg.LlenarCombo(query, cmbAcreedor, -1);

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

        private void CleanGrid(DataGridView dg)
        {
            dg.DataSource = null;
            dg.Refresh();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //flag para chequear si existen un Acreedor en particular
            CleanGrid(dataGridView1);
            Boolean flag;
            if (!fg.CheckDatePicker(dtpFechAct, dtpFechFin))
            {
                string Fech1 = dtpFechAct.Value.ToString("dd/MM/yyyy");
                string Fech2 = dtpFechFin.Value.ToString("dd/MM/yyyy");
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
                }
                else
                {
                    flag = false;
                }
                string cadena = "SELECT EMPRESA, CODIGO_ACTIVO, CODIGO_ACTIVO_EMPRESA, NOMBRE, MARCA, CODIGO_CENTRO_COSTO, COLOR, MODELO, NRO_SERIE, CODIGO_PROVEEDOR, FABRICANTE, ORIGEN, CODIGO_ESTADO, EN_USO, FECHA_INICIO_DEPRECIA, CODIGO_NO_USO, " + 
             "FECHA_COMPRA, NRO_FACTURA, CODIGO_USUARIO_RESPONSABLE, CODIGO_GRUPO_ACTIVO, CODIGO_TIPO_ACTIVO, PROPIEDAD_ACTIVO, CODIGO_UBIC_DEPAR_ACTIVO, CODIGO_UBIC_SUC_ACTIVO, CODIGO_UBIC_ZONA_ACTIVO, CODIGO_UBIC_AREA_ACTIVO, " +
             "MONEDA1, MONEDA2, CODIGO_BARRA, FOTOGRAFIA, OBSERVACIONES_ACTIVO, PERIODOS_DEPRECIADOS, PERIODOS_A_DEPRECIAR, NUMERO_UNIDADES, VIDA_UTIL, TIPO_PROCESO, TIPO_DEPRECIACION, INICIO_DEP, ACUMULABLE, MONEDA_ORIGINAL, COTIZACION, " + 
             "CODIGO_CUENTA_CONT_DEP_GASTO, NOMBRE_CUENTA_CONT_DEP_GASTO, CODIGO_CUENTA_CONT_DEP_ACUM, NOMBRE_CUENTA_CONT_DEP_ACUM, CUENTA_CONT_TIPO_DEP_GASTO, CUENTA_CONT_TIPO_DEP_ACUM, COMPONENTES, CODIGO_CUENTA_ACTIVO, " +
             "NOMBRE_CUENTA_ACTIVO, CODIGO_GRUPO, DEPRECIABLES_NODEP, VALOR_RESIDUAL, DADO_BAJA, FOTOGRAFIA1, FOTOGRAFIA2, VALOR_RESIDUAL_EXT, CODIGO_TRANSACCION, CONTABILIZADO, VALOR_COMPRA, CUENTA_CONT_TIPO_ACTIVO, " +
             "CODIGO_CUENTA_CONT_REEXPRESION, NOMBRE_CUENTA_CONT_REEXPRESION, CODIGO_CUENTA_CONT_REEXP_DEP, NOMBRE_CUENTA_CONT_REEXP_DEP, CODIGO_CUENTA_CONT_REEXP_MONET, NOMBRE_CUENTA_CONT_REEXP_MONET, " +
             "CUENTA_CONT_TIPO_REEXPEDICION, CUENTA_CONT_TIPO_REEXP_DEP, CUENTA_CONT_TIPO_REEXP_MONET, FECHA_BAJA, BAJA_AUTORIZADO_POR, OBSERVACION_BAJA, VALOR_ACTUAL_MONEDA_ORIGINAL, VALOR_ACTUAL_OTRA_MONEDA, VALOR_VENTA, " + 
             "UTILIDAD_MONEDA2, TIPO_BAJA, INICIO_REEXPRESION_FLG, REEX_SIGUIENTE_PERIODO, PERDIDA_MONEDA2, UTILIDAD_MONEDA1, PERDIDA_MONEDA1, VENTA_ACT_IVA, REEXPRESION, IMPORTACION_TEMPORAL, MONEDA_DEC1, MONEDA_DEC2, MONEDA_DEC3, " + 
             "METODO_DEP_REEXP, METODO_REEXP_DEP, FECHA_INICIO_REEXP, CODIGO_TRANSACCION_BAJA, CODIGO_TRANSACCION_BAJA_MON2, REEXP_ACT_ACUM_MON1, DEP_ACUM_MON1, REEXP_DEP_ACUM_MON1, REEXP_ACT_ACUM_MON2, DEP_ACUM_MON2, " +
             "REEXP_DEP_ACUM_MON2, IMPORTACION_ACT_REEXP, UNIDADES_PROD_DEPRECIADAS, CODIGO_PROYECTO, CODIGO_SUBPROYECTO, MEMO_PROYECTO, CODIGO_DEPARTAMENTO, CODIGO_CENTRO_COSTOS, MULTIDIMENSION_1, MULTIDIMENSION_2, " +
             "MULTIDIMENSION_3, `BOLEANO PROYECTO`, REFERENCIA_PROYECTO, Booelano_Multidimension, INFO_CONFIRMACION, INFO_CREACION, PERIODOS_DEP_TOTALES, __DISPONIBLE2, __DISPONIBLE1, Codigo_Sucursal, ORIGIN, Importacion_Activos, " +
             "MODIFICACION_TEMPORAL, DB_NumReg, Creacion_proveedores, Valor_IVA_MONEDA_ORIGINAL, Valor_IVA_MONEDA_EXTRANJERA, VALOR_VENTA_MONEDA_EXTRANJERA, IMPORT_INFO, Depunidadesprodvariable, pkUUID " +
             "FROM ACTV_Ficha_Principal";
                //"AND (PROV_Cobros_Cuotas.CORP = '" + _CORP + "') AND " +
                //"fp.VOID = cast('False' as Boolean) and fp.INVOICE_DATE >= '" + Fech1 + "' and fp.INVOICE_DATE <= '" + Fech2 + "'";
                // string cadena = "SELECT `INVOICE ID`, CORP, CON_DATOS, REFERENCIA_3, COBRADOR, PEDIDO_N FROM   CLNT_Factura_Principal_Adiciona";
                if (flag)
                //    cadena += " AND fp.VENDOR_ID_CORP = '" + _Acree + "'";
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
            }
            else
            {
                cmbAcreedor.Enabled = true;
                btnSearch.Enabled = true;
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
            Font _standardFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            Font font = R.Fuente(_standardFont);
            //Fuente para encabezados
            Font _EncstandardFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8);
            Font fontEnc = R.Fuente(_EncstandardFont);
            //Generar un writer para el reporte
            var writer = R.CreaWriter(doc);
            //Inicia la apertura del documento y escritura
            R.Iniciar(doc);
            //Titulo
            R.Titulo(doc, "Reportes de Cuentas por pagar",_standardFont);
            // Inserta imagen EN DESARROLLO
            //Image img = R.Imagen();
            //R.SetImagen(img, doc);
            //Settear anchos de la tabla en base a los encabezados
            //Se debe tener el numero exacto de encabezados que se presentan
            float[] widths = new float[] {2f, 1f, 2f, 1f, 1f, 1f, 1f,
                                            1f, 1f, 1f, 2f, 3f, 1.5f,
                                            0.5f};
            //Se cambia la fuente para el contenidol reporte
            _standardFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);
            font = R.Fuente(_standardFont);
            // Lista de encabezados para reporte
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

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
