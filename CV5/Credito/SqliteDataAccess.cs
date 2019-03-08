
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV5.Credito
{
    public class SqliteDataAccess
    {
        public static List<CabeceraCobros> CargarCabeceraCobros()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<CabeceraCobros>("Select * from BASECOBROS",new DynamicParameters()) ;
                return output.ToList();
            }
        }

        public static void SaveCabeceraCobros(CabeceraCobros cabecera) {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into BASECOBROS (FECHA_FACTURA,CODIGO_FACTURA," +
                    "CODIGO_COBRO,NOMBRE_CLIENTE,FECHA_PAGO,FECHA_DOCUMENTO," +
                    "FECHA_VENCIMIENTO,VALOR_COBRO,NUMERO_DOCUMENTO," +
                    "CUENTA_BANCARIA,FECHA_CHEQUE,FORMA_DE_PAGO," +
                    "CODIGO_COBRADOR) values (@FECHA_FACTURA,@CODIGO_FACTURA," +
                    "@CODIGO_COBRO,@NOMBRE_CLIENTE,@FECHA_PAGO,@FECHA_DOCUMENTO," +
                    "@FECHA_VENCIMIENTO,@VALOR_COBRO,@NUMERO_DOCUMENTO," +
                    "@CUENTA_BANCARIA,@FECHA_CHEQUE,@FORMA_DE_PAGO," +
                    "@CODIGO_COBRADOR)", cabecera);
            }
        }

        private static string LoadConnectionString(string id = "BASECOBROS")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
