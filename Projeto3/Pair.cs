using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto3
{
    class Pair
    {
        public Node node1 { get; set; }
        public Node node2 { get; set; }
        public List<Pair> listPairs { get; set; }
        public Pair()
        {

        }
        public Pair(Node node1, Node node2)
        {
            this.node1 = node1;
            this.node2 = node2;
            this.listPairs = new List<Pair>();
        }
    }
}
