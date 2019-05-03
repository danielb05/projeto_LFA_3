using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto3
{
    class Graph
    {
        public List<Node> nodes { get; set; }
        public static int i;
        public string name;
        public Graph(string name)
        {
            i = 0;
            this.nodes = new List<Node>();
            this.name = name;
        }

        public void AddNode(Node node)
        {
            i = nodes.Count();
            if(node.name == null)
            {
                node.name = name + i;
                i++;
            }
            this.nodes.Add(node);
        }

        public Node getFinalNode()
        {
            foreach(Node n in nodes)
            {
                if (n.final)
                {
                    return n;
                }
            }
            return null;
        }

        public Node getInitialNode()
        {
            foreach (Node n in nodes)
            {
                if (n.initial)
                {
                    return n;
                }
            }
            return null;
        }

        public Node findNode(string name)
        {
            foreach(Node n in nodes)
            {
                if (n.name.Equals(name))
                {
                    return n;
                }
            }
            return null;
        }
    }

    
}
