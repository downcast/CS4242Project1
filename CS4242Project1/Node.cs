using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4242Project1
{

    class Node
    {
        public String value { get; set; }
        public Node parent { get; set; }
        public List<Node> children { get; set; }

        public Node(String value)
        {
            this.value = value;
            children = new List<Node>();
        }
    }
}
