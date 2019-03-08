using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Data.Odbc;
using iTextSharp.text;
using clsConectaMBA;
using CV5.Properties;
using iTextSharp.text.pdf;
using System.Collections.Generic;

namespace CV5.Produccion
{
    public partial class frmEntregaParcial : Form
    {
        Reporte R = new Reporte();
        Funciones_Generales fg = new Funciones_Generales();

        public Chunk LineBreak { get; private set; }

        public frmEntregaParcial()
        {
            InitializeComponent();
            CargarDatos();
        }

        private void CargarDatos()
        {
            string query;
            query = "SELECT DOC_ID FROM INVT_AJUSTES_PRINCIPAL WHERE" +
                " CONFIRMED = CAST('True' as boolean) and" +
                " VOID = CAST('FALSE' as boolean) and" +
                " ADJUSTMENT_TYPE= 'PR' and " +
                " `CONFIRM DATE`  IS NOT NULL AND " +
                " (unidades_entregadas - unidades_a_elaborar) > 0 AND " +
                " STATUS_PROCESO_PROD = 'F' ";
            fg.LlenarCombo(query, cmbAcreedor, -1);
        }

        private void frmPagoProveedores_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            cmbAcreedor.Enabled = true;
            dtpFechAct.Enabled = true;
            dtpFechFin.Enabled = true;
            btnOk.Enabled = true;
            cmbAcreedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmbAcreedor.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbAcreedor.AutoCompleteSource = AutoCompleteSource.ListItems;
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
            ConexionMba cs = new ConexionMba();
            CleanGrid(dataGridView1);
            //if (!fg.CheckDatePicker(dtpFechAct, dtpFechFin))
            //{
            //    string Fech1 = dtpFechAct.Value.ToString("dd/MM/yyyy");
            //    string Fech2 = dtpFechFin.Value.ToString("dd/MM/yyyy");



                string cadena = "SELECT doc_id `# OP`, DATE_TO_CHAR(Date_i, 'dd[/]mm[/]yyyy') as `Fecha de produccion` , Memo," +
                                " DATE_TO_CHAR(`CONFIRM DATE`, 'dd[/]mm[/]yyyy') as `Fecha de confirmacion`," +
                                " nombre_producto_produc Producto, " +
                                " prod_id_corp_related `Codigo Producto`,  re_order_n `Pedido`,  unidades_a_elaborar `Unidades a elaborar`," +
                                " unidades_entregadas `Unidades entregadas`, " +
                                " unidades_entregadas - unidades_a_elaborar `Cant. Excedente`, `LOTE PRODUCCION` " +
                                " FROM INVT_AJUSTES_PRINCIPAL WHERE " +
                                " CONFIRMED = CAST('True' as boolean) and " +
                                "  VOID = CAST('FALSE' as boolean) and " +
                                " ADJUSTMENT_TYPE = 'PR' and " +
                                " `CONFIRM DATE`  IS NOT NULL AND " +
                                " STATUS_PROCESO_PROD = 'F' AND" +
                                " (unidades_entregadas - unidades_a_elaborar) > 0 AND "+
                                " DOC_ID = " + cmbAcreedor.Text + "";

                fg.FillDataGrid(cadena, dataGridView1);

            //}


        }



        private void cmbAcreedor_Leave(object sender, EventArgs e)
        {
            CheckCombo(cmbAcreedor, "empresa");
        }


        private void dtpFechFin_Leave(object sender, EventArgs e)
        {
            fg.CheckDatePicker(dtpFechAct, dtpFechFin);
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
       
                cmbAcreedor.Enabled = false;
           
        }

        private void cmbEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void btnPrint_Click(object sender, EventArgs e)
        {
            //Para generar reporte genera un objeto de clase
            Reporte R = new Reporte();
            //Genera un documento horizontal
            Document doc = R.CreaDoc(false);
            //Usa la fuente segun se requiera
            //Fuente para titulo
            Font _standardFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            Font font = R.Fuente(_standardFont);
            //Fuente para encabezados
            Font _EncstandardFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 6);
            Font fontEnc = R.Fuente(_EncstandardFont);
            //Generar un writer para el reporte
            var writer = R.CreaWriter(doc);
            //Inicia la apertura del documento y escritura
            R.Iniciar(doc);
            //Titulo
            R.Titulo(doc, "ENTREGA DE PRODUCTO EXCEDENTE", font);

            #region Cabecera 
            // Ancho de clmn
            float[] widths = new float[] { 7f, 9f, 3f, 3f };
            float[] widths0 = new float[] {4.5f,2f};
            var para = new Paragraph();
            para.Alignment = 2;
            para.Font.Size = 12;
            doc.Add(para);

