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
        private List<Action> myAction;
        private MCTS search;
        public BlackCloverAgent(List<(int, Vector2)> sharedSpawnStones, List<Vector2> sharedRemoveStones, List<State> sharedState, int turn, List<Action> myAction)
        {
            this.sharedRemoveStones = sharedRemoveStones;
            this.sharedSpawnStones = sharedSpawnStones;
            this.sharedState = sharedState;
            this.myAction = myAction;
            this.turn = turn;
        }

        public void GetNextMove()
        {
            search = new MCTS(sharedState[0]);
            //search.OponentPlay(sharedState[0]);
            Action action = search.Play();
            lock(myAction)
            {
                myAction.Add(action);
            }
            List<GUIAction> guiActions;
            lock(this.sharedState)
            {
                (guiActions, sharedState[0]) = sharedState[0].GetSuccessor(action);
             
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
