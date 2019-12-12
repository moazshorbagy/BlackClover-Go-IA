﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlackClover;
using System.Threading;
using System;

namespace Board
{
    

    public class BoardManager : MonoBehaviour
    {
        private const int NumberOfTiles=19;
        private int BlackStoneScore=0;
        private int WhiteStoneScore=0;
        private bool BlackTurn;
        private bool StoneAdded=false;
        private bool AgentvsAgent;
        public Text BlackStoneText;
        public Text WhiteStoneText;

        //private Score score = new Score();
        public List <GameObject> Stones; // a list containg a white stone and a black stone
        Dictionary<Vector2, GameObject> SpawnedStones = new Dictionary<Vector2, GameObject>();// hash map that contatins all stones currently available on the board
        List<(int, Vector2)> sharedSpawnStones;
        List<Vector2> sharedRemoveStones;
        List<State> sharedState;
    /*this Function takes an index which indicates which stone to spawn, index is 0 for black stones and 1 for white stones
      it's second parameter is the position to spawn the stone at position should range from 0 to 18 in x and y directions.
    */
        void SpawnStones(int index,Vector2 position)
        {
       

            if(CheckInBounds(position)==false)
            {
                Debug.Log("This position is out of bounds! " );
                StoneAdded=false;
                 return;
            }
            float XPos=1.2f+(position.x*1.2f);
            float YPos=1.2f+(position.y*1.2f);
            Vector3 MappedPosition = new Vector3(XPos,0.3f,YPos);
        
            if(!SpawnedStones.ContainsKey(position))
            {
            GameObject Stone = Instantiate(Stones[index],MappedPosition,Quaternion.identity)as GameObject;
            Stone.transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
            Stone.transform.SetParent(transform);
                SpawnedStones.Add(position,Stone);
                StoneAdded=true;

            }
            else
            {
                Debug.Log("This position is already taken!" );
                 StoneAdded=false;

            }
        

        }
        /* This function checks if the given position is inside the board bounds or not
          and returns true if the move is inside the bounds and returns false otherwise
        */
        bool CheckInBounds(Vector2 position)
        {
            if (position.x<0 || position.x>18 || position.y<0 ||position.y>18 )
                return false;
            return true;
        }
    /*
    This Function takes a position to delete the stone present at these locations 
    It returns true if the stone at position: position is deleted successfully
    It returns false if there is no stone at the given position, Throwing a KeyNotFoundException
    */
        bool RemoveStones(Vector2 position)
        {
            bool deleted =true;
            try
            {
                Destroy(SpawnedStones[position]);
                SpawnedStones.Remove(position);
            }
            catch (KeyNotFoundException e)
            {
                deleted = false;  
            }
             return deleted;
        }

        // Start is called before the first frame update
        void Start()
        {
            BlackTurn=true;
            AgentvsAgent=false;
            sharedState = new List<State>();
            sharedSpawnStones = new List<(int, Vector2)>();
            sharedRemoveStones = new List<Vector2>();
            sharedState.Add(new State(0, new char[19,19]));
            Thread agentThread = new Thread(new ThreadStart(startDummyGame));
            agentThread.Start();
           
        }

        void StartAgent()
        {
            
            sharedSpawnStones = new List<(int, Vector2)>();
            sharedRemoveStones = new List<Vector2>();
            BlackCloverAgent agent = new BlackCloverAgent(sharedSpawnStones, sharedRemoveStones, sharedState, 1);
            while(true)
            {
                if(sharedState[0].GetTurn() == 1)
                {
                    agent.GetNextMove(sharedState[0]);
                    Debug.Log("agent waiting for his turn..");
                    Score score = new Score();
                    char[,] boardcopy = new char[19, 19];
                    Array.Copy(sharedState[0].GetBoard(), boardcopy, 361);
                    int[] scores = score.getScore(sharedState[0].GetPrisonersB(), sharedState[0].GetPrisonersW(), boardcopy);
                    SetBlackScore(scores[0]);
                    SetWhiteScore(scores[1]);
                }
            }
        }

