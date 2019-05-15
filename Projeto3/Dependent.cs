using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto3
{
    class Dependent
    {
        public List<Dependent> dependents { get; set; }
        public Node node1 { get; set; }
        public Node node2 { get; set; }
    }
}
