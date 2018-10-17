using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CV5
{
    class Reporte
    {

        public Document CreaDoc() { 
        Document doc = new Document(PageSize.A4.Rotate());
            return doc;
        }

        public iTextSharp.text.Image Imagen() {
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(@"Resources\labovida.jpg");
            return img;
        }

        public void SetImagen(iTextSharp.text.Image img, Document doc) { 
        img.SetAbsolutePosition(100, 650);
        doc.Add(img);
        img.ScaleAbsolute(25f, 25F);
        }

        public iTextSharp.text.Font Fuente() { 
        // Creamos el tipo de Font que vamos utilizar
        iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(
                iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL,
                BaseColor.BLACK);
            return _standardFont;
        }

        public PdfWriter CreaWriter(Document doc) { 
        PdfWriter writer = PdfWriter.GetInstance(doc,
                                   new FileStream(@"C:\Pruebas REPORTES\prueba.pdf",
                                   FileMode.Create));
            return writer;
        }

        public void Iniciar(Document doc)
        {       
            doc.AddTitle("PDF LABO");
            doc.AddCreator("B. Vera");
            doc.Open();
        }

        public void Titulo(Document doc, string Titulo)
        {
            // Escribimos el encabezado en el documento
            doc.Add(new Paragraph(Titulo));
            doc.Add(Chunk.NEWLINE);
        }

        public PdfPTable TablaPDF (int columnas)
        {
            PdfPTable Tabla = new PdfPTable(columnas);
            return Tabla;
        }

        public float[] AnchoCols(Font font, params string[] headers)
        {
            var total = 0;
            var columns = headers.Length;
            var widths = new int[columns];
            for (var i = 0; i < columns; ++i)
            {
                var w = font.GetCalculatedBaseFont(true).GetWidth(headers[i]);
                total += w;
                widths[i] = w;
            }
            var result = new float[columns];
            for (var i = 0; i < columns; ++i)
            {
                result[i] = (float)widths[i] / total * 100;
            }
            return result;
        }

        public void Contenido(int columnas, List<string> encabezados,
            DataGridView dataGridView1, PdfPTable Tabla, iTextSharp.text.Font _standardFont)
        {
            //Se usa el 100% de la tabla 
            Tabla.WidthPercentage = 100;
            float[] widths = new float[] { 2f, 2f, 2f, 1f, 1f, 1f, 1f, 1f,1f, 1f, 1f, 1f,1f };
            Tabla.SetWidths(widths);

            //Ancho de cada celda
            // Configuramos el título de las columnas de la tabla
            for (int i = 0; i <= columnas-1; i++)
            {
                PdfPCell u = new PdfPCell(new Phrase(encabezados[i], _standardFont));
                u.BorderWidth = 0;
                u.BorderWidthBottom = 0.75f;
                // Añadimos las celdas a la tabla
                Tabla.AddCell(u);
               
            }
            
        }


        //Este metodo recoge los siguientes parametros:
        //          DATAGRIDVIEW: donde se esta realizando la consulta
        //          Tabla: Objeto tabla que se va a agregar al PDF
        //          _standardFont: Objeto de PDF# que contiene valores de fuente
        //          Celdas: Lista de celdas que contendran los valores 
        //
        //Este metodo genera un recorrido al DataGridView para ir poblando a las celdas

        public void detalleContenido(DataGridView dataGridView1,  PdfPTable Tabla,
                                    iTextSharp.text.Font _standardFont,  Document doc,
                                    List<PdfPCell> Celdas)
        {
               
                for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
                {
                    for (int i = 0; i < Celdas.Count; i++)
                    {
                        string value = dataGridView1.Rows[rows].Cells[i].Value.ToString();
                        Celdas[i] = new PdfPCell(new Phrase(value, _standardFont));
                        Celdas[i].BorderWidth = 0;
                        Celdas[i].BorderWidthBottom = 0.75f;
                        Tabla.AddCell(Celdas[i]);
                    }
               
                }

               doc.Add(Tabla);
            
        }

         

    public void Cerrar(Document doc, PdfWriter writer)
        {
            doc.Close();
            writer.Close();
            System.Diagnostics.Process.Start(@"C:\Pruebas REPORTES\prueba.pdf");
            doc.CloseDocument();
        }
    }
}
