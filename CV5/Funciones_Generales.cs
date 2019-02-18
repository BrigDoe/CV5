using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using clsConectaMBA;

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

            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
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

        public void ExcelClick(DataGridView grd)
        {
            SaveFileDialog fichero = new SaveFileDialog();
            fichero.Filter = "Excel (*.xls)|*.xls";
            if (fichero.ShowDialog() == DialogResult.OK)
            {
                Microsoft.Office.Interop.Excel.Application aplicacion;
                Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;
                aplicacion = new Microsoft.Office.Interop.Excel.Application();
                libros_trabajo = aplicacion.Workbooks.Add();

                hoja_trabajo =
                    (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);
                //Recorremos el DataGridView rellenando la hoja de trabajo
                for (int i = 0; i < grd.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < grd.Columns.Count; j++)
                    {
                        hoja_trabajo.Cells[i + 1, j + 1] = grd.Rows[i].Cells[j].FormattedValue.ToString();
                    }
                }
                libros_trabajo.SaveAs(fichero.FileName,
                    Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                libros_trabajo.Close(true);
                aplicacion.Quit();
            }
        }
    }
}
