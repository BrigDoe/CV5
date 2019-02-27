using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CV5.Credito
{
    class CabeceraCobros
    {
        public int ID { get; set; }
        public string FECHA_FACTURA { get; set; }
        public string CODIGO_FACTURA { get; set; }
        public string NOMBRE_CLIENTE { get; set; }
        public string FECHA_PAGO { get; set; }
        public string FECHA_DOCUMENTO { get; set; }
        public string FECHA_VENCIMIENTO { get; set; }
        public float VALOR_COBRO { get; set; }
        public string NUMERO_DOCUMENTO { get; set; }
        public string CUENTA_BANCARIA { get; set; }
        public string FECHA_CHEQUE { get; set; }
        public string FORMA_DE_PAGO { get; set; }
        public string CODIGO_COBRADOR { get; set; }
    }
}
