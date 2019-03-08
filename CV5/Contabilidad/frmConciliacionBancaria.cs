using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using clsConectaMBA;
using iTextSharp.text.pdf;
using System.Globalization;

namespace CV5
{
    public partial class frmConciliacionBancaria : Form
    {
        Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();



        public frmConciliacionBancaria()
        {
            InitializeComponent();
            CargarDatos();
            CargarCombo();
            dtpAnio.ShowUpDown = true;
        }

        private void CargarCombo()
        {
            ConexionMba cs = new ConexionMba();
            string CORP = "SELECT CORP FROM SIST_Parametros_Empresa  WHERE `CORPORATION NAM`= '" + cmbEmpresa.Text + "' ";
            OdbcCommand DbCommand = new OdbcCommand(CORP, cs.getConexion());
            OdbcDataReader reader = DbCommand.ExecuteReader();
            string _CORP = "";
            while (reader.Read())
            {
                _CORP = reader.GetString(0);
            }
            cs.cerrarConexion();
            string query = "SELECT BANC_Ficha_Principal.NOMBRE_CUENTA_CONTABLE FROM BANC_Ficha_Principal " +
                " BANC_FICHA_PRINCIPAL, SIST_Parametros_Empresa SIST_Parametros_Empresa WHERE SIST_Parametros_Empresa.CORP = BANC_Ficha_Principal.EMPRESA " +
                " AND SIST_Parametros_Empresa.CORP= '" + _CORP + "'";
            fg.LlenarCombo(query, cmbCBancaria, 1);
        }

        private void CargarDatos()
        {
            ConexionMba cs = new ConexionMba();
            string query = "SELECT  `CORPORATION NAM` FROM SIST_Parametros_Empresa ";
            fg.LlenarCombo(query, cmbEmpresa);
            
        }

        private void frmPagoProveedores_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
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
            ConexionMba cs = new ConexionMba();
            if (!string.IsNullOrEmpty(cmbEmpresa.Text) && 
                !string.IsNullOrEmpty(cmbCBancaria.Text)
                && !string.IsNullOrEmpty(dtpAnio.Text))
            {
                string CTA = "SELECT CODIGO_CUENTA_CONTABLE FROM BANC_MOVIMIENTOS_PRINCIPAL  WHERE NOMBRE_CUENTA_CONTABLE= '" + cmbCBancaria.Text + "' ";
                OdbcCommand DbCommand = new OdbcCommand(CTA, cs.getConexion());
                OdbcDataReader reader = DbCommand.ExecuteReader();
                string _CTA = "";
                while (reader.Read())
                {
                    _CTA = reader.GetString(0);
                }
                string Fech1 = dtpAnio.Value.ToString("dd/MM/yyyy");
                string cadena = "SELECT BANC_Movimientos_Principal.CODIGO_MOVIMIENTO_EMPRESA, " +
                    "BANC_Movimientos_Principal.CODIGO_MOVIMIENTO, BANC_Movimientos_Principal.PAGO_NUMERO, " +
                    "CAST(BANC_Movimientos_Principal.PAGO_A_PROVEEDOR AS int), DATE_TO_CHAR(BANC_Movimientos_Principal.FECHA_PAGO, 'dd[/]mm[/]yyyy') AS `Fecha pago` , " +
                    "BANC_Movimientos_Principal.BENEFICIARIO, BANC_Movimientos_Principal.VALOR, " +
                    "BANC_Movimientos_Principal.MEMO, BANC_Movimientos_Principal.PAGO_O_DEPOSITO, " +
                    "BANC_Movimientos_Principal.NOMBRE_CUENTA_CONTABLE, BANC_Movimientos_Principal.CODIGO_CUENTA_CONTABLE, " +
                    "DATE_TO_CHAR(BANC_Movimientos_Principal.CONCILIACION_FECHA, 'dd[/]mm[/]yyyy') AS `Fecha conciliacion` FROM BANC_Movimientos_Principal BANC_Movimientos_Principal " +
                    " WHERE BANC_Movimientos_Principal.CODIGO_CUENTA_CONTABLE = '" + _CTA + "'";
                fg.FillDataGrid(cadena, dataGridView1);
                
            } else
            {
                MessageBox.Show("Favor ingresar todos los campos");
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
            fg.ClearCombo(cmbCBancaria);
            CargarCombo();
        }



        private void btnPrint_Click(object sender, EventArgs e)
        {
            //Para generar reporte genera un objeto de clase
            Reporte R = new Reporte();
            //Genera un documento vertical
            Document doc = R.CreaDoc(false);
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
            R.Titulo(doc, cmbEmpresa.Text, font);
            R.LineaTexto(doc, "Conciliacion Bancaria - " + cmbCBancaria.Text,font);
            string fecha = dtpAnio.Value.ToString();
            DateTime _fecha = Convert.ToDateTime(fecha);
            R.LineaTexto(doc, "Al " + DateTime.DaysInMonth(_fecha.Year, _fecha.Month) +
                            " de " + dtpAnio.Value.ToString("MMMM", new CultureInfo("es-MX")) + " del " +
                            dtpAnio.Value.ToString("yyyy"), font );
            R.Cerrar(doc,writer);
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

        private void dtpAnio_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cmbCBancaria_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
