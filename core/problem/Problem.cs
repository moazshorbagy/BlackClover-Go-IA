using BlackClover_Go_IA.monte_carlo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackClover_Go_IA
{
    class Problem
    {
        public State initialState;

        public Problem(State initialState)
        {
            this.initialState = initialState;
        }

        public Problem()
        {
            this.initialState = new State(0);
        }

        public List<Action> GetActions(State state)
        {
            return new List<Action>();
        }

        public State GetSuccessor(State state, Action action)
        {
            return new State(0);
        }

        public List<State> GetSuccessors(State state)
        {
            return new List<State>();
        }

        public bool IsGoal(State state)
        {
            return false;
        }

        /// <summary>
        /// 0: Draw. 1: I win. -1: Opponnent wins.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public int GetWinner(State state)
        {
            return 0;
        }
    }
}
