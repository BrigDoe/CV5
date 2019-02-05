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
        
        //Este metodo recepta:
        //      flag: Booleano que de ser True convierte al reporte en horizontal 
        public Document CreaDoc(Boolean flag) { 
        Document doc = new Document(PageSize.A4);
            if (flag)
                doc.SetPageSize(PageSize.A4.Rotate());
            return doc;
        }

        public iTextSharp.text.Image Imagen(String ruta) {
            //@"Resources\labovida.jpg"
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(ruta);
            return img;
        }
        
        public void Logo(string ruta, Document doc) {
        iTextSharp.text.Image img = Imagen(ruta);
        img.SetAbsolutePosition(100, 650);
        doc.Add(img);
        img.ScaleAbsolute(25f, 25F);
        }

        public iTextSharp.text.Font Fuente(Font _font) {
            iTextSharp.text.Font _standardFont = _font;
        return _standardFont;
        }

        public iTextSharp.text.Font Fuente_B()
        {
            // Creamos el tipo de Font que vamos utilizar
            iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(
                    iTextSharp.text.Font.FontFamily.HELVETICA, 6, iTextSharp.text.Font.BOLD,
                    BaseColor.BLACK);
            return _standardFont;
        }


        public PdfWriter CreaWriter(Document doc) { 
        PdfWriter writer = PdfWriter.GetInstance(doc,
                                   new FileStream(@"C:\Reporteria\ReporteGeneral.pdf",
                                   FileMode.Create));
            return writer;
        }

        public void Iniciar(Document doc)
        {       
            doc.AddTitle("PDF LABO");
            doc.AddCreator("B. Vera");
            doc.Open();
        }

        public void Titulo(Document doc, string Titulo, iTextSharp.text.Font _standardFont)
        {
            Chunk _Titulo = new Chunk(Titulo, _standardFont);
            Paragraph __Titulo = new Paragraph(_Titulo);
            __Titulo.Alignment = Element.ALIGN_CENTER;
            doc.Add(__Titulo);
            doc.Add(Chunk.NEWLINE);
        }

        public PdfPTable TablaPDF (int columnas)
        {
            PdfPTable Tabla = new PdfPTable(columnas);
            //Crea borders del lado izquierdo y derecho de la celda
            Tabla.DefaultCell.Border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
            Tabla.HeaderRows = 1;
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
            DataGridView dataGridView1, PdfPTable Tabla,
            float[] widths, iTextSharp.text.Font _standardFont)
        {
       
            //Se usa el 100% de la tabla 
            Tabla.WidthPercentage = 100;
            Tabla.SetWidths(widths);

            //Ancho de cada celda
            // Configuramos el título de las columnas de la tabla
            for (int i = 0; i <= columnas-1; i++)
            {
                PdfPCell u = new PdfPCell(new Phrase(encabezados[i], _standardFont));
                u.BorderWidth = 0.5f;
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
                        string value = dataGridView1.Rows[rows].Cells[i].FormattedValue.ToString();
                        Celdas[i] = new PdfPCell(new Phrase(value, _standardFont));
                        Celdas[i].BorderWidth = 0.2f;
                        Celdas[i].BorderWidthBottom = 0.75f;
                        Tabla.AddCell(Celdas[i]);
                    }
               
                }

               doc.Add(Tabla);
               //Genera encabezados en las siguientes paginas 
               Tabla.HeaderRows = 1;
               
        }

        public void LineaTexto(Document doc, string linea ,iTextSharp.text.Font font)
        {
            Chunk Line = new Chunk(linea, font);
            Paragraph _Line = new Paragraph(Line);
            _Line.Alignment = Element.ALIGN_CENTER;
            doc.Add(_Line);
            doc.Add(Chunk.NEWLINE);
        }


        public void CreaReport(DataGridView dg, iTextSharp.text.Font font, iTextSharp.text.Font fontEnc,
                                Document doc, PdfWriter writer, float[] widths)
        {
            List<string> lista1 = Encabezados(dg);
            var celdas = new List<PdfPCell>();
            celdas = SetCeldasPDF(lista1);
            PdfPTable Tabla = TablaPDF(lista1.Count);
            Contenido(lista1.Count, lista1, dg, Tabla, widths,fontEnc);
            detalleContenido(dg, Tabla, font, doc, celdas);
            Cerrar(doc, writer);
        }

        public List<PdfPCell> SetCeldasPDF(List<string> lista1)
        {
            var celdas = new List<PdfPCell>();
            var _celdas = new PdfPCell();
            for (int i = 0; i < lista1.Count; i++)
            {
                celdas.Add(_celdas);
            }
            return celdas;
        }

        public List<string> Encabezados(DataGridView dg)
        {
            var celdas = new List<PdfPCell>();
            List<string> lista1 = new List<string>();
            for (int i = 0; i < dg.ColumnCount; i++)
            {
                lista1.Add(dg.Columns[i].HeaderText.ToString());
            }
            return lista1;
        }



        public void Cerrar(Document doc, PdfWriter writer)
        {
            doc.Close();
            writer.Close();
            System.Diagnostics.Process.Start(@"C:\Reporteria\ReporteGeneral.pdf");
            doc.CloseDocument();
        }
    }
}