            var cell1 = new PdfPCell();
            var phrase = new Phrase();
            phrase.Add(new Chunk("Orden produccion: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            phrase.Add(new Chunk(dataGridView1.Rows[0].Cells[0].Value.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12)));
            cell1.AddElement(phrase);

            var cell2 = new PdfPCell();
            var phrase2 = new Phrase();
            phrase2.Add(new Chunk("Entrega Parcial: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            phrase2.Add(new Chunk(dataGridView1.Rows[0].Cells[1].Value.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12)));
            cell2.AddElement(phrase2);

            var cell3 = new PdfPCell();
            var phrase3 = new Phrase();
            phrase3.Add(new Chunk("Cantidad a producir: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            phrase3.Add(new Chunk(dataGridView1.Rows[0].Cells[7].Value.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12)));
            cell3.AddElement(phrase3);

            var cell4 = new PdfPCell();
            var phrase4 = new Phrase();
            phrase4.Add(new Chunk("# Lote: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            phrase4.Add(new Chunk(dataGridView1.Rows[0].Cells[10].Value.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12)));
            cell4.AddElement(phrase4);

            var cell5 = new PdfPCell();
            var phrase5 = new Phrase();
            phrase5.Add(new Chunk("Pedido: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            phrase5.Add(new Chunk(dataGridView1.Rows[0].Cells[6].Value.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12)));
            cell5.AddElement(phrase5);
            
            PdfPTable table0 = new PdfPTable(2);
            table0.LockedWidth = true;
            table0.TotalWidth = 525f;
            PdfPCell cell0 = new PdfPCell();
            cell1.Border = PdfPCell.NO_BORDER;
            cell2.Border = PdfPCell.NO_BORDER;
            cell3.Border = PdfPCell.NO_BORDER;
            cell4.Border = PdfPCell.NO_BORDER;
            cell5.Border = PdfPCell.NO_BORDER;
            cell0.Colspan = 2;
            table0.AddCell(cell1);
            table0.AddCell(cell2);
            table0.AddCell(cell3);
            table0.AddCell(cell4);
            table0.AddCell(cell5);
            table0.SetWidths(widths0);
            
            doc.Add(table0);
            #endregion

            #region Detalle 
            PdfPTable table = new PdfPTable(4);
            PdfPCell cell = new PdfPCell();
            cell.Colspan = 4;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.LockedWidth= true;
            table.TotalWidth = 525f;
            table.SpacingBefore = 20f;
            table.SpacingBefore = 30f;
            table.AddCell(cell);
            table.AddCell(new Phrase (new Chunk("Codigo", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))));
            table.AddCell(new Phrase(new Chunk("Nombre Producto", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))));
            table.AddCell(new Phrase(new Chunk("Cant. producida", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))));
            table.AddCell(new Phrase(new Chunk("Cant. excedente", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))));
            table.AddCell(dataGridView1.Rows[0].Cells[5].Value.ToString());
            table.AddCell(dataGridView1.Rows[0].Cells[4].Value.ToString());
            table.AddCell(dataGridView1.Rows[0].Cells[8].Value.ToString());
            table.AddCell(dataGridView1.Rows[0].Cells[9].Value.ToString());
            table.SetWidths(widths);
            doc.Add(table);
            #endregion 
            
            #region Canvas de linea para firmas

            PdfPTable table3 = new PdfPTable(2);
            PdfPCell cellFirmas = new PdfPCell();
            cellFirmas.Colspan = 2;
            cellFirmas.HorizontalAlignment = 1; 
            table3.SpacingBefore = 20f;
            table3.SpacingAfter = 20f;
            table3.LockedWidth = true;
            table3.TotalWidth = 425f;
            

            var cell6 = new PdfPCell();
            var phrase8 = new Phrase();
            phrase8.Add(new Chunk("_________________", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell6.AddElement(phrase8);

            var cell7 = new PdfPCell();
            var phrase9 = new Phrase();
            phrase9.Add(new Chunk("Entrego conforme", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell7.AddElement(phrase9);

            var cell8 = new PdfPCell();
            var phrase10 = new Phrase();
            phrase10.Add(new Chunk("Recibo conforme", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            cell8.AddElement(phrase10);

            cellFirmas.Border = PdfPCell.NO_BORDER;
            cell6.Border = PdfPCell.NO_BORDER;
            cell6.Border = PdfPCell.NO_BORDER;
            cell7.Border = PdfPCell.NO_BORDER;
            cell8.Border = PdfPCell.NO_BORDER;
            table3.AddCell(cellFirmas);
            table3.AddCell(cell6);
            table3.AddCell(cell6);
            table3.AddCell(cell7);
            table3.AddCell(cell8);
            doc.Add(table3);
            #endregion

            #region Hora, fecha y Usuario            
            var phrase11 = new Phrase();
            phrase11.Add(new Chunk("Fecha/Hora Impresion: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            phrase11.Add(new Chunk(DateTime.Now.ToString()));
            doc.Add(new Paragraph(phrase11));

            var phrase12 = new Phrase();
            phrase12.Add(new Chunk("Usuario: ", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)));
            phrase12.Add(new Chunk(System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString()));
            doc.Add(new Paragraph(phrase12));
            #endregion


            doc.Close();
            writer.Close();
            System.Diagnostics.Process.Start(@"C:\Reporteria\ReporteGeneral.pdf");
            doc.CloseDocument();

        }


        //Formateo de celdas decimales para el datagrid 
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

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

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void cmbAcreedor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
