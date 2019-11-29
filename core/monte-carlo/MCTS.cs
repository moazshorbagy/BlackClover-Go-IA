using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackClover_Go_IA.monte_carlo
{
    class MCTS
    {
        private Problem problem;
        private DAG tree;
        private int limit;
        private Random random;

        /// <summary>
        /// <para>Class representing the Monte Carlo simulation.</para>
        /// <para>Handles best-move selection.</para>
        /// </summary>
        /// <param name="problem">The problem definition.</param>
        public MCTS(Problem problem)
        {
            this.problem = problem;
            tree = new DAG(this.problem.initialState);
            limit = 1600;
            random = new Random();
        }

        /// <summary>
        ///  Handles the four MCTS steps: selection, expansion, simulation, backpropagation.
        /// </summary>
        /// <returns>The best action to be taken</returns>
        public Action Play()
        {
            int time = 0; // keeps track of number of simulations

            // The first selection is the actual current state of the game
            Node currentNode = tree.root;

            while(time < limit)
            {
                if(currentNode.hasChildNodes)
                {
                    // Selection
                    currentNode = Select(currentNode);
                } else
                {
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

                    // Selection
                    currentNode = tree.root;
                    time++;
                }
            }

            tree.root = tree.root.children[0]; // Modify the zero to be the best play after the simulation
            tree.root.parents.Clear();
            return new Action(); // Modify this to be the best action after the simulation
        }

        /// <summary>
        /// Implements the Tree Policy.
        /// Chooses the node (coming from the action) that maximizes Q + U.
        /// </summary>
        /// <remarks>It is responsible for balancing the trade-off between exploration and exploitation.</remarks>
        private Node Select(Node node)
        {
            return node.children[random.Next()];
        }

        /// <summary>
        /// Adds nodes to the tree
        /// </summary>
        private void Expand(Node node)
        {
            List<State> successors = problem.GetSuccessors(node.state);
            for (int i = 0; i < successors.Count; i++)
            {
                node.AddChild(new Node(successors[i]));
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
            while (problem.IsGoal(state))
            {
                state = problem.GetSuccessor(state, problem.GetActions(state)[random.Next()]);
            }
            return problem.GetWinner(state);
        }

        /// <summary>
        /// The result of the simulation is propagated back through the tree to the
        /// root node, updating the value and visit count of each node along the way.
        /// </summary>
        private void BackPropagate(Node node, int value)
        {
            tree.totalNumberOfSimulations += 1;
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
        private Action RollOutPolicy()
        {
            return new Action();
        }
    }
}
