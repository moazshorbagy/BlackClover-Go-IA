using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackClover_Go_IA.monte_carlo
{
    class DAG
    {
        // The root of the tree (Denotes the current game state)
        public Node root;
        public int totalNumberOfSimulations;

        /// <summary>Constructs the graph</summary>
        /// <param name="value">the value of the node</param>
        public DAG(State state)
        {
            if (state == null)
            {
                throw new ArgumentNullException("Cannot insert null value!");
            }
            root = new Node(state);
            totalNumberOfSimulations = 0;
        }

        /// <summary>Traverses and prints the graph in
        /// Breadth-First Search (BFS) manner</summary>
        public void TraverseBFS()
        {
            if (this.root == null)
            {
                return;
            }
            Queue<Node> nodes = new Queue<Node>();
            Node tempNode;

            nodes.Enqueue(this.root);
            while(nodes.Count != 0)
            {
                tempNode = nodes.Dequeue();
                //Console.WriteLine(tempNode.state.id);
                for (int i = 0; i < tempNode.children.Count; i++)
                {
                    nodes.Enqueue(tempNode.children[i]);
                }
            }
        }
    }

    class Node
    {
        public State state;
        public List<Node> parents;
        public List<Node> children;
        public int numberOfSimulations;
        public float currentValue;

        public Node(State state)
        {
            this.state = state ?? throw new ArgumentNullException("Cannot construct Node with null value!");
            parents = new List<Node>();
            children = new List<Node>();
            numberOfSimulations = 0;
            currentValue = 0;
        }

        /// <summary>
        /// Determines if the node has any child node
        /// </summary>
        /// <returns>true if has any</returns>
        public bool hasChildNodes
        {
            get { return this.children.Count != 0; }
        }

        /// <summary>Adds parent to the node</summary>
        /// <param name="child">the parent to be added</param>
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
            this.children.Add(child);
        }

        /// <summary>
        /// Gets the child of the node at given index.
        /// </summary>
        /// <param name="index">The index of the desired child.</param>
        /// <returns>The child on the given position</returns>
        public Node GetChild(int index)
        {
            if(index >= this.children.Count)
            {
                throw new IndexOutOfRangeException($"Cannot getChild with index {index}!");
            }
            return this.children[index];
        }

        /// <summary>
        /// Upper Confidence Trees.
        /// Maintains balance between the exploitation and exploration.
        /// </summary>
        public float calculateUCT()
        {
            return 0F;
        }
    }

    class State
    {
        public int id;
        public char[] board;
        public State(int id)
        {
            try
            {
                this.id = id;
                board = new char[361];
            } catch (OutOfMemoryException e)
            {
                Console.WriteLine(id);
                throw new OutOfMemoryException();
            }
        }
    }
}
