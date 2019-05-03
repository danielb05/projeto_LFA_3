using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto3
{
    class Node
    {
        public string name  { get; set; }
        public List<Edge> edges { get; set; }
        public List<Node> parents { get; set; }
        public bool initial { get; set; }
        public bool final { get; set; }

        public Node(Node parent)
        {
            this.edges = new List<Edge>();
            this.parents = new List<Node>();
            parents.Add(parent);
        }

        public Node()
        {
            this.parents = new List<Node>();
            this.edges = new List<Edge>();
        }

        public Node(string name)
        {
            this.parents = new List<Node>();
            this.edges = new List<Edge>();
            this.name = name;
        }

        public void AddEdge(Edge edge)
        {
            this.edges.Add(edge);
            if(edge.to.findParent(edge.from) == null)
            {
                edge.to.AddParent(edge.from);
            }
        }

        public void AddParent(Node parent)
        {
            this.parents.Add(parent);
        }

        public Node findParent(Node parent)
        {
            foreach(Node p in parents)
            {
                if(p.name == parent.name)
                {
                    return p;
                }
            }
            return null;
        }
    }
}
