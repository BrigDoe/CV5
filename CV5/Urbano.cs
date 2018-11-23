using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using RestSharp;
using RestSharp.Authenticators;
using System.Web.Script;
using System.Web.Script.Serialization;
using System.Web;
using System.Collections.Specialized;
using System.Linq;

namespace CV5
{
    public partial class Urbano : Form
    {
              Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();

        public Urbano()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            //string query = "SELECT  `CORPORATION NAM` FROM SIST_Parametros_Empresa ";
            //fg.LlenarCombo(query, cmbEmpresa);
            //query = "SELECT DISTINCT VENDOR_NAME FROM PROV_Ficha_Principal";
            //fg.LlenarCombo(query, cmbAcreedor, -1);

        }

        private void frmPagoProveedores_Load(object sender, EventArgs e)
        {
            //cmbAcreedor.Enabled = true;
            //dtpFechAct.Enabled = true;
            //dtpFechFin.Enabled = true;
            //btnOk.Enabled = true;
        }


        private void btnExcel_Click(object sender, EventArgs e)
        {
        //    fg.ExcelClick(dataGridView1);
        }

        private void CheckCombo(ComboBox cb, string control)
        {
            //if (cb.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Por favor seleccione un valor en " + control, "Informacion",
            //                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}
        }

        private void CleanGrid(DataGridView dg)
        {
            dg.DataSource = null;
            dg.Refresh();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ////flag para chequear si existen un Acreedor en particular
            //CleanGrid(dataGridView1);
            //Boolean flag;
            //if (!fg.CheckDatePicker(dtpFechAct, dtpFechFin))
            //{
            //    string Fech1 = dtpFechAct.Value.ToString("dd/MM/yyyy");
            //    string Fech2 = dtpFechFin.Value.ToString("dd/MM/yyyy");
            //    string CORP = "SELECT CORP FROM SIST_Parametros_Empresa  WHERE `CORPORATION NAM`= '" + cmbEmpresa.Text + "' ";
            //    DbConnection.Open();
            //    OdbcCommand DbCommand = new OdbcCommand(CORP, DbConnection);
            //    OdbcDataReader reader = DbCommand.ExecuteReader();
            //    string _CORP = "";
            //    string _Acree = "";
            //    while (reader.Read())
            //    {
            //        _CORP = reader.GetString(0);
            //    }
            //    DbConnection.Close();
            //    if (!chkAllProv.Checked)
            //    {
            //        string Acree = "SELECT CODIGO_PROVEEDOR_EMPRESA FROM PROV_FICHA_PRINCIPAL" +
            //                        " WHERE VENDOR_NAME ='" + cmbAcreedor.Text + "'" +
            //                        " AND CODIGO_PROVEEDOR_EMPRESA LIKE '%" + _CORP + "'";
            //        DbConnection.Open();
            //        DbCommand = new OdbcCommand(Acree, DbConnection);
            //        reader = DbCommand.ExecuteReader();

            //        while (reader.Read())
            //        {
            //            _Acree = reader.GetString(0);
            //        }
            //        DbConnection.Close();
            //        flag = true;
            //    }
            //    else
            //    {
            //        flag = false;
            //    }
            //    string cadena = "SELECT pfp.VENDOR_NAME AS Proveedor , TRIM(' DIAS' FROM pfp.`TERMS ALFA`) AS `Dias Credito`, " +
            //    "pcu.`VEND INV REF` AS Factura, DATE_TO_CHAR(fp.INVOICE_DATE, 'dd[/]mm[/]yyyy') AS `Fecha factura`," +
            //    "fp.`RETENTION BASIS`  AS Subtotal, fp.`AMOUNT TAX2` AS `Total Cr Tributario`, fp.INVOICE_TOTAL AS" +
            //    "`Total Factura`, pcu.`PAYMENT AMOUNT` AS `Total Pagos`," +
            //    " " +
            //    "pcu.`AMOUNT DUE` AS `Saldos Actual`, pcu.`CHECK NUMBER` AS `No. Cheque`, " +
            //    "bfp.NOMBRE_BANCO AS Banco, bmp.MEMO as Memo, DATE_TO_CHAR(bmp.FECHA_PAGO, 'dd[/]mm[/]yyyy') as `Fecha Cheque`," +
            //    "pfp.LOCALIZACION_PROVEEDOR as Localizacion " +
            //    "FROM PROV_Cobros_Cuotas pcu, PROV_Factura_Principal fp, PROV_Ficha_Principal pfp, BANC_Movimientos_Principal bmp, BANC_FICHA_PRINCIPAL bfp " +
            //    "WHERE PROV_Cobros_Cuotas.`VEND INV REF` = fp.DOC_REFERENCE AND bfp.CODIGO_BANCO_EMPRESA = bmp.CODIGO_BANCO_EMPRESA " +
            //    "AND  pcu.`CHECK ID CORP`= bmp.CODIGO_MOVIMIENTO_EMPRESA " +
            //    "AND fp.VENDOR_ID_CORP = pfp.CODIGO_PROVEEDOR_EMPRESA " +
            //    "AND (PROV_Cobros_Cuotas.CORP = '" + _CORP + "') AND " +
            //    "fp.VOID = cast('False' as Boolean) and fp.INVOICE_DATE >= '" + Fech1 + "' and fp.INVOICE_DATE <= '" + Fech2 + "'";
            //    if (flag)
            //        cadena += " AND fp.VENDOR_ID_CORP = '" + _Acree + "'";
            //    fg.FillDataGrid(cadena, dataGridView1, DbConnection);
            //}


            //return;

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
            //    // SE GENERA EL PDF CON LA CLASE REPORTE
            //    Reporte R = new Reporte();
            //    // ES HORIZONTAL
            //    Document doc = R.CreaDoc(true);
            //    //Se genera fuente
            //    Font font = R.Fuente();
            //    //Generar un writer para el reporte
            //    var writer = R.CreaWriter(doc);
            //    //Inicia la apertura del documento y escritura
            //    R.Iniciar(doc);
            //    //Titulo
            //    R.Titulo(doc, "Reportes de Cuentas por pagar");
            //    Image img = R.Imagen();
            //    R.SetImagen(img, doc);
            //    //Settear anchos de la tabla en base a los encabezados
            //    float[] widths = new float[] {  2f, 1f, 2f, 1f, 1f, 1f, 1f,
            //                                    1f, 1f, 1f, 2f, 3f, 1.5f,
            //                                    0.5f };
            //    // Lista de encabezados para reporte
            //    R.CreaReport(dataGridView1, font, doc, writer, widths);
            //
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

        private class Lad
        {
            public string guia;
            public string docref;
            public string vp_linea;
        }

        private string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return string.Join("&", array);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var user = "2412-WS";
            var pwd = "7c222fb2927d828af22f592134e8932480637c0d";
            //NameValueCollection nv = new NameValueCollection();
            //nv.Add("guia", "WYB6146112");
            //nv.Add("docref", "0");
            //nv.Add("vp_linea", "3");
            //var qry = ToQueryString(nv);
            var client = new RestClient();
            client.BaseUrl = new Uri("https://app.urbano.com.ec/ws/ue/tracking/?");
            //client.Authenticator= new SimpleAuthenticator("user", user, "pass", pwd);
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type","application/x-www-form-urlencoded");
            request.AddHeader("user",user);
            request.AddHeader("pass",pwd);
            request.RequestFormat = DataFormat.Json;

            var obj = new Lad
            {
                guia = "WYB6146112",
                docref = "0",
                vp_linea = "3"
            };
            var json = new JavaScriptSerializer().Serialize(obj);
            request.AddBody(json);

            //request.AddParameter("docref", "0");
            //request.AddParameter("vp_linea", "3");








            // var json = new JavaScriptSerializer().Serialize(obj);

            // request.AddParameter("application/json", json, ParameterType.RequestBody);

            //request.AddParameter("guia", "WYB6146112");
            //request.AddParameter("","0");
            //request.AddParameter("","3");
            // execute the request
            IRestResponse response = client.Execute(request);
            var content = response.Content; // raw content as string





            // var client = new RestClient("https://app.urbano.com.ec/ws/ue/tracking/?");
            // client.Authenticator = new SimpleAuthenticator("user:",user,"pass:",pwd);
            ////client.Authenticator = new HttpBasicAuthenticator(user,"pass:", pwd);
            // var request = new RestRequest();
            // request.Method = Method.GET;
            // request.AddHeader("Accept","application/x-www-form-urlencoded");

            // var obj = new Lad
            // {
            //     guia = "WYB6146112",
            //     docref = "0",
            //     vp_linea = "3"
            // };
            // var json = new JavaScriptSerializer().Serialize(obj);


            // //// easily add HTTP Headers

            // request.AddParameter("application/json", json, ParameterType.RequestBody);

            // //request.AddParameter("guia", "WYB6146112");
            // //request.AddParameter("","0");
            // //request.AddParameter("","3");


            // // execute the request
            // IRestResponse response = client.Execute(request);
            // var content = response.Content; // raw content as string

        }
    }
}
