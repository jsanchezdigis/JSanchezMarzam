using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class CompraMedicamento
    {
        public int IdCompraMedicamento { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public ML.Medicamento Medicamento { get; set; }
        public ML.Usuario Usuario { get; set; }
        public List<object> Compras { get; set; }
    }
}
