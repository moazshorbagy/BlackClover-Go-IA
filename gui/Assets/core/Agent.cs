using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlackClover
{
    public class BlackCloverAgent
    {
        private List<(int, Vector2)> sharedSpawnStones;
        private List<Vector2> sharedRemoveStones; 
        private readonly int turn;
        private List<State> sharedState;
        public BlackCloverAgent(List<(int, Vector2)> sharedSpawnStones, List<Vector2> sharedRemoveStones, List<State> sharedState, int turn)
        {
            this.sharedRemoveStones = sharedRemoveStones;
            this.sharedSpawnStones = sharedSpawnStones;
            this.sharedState = sharedState;
            this.turn = turn;
        }

        public void GetNextMove(State state)
        {
            MCTS search = new MCTS(state);
            Debug.Log("is everything okay?");
            Action action = search.Play();
            List<GUIAction> guiActions = State.GetSuccessor(state, action, true);
            lock(this.sharedState)
            {
                sharedState[0] = state.GetSuccessor(action);
                Debug.Log("changed shared state");
            }
            foreach(GUIAction guiAction in guiActions)
            {
                if (guiAction.isAddition)
                {
                    lock(this.sharedSpawnStones)
                    {
                        sharedSpawnStones.Add((this.turn, guiAction.position));
                        Debug.Log("Added stone");
                    }
                }
                else
                {
                    lock(this.sharedRemoveStones)
                    {
                        sharedRemoveStones.Add(guiAction.position);
                        Debug.Log("Removed stone");
                    }
                }
            }
            
        }
    }
}
