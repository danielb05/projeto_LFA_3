using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto3
{
    class Edge
    {
        public Node from { get; set; }
        public Node to { get; set; }
        public char value { get; set; }

        public Edge(Node from, Node to, char value)
        {
            this.from = from;
            this.to = to;
            this.value = value;
        }
    }
}
