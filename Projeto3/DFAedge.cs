using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto3
{
    class DFAedge
    {
        public Estado estado { get; set; }
        public char simbolo { get; set; }
        public Estado result { get; set; }

        public DFAedge(Estado estado, char simbolo, Estado result)
        {
            this.estado = estado;
            this.simbolo = simbolo;
            this.result = result;
        }
    }
}
