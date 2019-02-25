using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using clsConectaMBA;
using CV5.Roles;

namespace CV5
{
    class Funciones_Generales
    {
        
        public void LlenarCombo(String Sql, ComboBox cb, int index=0) { 
            cb.MaxDropDownItems = 6;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            cb.IntegralHeight = false;
            cb.Enabled=true;
            try
            {
                Fill(Sql, cb, 0);
                if (cb.Items.Count > 0) { 
                cb.SelectedIndex = index;
                }
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

        public void BuscarGrid(DataGridView dg, TextBox txtBuscar)
        {
            if (txtBuscar.Visible == false) { txtBuscar.Visible = true; } else { txtBuscar.Visible = false; }
            txtBuscar.Text = "Texto a buscar...";
            txtBuscar.SelectAll();
            txtBuscar.Focus();
            txtBuscar.KeyDown += (sender, e) => tb_KeyDown(sender, e, dg, txtBuscar);
        }

        void tb_KeyDown(object sender, KeyEventArgs e, DataGridView dataGridView1,TextBox txtBuscar)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BuscarDataGrid(dataGridView1, txtBuscar.Text.ToUpper());
            }
        }


        public void LimpiarGrid(DataGridView dg)
        {
            dg.DataSource = null;
            dg.Refresh();
        }

        public string EjecutarQuery(string query)
        {
            ConexionMba cs = new ConexionMba();
            OdbcCommand DbCommand = new OdbcCommand(query, cs.getConexion());
            OdbcDataReader reader = DbCommand.ExecuteReader();
            string _value = "";
            while (reader.Read())
            {
                _value = reader.GetString(0);
            }
            return _value;
        }

        public void BuscarDataGrid(DataGridView dg1, string txt_a_buscar)
        {
            int rowIndex = -1;            
            string searchValue = txt_a_buscar;
            dg1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow row in dg1.Rows)
                {
                    while (row.Cells[1].Value.ToString().Contains(searchValue))
                    {
                        rowIndex = row.Index;
                        dg1.Rows[rowIndex].Selected = true;      
                        break;
                    }
                   
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }




        private void Fill(string query, ComboBox cb, int a)
        {
            ConexionMba cn = new ConexionMba();
            OdbcCommand DbCommand = new OdbcCommand(query, cn.getConexion());
            OdbcDataAdapter adp1 = new OdbcDataAdapter();
            DataTable dt = new DataTable();
            DbCommand.Connection = cn.getConexion();
            DbCommand.CommandText = query;
            adp1.SelectCommand = DbCommand;
            adp1.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cb.Items.Add(dt.Rows[i][a].ToString());
                }
            }
            cn.cerrarConexion();
        }


       
        public void FillDataGrid(string cadena, DataGridView dataGridView1)
        {
            dataGridView1.AllowUserToAddRows = false;
            ConexionMba cn = new ConexionMba();
            OdbcCommand DbCommand = new OdbcCommand(cadena, cn.getConexion());
            OdbcDataAdapter adp1 = new OdbcDataAdapter();
            DataTable dt = new DataTable();
            adp1.SelectCommand = DbCommand;
            adp1.Fill(dt);
            if (dt.Rows.Count >= 0)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                AutoSizeGrid(dataGridView1);
            }
            cn.cerrarConexion();
        }

        public void FormatoGrid(List<int> list, DataGridView dgv)
        {
            foreach (int cols in list)
            {
                dgv.Columns[cols].DefaultCellStyle.Format = "N2";
            }
        }



        public void FillDataGridNewRow(string cadena, DataGridView dataGridView1,
                                        DataRow NuevaRow)
        {

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = false;
            ConexionMba cn = new ConexionMba();
            OdbcCommand DbCommand = new OdbcCommand(cadena, cn.getConexion());
            OdbcDataAdapter adp1 = new OdbcDataAdapter();
            DataTable dt = new DataTable();
            dt.Rows.InsertAt(NuevaRow, dt.Rows.Count);
            adp1.SelectCommand = DbCommand;

            adp1.Fill(dt);
            
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
            }
            AutoSizeGrid(dataGridView1);
            cn.cerrarConexion();
        }



        public void FillDataGridImg(string cadena, DataGridView dataGridView1)
        {

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = false;
            ConexionMba cn = new ConexionMba();
            OdbcCommand DbCommand = new OdbcCommand(cadena, cn.getConexion());
            OdbcDataAdapter adp1 = new OdbcDataAdapter();
            DataTable dt = new DataTable();
            adp1.SelectCommand = DbCommand;
            adp1.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
            }
            cn.cerrarConexion();
        }

        public void AutoSizeGrid(DataGridView dataGridView)
        {

            for (int i = 0; i < dataGridView.ColumnCount; i++)
            {
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                int widthCol = dataGridView.Columns[i].Width;
                dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView.Columns[i].Width = widthCol;               
            }
            //Loading.Close();
        }

        private void myStartingMethod()
        {
            
        }

        public Image Base64ToImage(string base64String)
        {
            // Convertie base 64 string a byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        public byte[] ImageToByte(System.Drawing.Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();
                byteArray = stream.ToArray();
            }
            return byteArray;
        }


        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
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

        public void ClearCombo(ComboBox cb1)
        {
            cb1.Items.Clear();
        }

        private void copyAlltoClipboard(DataGridView grd)
        {
            grd.SelectAll();
            DataObject dataObj = grd.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }


        public void ExcelClick(DataGridView grd)
        {
            //Copy to clipboard
            grd.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            copyAlltoClipboard(grd);
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
