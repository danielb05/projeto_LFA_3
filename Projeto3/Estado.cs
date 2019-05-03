using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto3
{
    class Estado
    {
        private static int i = 0;
        public string name { get; set; }
        public List<Node> nodes { get; set; }
        public bool final { get; set; }
        public bool initial { get; set; }
        public bool visited { get; set; }

        public Estado(List<Node> nodes)
        {
            this.nodes = nodes;
            this.name = "S" + i;
            i++;
        }
        public Estado()
        {
            this.nodes = new List<Node>();
            this.name = "S" + i;
            i++;
        }

        public void AddNode(Node node)
        {
            this.nodes.Add(node);
        }

        public void zeraEstado()
        {
            i = 0;
        }
    }
}