        void startDummyGame()
        {
            char[,] board = new char[19, 19];
            State state = new State(0, board);
            int sleepDuration = 2000;
            System.Random random = new System.Random();
            List<BlackClover.Action> actions = BlackClover.Action.PossibleActions(state);
            List<GUIAction> a;
            for (int i = 0; i < 250; i++)
            {
                Console.WriteLine(actions.Count);
                (a, state) = state.GetSuccessor(actions[random.Next(actions.Count)]);
                lock(sharedSpawnStones)
                {
                    foreach(GUIAction action in a)
                    {
                        if (action.isAddition)
                        { 
                            sharedSpawnStones.Add((state.GetTurn(), action.position));
                        }
                    }
                }
                lock(sharedRemoveStones)
                {
                    foreach (GUIAction action in a)
                    {
                        if (!action.isAddition)
                        {
                            sharedRemoveStones.Add(action.position);
                        }
                    }
                }
                Console.WriteLine(state.GetTurn());
                actions = BlackClover.Action.PossibleActions(state);
                Console.WriteLine(actions[0].getColor());
                Thread.Sleep(Math.Max(150, sleepDuration));
                sleepDuration -= 100;
            }
        }

        Vector2 GenerateRandomPosition()
        {
           System.Random random = new System.Random();  
           Vector2 Position = new Vector2(random.Next(0, 18),random.Next(0, 18));   
           return Position; 
        }

        /*
        This function gets the user actions and spawn a stone on the board if the user clicks on a valid position on the board.
        It also passes his turn if The keyboard button "P" was pressed
        */


        void GetUserActions()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = 10;
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        

                int xPos = (int)Math.Round((mouseWorldPosition.x/1.2)-1);
                int yPos = (int)Math.Round((mouseWorldPosition.z/1.2)-1);

        
                Vector2 spawningPosition = new Vector2(xPos,yPos);

                SpawnStones(0, spawningPosition);
                BlackClover.Action action = new BlackClover.Action(xPos, 18 - yPos, "BLACK");
                List<GUIAction> guiActions;
                lock (sharedState)
                {
                    (guiActions, sharedState[0]) = sharedState[0].GetSuccessor(action);
                }
                foreach (GUIAction guiAction in guiActions)
                {
                    if(!guiAction.isAddition)
                    {
                        lock (sharedRemoveStones)
                        {
                            sharedRemoveStones.Add(guiAction.position);
                            Debug.Log("Removed stone");
                        }
                    }
                }
                if (StoneAdded)
                {
                    BlackTurn=false;
                    Score score = new Score();
                    char[,] boardcopy = new char[19, 19];
                    Array.Copy(sharedState[0].GetBoard(), boardcopy, 361);
                    int[] scores = score.getScore(sharedState[0].GetPrisonersB(), sharedState[0].GetPrisonersW(), boardcopy);
                    SetBlackScore(scores[0]);
                    SetWhiteScore(scores[1]);
                }
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                 BlackTurn=false; 
                 Debug.Log("Your Turn is passed " );
            }   
        


        }

        void SetBlackScore(int score)
        {
            BlackStoneScore=score;
        }

        void SetWhiteScore(int score)
        {
            WhiteStoneScore=score;
        }
        void UpdateScores()
        {
            BlackStoneText.text= $"Black Score \n {BlackStoneScore}";
            WhiteStoneText.text= $"White Score \n {WhiteStoneScore}";

        }
    
        void Update()
        {
            UpdateScores();
            
            if (AgentvsAgent == false)
            {
                //if(sharedState[0].GetTurn() == 0)
                //{
                //    GetUserActions();
                //}
                lock (sharedSpawnStones)
                {
                    for (int i = sharedSpawnStones.Count - 1; i >= 0; i--)
                    {
                        Vector2 position = sharedSpawnStones[i].Item2;
                        position[1] = 18 - position[1];
                        SpawnStones(sharedSpawnStones[i].Item1, position);
                        sharedSpawnStones.RemoveAt(i);
                        Debug.Log("Added stone to board");
                    }
                }

                lock (sharedRemoveStones)
                {
                    for (int i = sharedRemoveStones.Count - 1; i >= 0; i--)
                    {
                        Vector2 position = sharedRemoveStones[i];
                        position[1] = 18 - position[1];
                        RemoveStones(position);
                        sharedRemoveStones.RemoveAt(i);
                        Debug.Log("removed stone from board");
                    }
                }
            }
            else
            {
                  SpawnStones(0,GenerateRandomPosition());
                  SpawnStones(1,GenerateRandomPosition());
            }
    
        }
    }
}