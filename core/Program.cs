using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackClover
{
    class Program
    {
        static void Main(string[] args)
        {
            char[,] board = new char[19, 19];
            
            State state = new State(0, board);
            Debug.Assert(Action.PossibleActions(state).Count == 361, "First state must have 361 successor.");
            Print(state);

            MCTS search = new MCTS(state);

            Stopwatch stopwatch = Stopwatch.StartNew();
            Action a = search.Play();
            stopwatch.Stop();
            Console.WriteLine("Time taken = {0}ms", stopwatch.ElapsedMilliseconds);
            
            Console.WriteLine("Decision taken is ({0}, {1})", a.getX(), a.getY());

            state = state.GetSuccessor(a); // Getting the new state after making the decision
        }

        static void Print(State state)
        {
            Console.WriteLine("State:");
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    char x = state.GetBoard()[i, j] != '\0' ? state.GetBoard()[i, j] : '.';
                    Console.Write("{0} ", x);
                }
                Console.WriteLine();
            }
        }
    }
}
