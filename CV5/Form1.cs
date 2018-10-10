using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Microsoft.Win32;

namespace CV5
{
    public partial class Form1 : Form
    {
        OdbcConnection DbConnection = new OdbcConnection("Dsn=MBA3 PRUEBA12;Driver={4D v12 ODBC Driver};server=192.168.1.2;port=19819;");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                DbConnection.Open();
                string query = "SELECT DISTINCT NOMBRE_CUENTA_ACTIVO FROM ACTV_Ficha_Principal";
                LlenarCombo(query,comboBox1);
                comboBox1.SelectedIndex=1;
            }
            catch (OdbcException ex)
            {
                MessageBox.Show("Existe un problema con la conexion ODBC." + 
                    System.Environment.NewLine + "Favor verifique " +
                    "que existe concordancia con el DSN. " + System.Environment.NewLine + ex.Message,
                    "Error detectado en el DSN");
                Application.Exit();
            }
          
        }

        //Checkea si el sistema es de 32 o 64 bits
        public int Bits()
        {
            return IntPtr.Size * 8;
        }


        //Llena combobox en base a select 
        private void LlenarCombo(string query, ComboBox cb)
        {
            OdbcCommand DbCommand = new OdbcCommand(query,DbConnection);
            OdbcDataAdapter adp1 = new OdbcDataAdapter();
            DataTable dt = new DataTable();
            DbCommand = new OdbcCommand(query,DbConnection);
            adp1 = new OdbcDataAdapter();
            dt = new DataTable();
            adp1.SelectCommand = DbCommand;
            adp1.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox1.Items.Add(dt.Rows[i]["NOMBRE_CUENTA_ACTIVO"].ToString());
            }
            DbConnection.Close();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            //string cadena = "SELECT NOMBRE, CODIGO_ACTIVO FROM ACTV_Ficha_Principal";
            string cadena = "SELECT EMPRESA, NOMBRE_CLIENTE, IDENTIFICACION_FISCAL FROM CLNT_Ficha_Principal ";
            string cadena3 = "SELECT DISTINCT NOMBRE_CUENTA_ACTIVO FROM ACTV_Ficha_Principal";
            OdbcCommand DbCommand = new OdbcCommand(cadena, DbConnection);
            OdbcDataAdapter adp1 = new OdbcDataAdapter();
            DataTable dt = new DataTable();
            adp1.SelectCommand = DbCommand;
            adp1.Fill(dt);
            if (dt.Rows.Count >0){
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                //Soy un comentario
            }
            DbCommand = new OdbcCommand(cadena3, DbConnection);
            adp1 = new OdbcDataAdapter();
            dt = new DataTable();
            adp1.SelectCommand = DbCommand;
            adp1.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBox1.Items.Add(dt.Rows[i]["NOMBRE_CUENTA_ACTIVO"].ToString());
            }
            DbConnection.Close();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            Reporte Rp = new Reporte();


            Document doc = new Document(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(doc,
                                        new FileStream(@"C:\Pruebas REPORTES\prueba.pdf", 
                                        FileMode.Create));
            doc.AddTitle("PDF LABO");
            doc.AddCreator("B. Vera");
            doc.Open();

            // Creamos el tipo de Font que vamos utilizar
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(
                iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL,
                BaseColor.BLACK);           
            
            // Escribimos el encabezado en el documento
            doc.Add(new Paragraph("Reporte de activos"));
            doc.Add(Chunk.NEWLINE);

            // Creamos una tabla que contendrá el nombre, apellido y país
            // de nuestros visitante.
            PdfPTable tblPrueba = new PdfPTable(3);
            tblPrueba.WidthPercentage = 50;

            // Configuramos el título de las columnas de la tabla
            PdfPCell clNombre = new PdfPCell(new Phrase("Empresa", _standardFont));
            clNombre.BorderWidth = 0;
            clNombre.BorderWidthBottom = 0.75f;

            PdfPCell clCodigo = new PdfPCell(new Phrase("Codigo", _standardFont));
            clCodigo.BorderWidth = 0;
            clCodigo.BorderWidthBottom = 0.75f;

            PdfPCell clEmpresa = new PdfPCell(new Phrase("Nombre", _standardFont));
            clEmpresa.BorderWidth = 0;
            clEmpresa.BorderWidthBottom = 0.75f;


            // Añadimos las celdas a la tabla
            tblPrueba.AddCell(clNombre);
            tblPrueba.AddCell(clCodigo);
            tblPrueba.AddCell(clEmpresa);

            for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
            {
                    string value = dataGridView1.Rows[rows].Cells[0].Value.ToString();
                    Console.WriteLine("Nombre:" + value);
                    clNombre = new PdfPCell(new Phrase(value, _standardFont));
                    clNombre.BorderWidth = 0;
                    tblPrueba.AddCell(clNombre);
                    value = dataGridView1.Rows[rows].Cells[1].Value.ToString();
                    Console.WriteLine("Cod:" + value);
                    clCodigo = new PdfPCell(new Phrase(value, _standardFont));
                    clCodigo.BorderWidth = 0;
                    tblPrueba.AddCell(clCodigo);
                    value = dataGridView1.Rows[rows].Cells[2].Value.ToString();
                    Console.WriteLine("Cod:" + value);
                    clEmpresa = new PdfPCell(new Phrase(value, _standardFont));
                    clEmpresa.BorderWidth = 0;
                    tblPrueba.AddCell(clEmpresa);
            }
            doc.Add(tblPrueba);
            doc.Close();
            writer.Close();
            System.Diagnostics.Process.Start(@"C:\Pruebas REPORTES\prueba.pdf");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource=null;
            DbConnection.Open();
            string cadena2_1 = "SELECT EMPRESA, CODIGO_ACTIVO, NOMBRE_CUENTA_ACTIVO FROM ACTV_Ficha_Principal WHERE NOMBRE_CUENTA_ACTIVO LIKE ('";
            string cadena2_2 = "%')";
            string cadena2 = cadena2_1 + comboBox1.Text + cadena2_2;
            OdbcCommand DbCommand = new OdbcCommand(cadena2, DbConnection);
            OdbcDataAdapter adp1 = new OdbcDataAdapter();
            DataTable dt = new DataTable();
            adp1.SelectCommand = DbCommand;
            adp1.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
            }
            DbConnection.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Reporte R = new Reporte();
            Document doc = R.CreaDoc();
            iTextSharp.text.Font font = R.Fuente();
            PdfWriter writer = R.CreaWriter(doc);
            R.Iniciar(doc);
            R.Titulo(doc,"Reportes prueba");
            iTextSharp.text.Image img = R.Imagen();
            R.SetImagen(img,doc);

            // Lista de encabezados para reporte
            List<string> lista1 = new List<string>()
            {
                "Empresa",
                "Codigo",
                "Nombre"
            };

            // Se crea objetos en base a la lista de encabezados
            var celdas = new List<PdfPCell>();
            var Empresa = new PdfPCell();
            celdas.Add(Empresa);
            var Codigo = new PdfPCell();
            celdas.Add(Codigo);
            var Nombre = new PdfPCell();
            celdas.Add(Nombre);

            //Genera las columnas de la tabla en base a la lista
            PdfPTable Tabla = R.TablaPDF(lista1.Count);
            R.Contenido(lista1.Count, lista1, dataGridView1, Tabla, font);
            R.detalleContenido(dataGridView1, Tabla, font, doc, celdas);
            R.Cerrar(doc,writer);            
        }
    }
}
