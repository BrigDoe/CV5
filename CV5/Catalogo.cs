using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using ZXing;
using System.Drawing;
using System.IO;
using Org.BouncyCastle.Utilities.Encoders;
using clsConectaMBA;

namespace CV5
{
    public partial class Catalogo : Form
    {
        Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();

        public Catalogo()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            string query = "SELECT  `CORPORATION NAM` FROM SIST_Parametros_Empresa ";
            fg.LlenarCombo(query, cmbEmpresa);
            
        }

        private void frmPagoProveedores_Load(object sender, EventArgs e)
        {
            btnOk.Enabled = true;
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

        private void btnOk_Click(object sender, EventArgs e)
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
            string cadena = "SELECT PRODUCT_ID AS `CODIGO`, DESCRIPTION AS `NOMBRE`, CODE_PROV_O_ALT `CODIGO BARRAS`, img.Fotografia AS `IMAGEN` " +
                " FROM   {oj INVT_FICHA_PRINCIPAL inv LEFT OUTER JOIN SIST_IMAGENES img ON img.REFERENCIA = " +
                " inv.PRODUCT_ID_CORP } WHERE(CODE_PROV_O_ALT <> '') AND(PRODUCT_ID_CORP LIKE '%LABOV') ORDER BY DESCRIPTION";
            //string cadena = "SELECT DISTINCT PRODUCT_ID AS CODIGO, DESCRIPTION AS NOMBRE, CODE_PROV_O_ALT AS `CODIGO BARRAS`, img.Fotografia AS `IMAGEN` FROM   {oj INVT_FICHA_PRINCIPAL inv LEFT OUTER JOIN SIST_IMAGENES img ON img.REFERENCIA = inv.PRODUCT_ID_CORP } WHERE(CODE_PROV_O_ALT <> '') AND (PRODUCT_ID_CORP LIKE '%LABOV') AND(PRODUCT_ID LIKE '%GV000166%') OR (PRODUCT_ID LIKE 'GV000180') ORDER BY DESCRIPTION";
            dataGridView1.RowTemplate.Height = 120;
            fg.FillDataGrid(cadena, dataGridView1);
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.EAN_13,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 120,
                    Height = 60
                }
            };
          

            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn = (DataGridViewImageColumn)dataGridView1.Columns[3];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridViewImageColumn Barras = new DataGridViewImageColumn();
            dataGridView1.Columns.Add(Barras);
            imageColumn = (DataGridViewImageColumn)dataGridView1.Columns[4];
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Normal;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                string content = dataGridView1.Rows[i].Cells[2].Value.ToString();
                dataGridView1.Rows[i].Cells[4].Value = writer.Write(content);
            }
            dataGridView1.Columns[0].HeaderText = "Código";
            dataGridView1.Columns[1].HeaderText = "Producto";
            dataGridView1.Columns[3].HeaderText = "Imagen";
            dataGridView1.Columns[4].HeaderText = "Cod. Barras";
            dataGridView1.Columns[2].Visible = false;
        }

        private void cmbEmpresa_Leave(object sender, EventArgs e)
        {
            CheckCombo(cmbEmpresa, "empresa");
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
            R.Titulo(doc, "Catalogo de productos", font);
            // Inserta imagen EN DESARROLLO
            //Image img = R.Imagen();
            //R.SetImagen(img, doc);
            //Settear anchos de la tabla en base a los encabezados
            //Se debe tener el numero exacto de encabezados que se presentan
            float[] widths = new float[] {2f, 2f, 2f, 2f};
            //Se cambia la fuente para el contenidol reporte
            _standardFont = FontFactory.GetFont(FontFactory.HELVETICA, 6);
            font = R.Fuente(_standardFont);
            // Lista de encabezados y contenido para reporte
            // desde el datagridView 
            R.CreaReport(dataGridView1, font, fontEnc, doc, writer, widths);
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
                catch (System.FormatException)
                {
                    formatting.FormattingApplied = false;

                }
            }
        }



        private void btnSearch_Click(object sender, EventArgs e)
        {
            System.Drawing.Image im = fg.Base64ToImage(dataGridView1.Rows[0].Cells[3].Value.ToString());
            byte[] ima = fg.ImageToByteArray(im);
            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(ima);
            // Convert base 64 string to byte[]          
            Document doc = new Document(PageSize.A4);
            PdfWriter.GetInstance(doc, new FileStream("image.pdf", FileMode.Create));
            doc.Open();
            PdfPTable table = new PdfPTable(1);
            PdfPCell cell = new PdfPCell();
            cell.AddElement(imagen);
            table.AddCell(cell);
            doc.Add(table);
            doc.Close();
        }


        private Bitmap ImgRandom() { 
        Bitmap standardImage = new Bitmap(200, 200);
        Graphics standardGraphics = Graphics.FromImage(standardImage);
        int red = 0;
        int white = 10;
        while (white <= 200)
        {
            standardGraphics.FillRectangle(Brushes.Red, 0, red, 200, 10);
            standardGraphics.FillRectangle(Brushes.White, 0, white, 200, 10);
            red += 20;
            white += 20;
        }
            return standardImage;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            var Font = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            //Obtiene el nombre del mes
            string Mes = new DateTime(datevalue.Year, datevalue.Month, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            Document doc = new Document(PageSize.A4);
            PdfWriter.GetInstance(doc, new FileStream("image.pdf", FileMode.Create));
            doc.Open();
            Chunk Titulo = new Chunk("Catálogo GRUPO LABOVIDA", boldFont);
            Paragraph _Titulo = new Paragraph(Titulo);
            _Titulo.Alignment = Element.ALIGN_CENTER;
            doc.Add(_Titulo);
            doc.Add(Chunk.NEWLINE);
            Chunk Fecha = new Chunk(char.ToUpper(Mes[0])+Mes.Substring(1) + " " + datevalue.Year.ToString());
            Paragraph _Fecha = new Paragraph(Fecha);
            _Fecha.Alignment = Element.ALIGN_CENTER;
            doc.Add(_Fecha);
            doc.Add(Chunk.NEWLINE);
            PdfPTable table = new PdfPTable(4);
            table.SetWidths(new int[] { 2, 3, 2, 3 });
            byte[] byt2;
            byte[] byt;
            for (int i = 0; i < (dataGridView1.RowCount -1); i++)
            {
                //Castear la imagen del datagrid en byte
                if(dataGridView1.Rows[i].Cells[3].Value != null  && dataGridView1.Rows[i].Cells[3].Value.ToString().Length > 0) { 
                byt = (byte[])dataGridView1.Rows[i].Cells[3].Value;
                } else
                {
                    ImageConverter converter = new ImageConverter();
                    byt = (byte[])converter.ConvertTo(ImgRandom(), typeof(byte[]));
                }

                if (dataGridView1.Rows[i+1].Cells[3].Value != null && dataGridView1.Rows[i+1].Cells[3].Value.ToString().Length > 0)
                {
                    byt2 = (byte[])dataGridView1.Rows[i+1].Cells[3].Value;
                }
                else
                {
                    ImageConverter converter = new ImageConverter();
                    byt2 = (byte[])converter.ConvertTo(ImgRandom(), typeof(byte[]));
                }

            //Crea object stream con el byte de la imagen
            MemoryStream ms = new MemoryStream(byt);
            MemoryStream ms2 = new MemoryStream(byt2);
            //Genera una imagen del sistema
            System.Drawing.Image sdi = System.Drawing.Image.FromStream(ms);
            System.Drawing.Image sxi = (System.Drawing.Image)dataGridView1.Rows[i].Cells[4].Value;
            System.Drawing.Image sdi2 = System.Drawing.Image.FromStream(ms2);
            System.Drawing.Image sxi2 = (System.Drawing.Image)dataGridView1.Rows[i+1].Cells[4].Value;
            //Crea la instancia de imagen de iTextSharp
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(sdi, System.Drawing.Imaging.ImageFormat.Bmp);
            iTextSharp.text.Image img2 = iTextSharp.text.Image.GetInstance(sxi, System.Drawing.Imaging.ImageFormat.Bmp);
            iTextSharp.text.Image img3 = iTextSharp.text.Image.GetInstance(sdi2, System.Drawing.Imaging.ImageFormat.Bmp);
            iTextSharp.text.Image img4 = iTextSharp.text.Image.GetInstance(sxi2, System.Drawing.Imaging.ImageFormat.Bmp);
            PdfPTable table2 = new PdfPTable(1);
            PdfPTable table3 = new PdfPTable(1);
            PdfPCell cell = new PdfPCell();
            PdfPCell cell2 = new PdfPCell();
            PdfPCell cell3 = new PdfPCell();
            PdfPCell cell4 = new PdfPCell();
            PdfPCell cell5 = new PdfPCell();
            PdfPCell cell6 = new PdfPCell();
            cell.AddElement(img);
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table2.AddCell(cell);
            cell2.AddElement(img2);
            cell2.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            table2.AddCell(cell2);
            table.AddCell(table2);
            cell3.AddElement(new Phrase(dataGridView1.Rows[i].Cells[0].Value.ToString(), boldFont));
            cell3.AddElement(new Phrase(dataGridView1.Rows[i].Cells[1].Value.ToString(),Font));
            table.AddCell(cell3);
            cell4.AddElement(img3);
            cell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table3.AddCell(cell4);
            cell5.AddElement(img4);
            cell5.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            table3.AddCell(cell5);
            table.AddCell(table3);
            cell6.AddElement(new Phrase(dataGridView1.Rows[i+1].Cells[0].Value.ToString(), boldFont));
            cell6.AddElement(new Phrase(dataGridView1.Rows[i+1].Cells[1].Value.ToString(),Font));
            table.AddCell(cell6);
                ms.Dispose();
                ms2.Dispose();
            }
            doc.Add(table);
            doc.Close();
            System.Diagnostics.Process.Start(@"C:\Users\bryan\source\repos\CV5\CV5\bin\Debug\image.pdf");
        }

    }
}


