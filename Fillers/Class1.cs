using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Windows.Forms;

namespace Fillers
{
    public class Fill
    {



        private void LlenarCombo(string query, ComboBox cb, int a)
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
    }
}
