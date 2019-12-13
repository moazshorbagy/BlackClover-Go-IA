﻿using System.Collections.Generic;
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
            Action action = search.Play();
            List<GUIAction> guiActions;
            lock(this.sharedState)
            {
                (guiActions, sharedState[0]) = state.GetSuccessor(action);
             
            }
            foreach(GUIAction guiAction in guiActions)
            {
                if (guiAction.isAddition)
                {
                    lock(this.sharedSpawnStones)
                    {
                        sharedSpawnStones.Add((this.turn, guiAction.position));
                        Debug.Log("Added stone at (" + guiAction.position.x + ", " + guiAction.position.y + ")");
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