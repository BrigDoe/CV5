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
        Document doc = new Document(PageSize.A4);
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
        
        public void Contenido(int columnas, List<string> encabezados,
            DataGridView dataGridView1, PdfPTable Tabla, iTextSharp.text.Font _standardFont)
        {
                // Creamos una tabla que contendrá el nombre, apellido y país
                // de nuestros visitante.
            Tabla.WidthPercentage = 50;
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
                    for (int i = 0; i <= Celdas.Count-1; i++)
                    {
                        string value = dataGridView1.Rows[rows].Cells[i].Value.ToString();
                        Celdas[i] = new PdfPCell(new Phrase(value, _standardFont));
                        Celdas[i].BorderWidth = 0;
                        Celdas[i].BorderWidthBottom = 0.75f;
                        Tabla.AddCell(Celdas[i]);
                    }
                    //string value = dataGridView1.Rows[rows].Cells[0].Value.ToString();
                    //Celda = new PdfPCell(new Phrase(value, _standardFont));
                    //Celda.BorderWidth = 0;  
                    //Celda.BorderWidthBottom = 0.75f;
                    //Tabla.AddCell(Celda);
                    //value = dataGridView1.Rows[rows].Cells[1].Value.ToString();
                    //Celda = new PdfPCell(new Phrase(value, _standardFont));
                    //Celda.BorderWidth = 0;
                    //Celda.BorderWidthBottom = 0.75f;
                    //Tabla.AddCell(Celda);
                    //value = dataGridView1.Rows[rows].Cells[2].Value.ToString();
                    //Celda = new PdfPCell(new Phrase(value, _standardFont));
                    //Celda.BorderWidth = 0;
                    //Celda.BorderWidthBottom = 0.75f;
                    //Tabla.AddCell(Celda);
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
