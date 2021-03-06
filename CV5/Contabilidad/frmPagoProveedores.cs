﻿using System;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using clsConectaMBA;

namespace CV5
{
    public partial class frmPagoProveedores : Form
    {
        Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();

        public frmPagoProveedores()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            string query = "SELECT  `CORPORATION NAM` FROM SIST_Parametros_Empresa ";
            fg.LlenarCombo(query, cmbEmpresa);
            query = "SELECT DISTINCT VENDOR_NAME FROM PROV_Ficha_Principal";
            fg.LlenarCombo(query, cmbAcreedor);
           
        }

        private void frmPagoProveedores_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            cmbAcreedor.Enabled = false;
            btnBuscar.Enabled = false;
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

        private void CleanGrid(DataGridView dg) {
            dg.DataSource=null;
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
                string _CORP="";
                string _Acree = "";
                while (reader.Read()) { 
                    _CORP = reader.GetString(0);
                }
                cs.cerrarConexion();
                if (!chkAllProv.Checked)
                {
                    string Acree = "SELECT CODIGO_PROVEEDOR_EMPRESA FROM PROV_FICHA_PRINCIPAL" +
                                    " WHERE VENDOR_NAME ='" + cmbAcreedor.Text + "'" +
                                    " AND CODIGO_PROVEEDOR_EMPRESA LIKE '%" + _CORP + "'";
                    DbCommand = new OdbcCommand(Acree, cs.getConexion());
                    reader = DbCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        _Acree = reader.GetString(0);
                    }
                    cs.cerrarConexion();
                    flag = true;
                } else
                {
                    flag = false;
                }


                string cadena = "SELECT pfp.VENDOR_NAME AS Proveedor , TRIM(' DIAS' FROM pfp.`TERMS ALFA`) AS `Dias Credito`, " +
                " fp.DOC_REFERENCE AS Factura, " +
                " DATE_TO_CHAR(fp.INVOICE_DATE, 'dd[/]mm[/]yyyy') AS `Fecha factura`," +
                " fp.`RETENTION BASIS`  AS Subtotal, fp.MONTO_IMPUESTO_1 as `Total Impuestos`, " +
                " fp.`AMOUNT TAX2` AS `Total Cr Tributario`,fp.INVOICE_TOTAL AS `Total Factura`, " +
                " fp.`AMOUNT PAID` AS `Total Pagos`, fp.`BALANCE` AS `Saldos Actual`, " +
                " fp.MEMO as `F.Memo`,  " +
                " pfp.LOCALIZACION_PROVEEDOR as `L / E` " +
                " FROM PROV_Factura_Principal fp, " +
                " PROV_Ficha_Principal pfp " +
                " WHERE " +
                " fp.VENDOR_ID_CORP = pfp.CODIGO_PROVEEDOR_EMPRESA AND " +
                " fp.VOID = cast('False' as Boolean) AND " +
                " fp.INVOICE_DATE >= '" +Fech1+ "' AND " +
                " fp.INVOICE_DATE <= '"+Fech2+"' AND " +
                " fp.CORP = '" + _CORP + "' ";
                //"pcu.`PAYMENT AMOUNT` < fp.INVOICE_TOTAL";
               // string cadena = "SELECT `INVOICE ID`, CORP, CON_DATOS, REFERENCIA_3, COBRADOR, PEDIDO_N FROM   CLNT_Factura_Principal_Adiciona";
                if (flag)
                   cadena+=" AND fp.VENDOR_ID_CORP = '" + _Acree + "'";
                fg.FillDataGrid(cadena, dataGridView1);

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
            } else
            {
                cmbAcreedor.Enabled = false;
                btnSearch.Enabled = false;
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
            R.Titulo(doc, "Reportes de Cuentas por pagar",font);
            // Inserta imagen EN DESARROLLO
                //Image img = R.Imagen();
                //R.SetImagen(img, doc);
            //Settear anchos de la tabla en base a los encabezados
            //Se debe tener el numero exacto de encabezados que se presentan
            float[] widths = new float[] {2f, 1f, 2f, 1.5f, 1f, 1.25f, 1.25f, 1f,
                                          1f, 1f,2f, 2f,1f, 3f, 3f,
                                          0.5f};
            //Se cambia la fuente para el contenidol reporte
            _standardFont = FontFactory.GetFont(FontFactory.HELVETICA,6);
            font = R.Fuente(_standardFont);
            // Lista de encabezados y contenido para reporte
            // desde el datagridView 
            R.CreaReport(dataGridView1,font,fontEnc,doc,writer, widths);   
        }


        //Formateo de celdas decimales para el datagrid 
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dataGridView1.Columns[e.ColumnIndex].Index == 4 ||
                this.dataGridView1.Columns[e.ColumnIndex].Index ==  5 ||
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
