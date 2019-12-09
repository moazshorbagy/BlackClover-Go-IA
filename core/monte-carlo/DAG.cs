using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackClover
{
    class DAG
    {
        // The root of the tree (Denotes the current game state)
        public Node root;
        public int totalNumberOfSimulations;

        /// <summary>Constructs the graph.</summary>
        /// <param name="state">The state of the node.</param>
        public DAG(State state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("Cannot insert null value!");
            }
            root = new Node(state);
            totalNumberOfSimulations = 0;
        }

        /// <summary>
        /// Traverses and prints the graph in Breadth-First Search (BFS) manner
        /// </summary>
        public int TraverseBFS()
        {
            if (root == null)
            {
                return 0;
            }
            int count = 0;
            Queue<Node> nodes = new Queue<Node>();
            Node tempNode;

            nodes.Enqueue(root);
            while(nodes.Count != 0)
            {
                count++;
                tempNode = nodes.Dequeue();
                // Console.WriteLine(tempNode.state);
                for (int i = 0; i < tempNode.children.Count; i++)
                {
                    nodes.Enqueue(tempNode.children[i]);
                }
            }
            return count;
        }
    }

    class Node
    {
        public State state;
        public List<Node> parents;
        public List<Node> children;
        public int numberOfSimulations;
        public float currentValue;
        public Action action;

        public Node(State state, Action action = null)
        {
            this.state = state ?? throw new ArgumentNullException("Cannot construct Node with null state value!");
            this.action = action; // ?? throw new ArgumentNullException("Cannot construct Node with null action value!");
            parents = new List<Node>();
            children = new List<Node>();
            numberOfSimulations = 0;
            currentValue = 0;
        }

        /// <summary>
        /// Determines if the node has any child node.
        /// </summary>
        /// <returns>True if it has any child.</returns>
        public bool hasChildNodes
        {
            get { return this.children.Count != 0; }
        }

        /// <summary>Adds parent to the node.</summary>
        /// <param name="parent">the parent to be added.</param>
        public void AddParent(Node parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException("Cannot addParent with null value!");
            }
            this.parents.Add(parent);
        }

        /// <summary>Adds a child to the node.</summary>
        /// <param name="child">The child to be added.</param>
        public void AddChild(Node child)
        {
            if (child == null)
            {
                throw new ArgumentNullException("Cannot addChild with null value!");
            }
            children.Add(child);
        }

        /// <summary>
        /// Gets the child of the node at given index.
        /// </summary>
        /// <param name="index">The index of the desired child.</param>
        /// <returns>The child on the given position.</returns>
        public Node GetChild(int index)
        {
            if(index >= children.Count)
            {
                throw new IndexOutOfRangeException($"Cannot getChild with index {index}!");
            }
            return children[index];
        }
    }
}
