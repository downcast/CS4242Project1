using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS4242Project1
{
    class Tree
    {
        public Node root { get; set; }
        private Random rand;

        public Tree (Node root)
        {
            this.root = root;
            this.rand = new Random();
        }

        public String depthSearch(String value)
        { return deepSearch(root, value, 0); }

        public String breadthSearch(String value)
        {
            Queue<Node> queue = new Queue<Node>();
            queue.Enqueue(root);

            return wideSearch(queue, value);
        }

        private String wideSearch(Queue<Node> queue, String Value)
        {
            String leafValue = null;
            Queue<Node> children = new Queue<Node>();

            // If there are no more children
            if (queue.Count == 0) { return leafValue; }

            // While there are node that havent been looked at
            while (queue.Count > 0)
            {   // see if node has the right value
                if (queue.Peek().value.ToLower().Equals(Value))
                { return leafValue = findLeaf(queue.Peek()); }
                else
                {   // Add children to the queue and remove the parent
                    foreach (Node n in queue.Peek().children)
                    { children.Enqueue(n);  }
                    queue.Dequeue();
                }
            }
            // Pass children and repeat
            return wideSearch(children, Value);
        }

        private String deepSearch(Node currentNode, String Value, int childIndex)
        {
            String leafValue = null;

            if (currentNode.value.ToLower().Equals(Value))
            {
                // Randomly select children until no child exist then return that child's value
                int index = rand.Next(currentNode.children.Count);
                return leafValue = findLeaf(currentNode.children[index]);
            }
            else
            {
                while (leafValue == null)
                {
                    if (currentNode.children.Count > childIndex)
                    {
                        leafValue = deepSearch(currentNode.children[childIndex], Value, 0);

                        // Go deeper
                        if (leafValue == null)
                        { childIndex++; }
                    }
                    // Hit leaf, go higher
                    else { return null; }
                }
            }
            // found correct leaf
            return leafValue;
        }

        private String findLeaf(Node currentNode)
        {
            if (currentNode.children.Count == 0)
            {
                // Found leaf
                return currentNode.value;
            }
            else
            {
                int index = rand.Next(currentNode.children.Count);
                return findLeaf(currentNode.children[index]);
            }
        }
    }
}
