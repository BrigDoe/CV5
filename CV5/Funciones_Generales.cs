using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CV5
{
    class Funciones_Generales
    {
        OdbcConnection DbConnection = new OdbcConnection("Dsn=MBA3 PRUEBA12;Driver={4D v12 ODBC Driver};server=192.168.1.2;port=19819;");

        public void LlenarCombo(String Sql, ComboBox cb, int index=0) {
            cb.MaxDropDownItems = 6;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.IntegralHeight = false;
            cb.Enabled=true;
             try
            {
                DbConnection.Open();
                Fill(Sql, cb, 0);
                cb.SelectedIndex = index;
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
        
        private void Fill(string query, ComboBox cb, int a)
        {
            OdbcCommand DbCommand = new OdbcCommand(query, DbConnection);
            OdbcDataAdapter adp1 = new OdbcDataAdapter();
            DataTable dt = new DataTable();
            DbCommand = new OdbcCommand(query, DbConnection);
            adp1 = new OdbcDataAdapter();
            dt = new DataTable();
            adp1.SelectCommand = DbCommand;
            adp1.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cb.Items.Add(dt.Rows[i][a].ToString());
            }
            DbConnection.Close();
        }

        public void FillDataGrid(string cadena, DataGridView dataGridView1,
                                  OdbcConnection DbConnection)
        {
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            OdbcCommand DbCommand = new OdbcCommand(cadena, DbConnection);
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

        public Boolean CheckDatePicker(DateTimePicker dtpinicio, DateTimePicker dtpfin)
        {
            if (dtpinicio.Value >= dtpfin.Value)
            {
                MessageBox.Show("La fecha de inicio no puede ser mayor a la de corte.", "Informacion",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true; 
            }
            else
            {
                return false;
            }
        }

        private void copyAlltoClipboard(DataGridView dg)
        {
            dg.SelectAll();
            DataObject dataObj = dg.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        public void ExcelClick(DataGridView dg)
        {
            copyAlltoClipboard(dg);
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }
    }
}
