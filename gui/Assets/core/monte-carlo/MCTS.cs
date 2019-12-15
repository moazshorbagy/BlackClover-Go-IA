using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackClover
{
    class MCTS
    {
        private readonly State initialState;
        private readonly DAG tree;
        private readonly int limit;
        private readonly int depthLimit;
        private readonly Random random;

        /// <summary>
        /// <para>Class representing the Monte Carlo simulation.</para>
        /// <para>Handles best-move selection.</para>
        /// </summary>
        /// <param name="initialState">The initial state of the game.</param>
        public MCTS(State initialState)
        {
            this.initialState = initialState;
            tree = new DAG(this.initialState);
            limit = 400;
            depthLimit = 25;
            random = new Random();
        }

        /// <summary>
        ///  Handles the four MCTS steps: selection, expansion, simulation, backpropagation.
        /// </summary>
        /// <returns>The best action to be taken</returns>
        public Action Play()
        {
            int time = 0; // keeps track of number of simulations

            Node currentNode;

            while (time < limit)
            {
                currentNode = tree.root; // the first selection is the actual current state of the game
                
                while (currentNode.hasChildNodes) // if true then it is not a leaf node
                {
                    // Selection
                    currentNode = Select(currentNode);
                }
                
                if (currentNode.numberOfSimulations != 0)
                {
                    // Expansion
                    Expand(currentNode);
                    currentNode = currentNode.children[0];
                }
                // Simulation
                int value = Simulate(currentNode);
                
                // Backpropagation
                BackPropagate(currentNode, value);

                time++;
            }

            // Finding the best action after simulation is terminated
            float maxCurrentValue = tree.root.children[0].currentValue;
            int index = 0;
            int secondMaxIndex = 1;

            for (int i = 1; i < tree.root.children.Count; i++)
            {
                if (tree.root.children[i].currentValue > maxCurrentValue)
                {
                    secondMaxIndex = index;
                    maxCurrentValue = tree.root.children[i].currentValue;
                    index = i;
                }
            }
            /*
            // Check for KO
            if (!problem.ValidateAction(tree.root.children[index].state))
            {
                index = secondMaxIndex;
            }*/
            Console.WriteLine("Tree nodes count = {0}", tree.TraverseBFS());
            for (int i = 0; i < tree.root.children.Count; i++)
            {
                Console.WriteLine("# sim = {0}, value = {1}", tree.root.children[i].numberOfSimulations, tree.root.children[i].currentValue);
            }

            tree.root = tree.root.children[index]; // modifying the root
            tree.root.parents.Clear(); // cutting the rest of the tree
            return tree.root.action;
        }

        public bool OponentPlay(State state)
        {
            bool stateFound = false;
            for (int k = 0; k < tree.root.children.Count; k++)
            {
                stateFound = true;
                for (int i = 0; i < 19; i++)
                {
                    if(!stateFound)
                    {
                        break;
                    }
                    for (int j = 0; j < 19; j++)
                    {
                        if(state.GetBoard()[i, j] != tree.root.children[k].state.GetBoard()[i, j])
                        {
                            stateFound = false;
                            break;
                        }
                    }
                }
                if(stateFound)
                {
                    tree.root = tree.root.children[k]; // modifying the root
                    tree.root.parents.Clear(); // cutting the rest of the tree
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Implements the Tree Policy.
        /// Chooses the node (coming from the action) that maximizes Q + U.
        /// </summary>
        /// <remarks>It is responsible for balancing the trade-off between exploration and exploitation.</remarks>
        private Node Select(Node node)
        {
            float maxUCB = CalculateUCB(node.children[0]);
            float currUCB;
            int index = 0;

            for (int i = 0; i < node.children.Count; i++)
            {
                if (node.children[i].numberOfSimulations == 0)
                {
                    return node.children[i];
                }
                currUCB = CalculateUCB(node.children[i]);
                if (currUCB > maxUCB)
                {
                    maxUCB = currUCB;
                    index = i;
                }
            }
            return node.children[index];
        }

        /// <summary>
        /// Upper Confidence Bound.
        /// Maintains balance between the exploitation and exploration.
        /// </summary>
        private float CalculateUCB(Node node)
        {
            return node.currentValue / node.numberOfSimulations + (float)(2 * Math.Sqrt(Math.Log(node.parents[0].numberOfSimulations) / node.numberOfSimulations));
        }

        /// <summary>
        /// Adds nodes to the tree
        /// </summary>
        private void Expand(Node node)
        {
            List<Action> actions = Action.PossibleActions(node.state);
            State successor;
            List<GUIAction> _;
            for (int i = 0; i < actions.Count; i++)
            {
                (_, successor) = node.state.GetSuccessor(actions[i]);
                node.AddChild(new Node(successor, actions[i]));
                node.children[i].AddParent(node);
            }
        }

        /// <summary>
        /// Rollouts from the node expanded in the previous phase until a terminal state is reached.
        /// This is done according to the rollout policy (currently random)
        /// </summary>
        /// <returns>The value that will be back-propagated</returns>
        private int Simulate(Node node)
        {
            State state = node.state;
            List<Action> actions;
            List<GUIAction> _;
            int count = 0;
            while (count < depthLimit && !state.IsTerminal())
            {
                count++;
                actions = Action.PossibleActions(state);
                (_, state) = state.GetSuccessor(actions[random.Next(actions.Count)]);
            }
            return state.Evaluate(1); //problem.GetWinner(state);
        }

        /// <summary>
        /// The result of the simulation is propagated back through the tree to the
        /// root node, updating the value and visit count of each node along the way.
        /// </summary>
        private void BackPropagate(Node node, int value)
        {
            tree.totalNumberOfSimulations += 1;
            if (node == tree.root)
            {
                node.numberOfSimulations++;
                node.currentValue += value;
                return;
            }
            while (node != tree.root)
            {
                node.numberOfSimulations++;
                node.currentValue += value;
                node = node.parents[0];
            }
        }

        /// <summary>
        /// Randomly chooses the action to complete the simulation (light playouts)
        /// </summary>
        /// <remarks>This function is not implemented nor used.</remarks>
        /*
        private Action RollOutPolicy()
        {
            return new Action();
        }
        */
    }
}
