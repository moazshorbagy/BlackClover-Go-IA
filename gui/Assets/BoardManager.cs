using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BlackClover;
using System.Threading;
using System;

namespace Board
{
    

    public class BoardManager : MonoBehaviour
    {
        private int BlackStoneScore;
        private int WhiteStoneScore;
        private bool StoneAdded;
        private bool AgentvsAgent;
        public Text BlackStoneText;
        public Text WhiteStoneText;

        //private Score score = new Score();
        public List <GameObject> Stones; // a list containg a white stone and a black stone
        Dictionary<Vector2, GameObject> SpawnedStones = new Dictionary<Vector2, GameObject>();// hash map that contatins all stones currently available on the board
        List<(int, Vector2)> sharedSpawnStones;
        List<Vector2> sharedRemoveStones;
        List<State> sharedState;
        List<BlackClover.Action> myAction;
        List<BlackClover.Action> opAction;
        List<bool> isMyTurn;
        List<string> myClr;
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
            catch (KeyNotFoundException)
            {
                deleted = false;  
            }
             return deleted;
        }

        // Start is called before the first frame update
        void Start()
        {
            AgentvsAgent=true;
            sharedState = new List<State>();
            sharedSpawnStones = new List<(int, Vector2)>();
            sharedRemoveStones = new List<Vector2>();
            opAction = new List<BlackClover.Action>();
            isMyTurn = new List<bool>();
            myClr = new List<string>();
            myAction = new List<BlackClover.Action>();
            if(!AgentvsAgent)
            {
                char[,] board = new char[19, 19];
                for (int i = 0; i < 7; i++)
                {
                    board[i, 0] = 'W';
                    board[i, 2] = 'W';
                    SpawnStones(1, new Vector2(i, 18 - 0));
                    SpawnStones(1, new Vector2(i, 18 - 2));
                    board[i, 1] = 'B';
                    SpawnStones(0, new Vector2(i, 18 - 1));

                }
                myClr.Add("W");
                sharedState.Add(new State(1, board));
                if (myClr[0][0] == 'W')
                {
                    isMyTurn.Add(true);
                }
                else
                {
                    isMyTurn.Add(false);
                }
            }
            Thread agentThread = new Thread(new ThreadStart(StartAgent));
            agentThread.Start();
            if (AgentvsAgent)
            {
                Thread clientThread = new Thread(new ThreadStart(StartClient));
                clientThread.Start();
            }
        }

        void StartAgent()
        {
            Debug.Log("Starting agent");
            char[,] Board = new char[19, 19];
            while (myClr.Count == 0)
            {

            }
            char clr = myClr[0][0];
            Debug.Log("My color is: " + clr);
            lock (myClr)
            {
                myClr.RemoveAt(0);
            }
            int turn = clr == 'W' ? 1 : 0;
            Debug.Log("My turn " + turn);
            BlackCloverAgent agent = new BlackCloverAgent(sharedSpawnStones, sharedRemoveStones, sharedState, turn, myAction);
            sharedState.Add(new State(turn, Board));
            while (true)
            {
                if (this.isMyTurn.Count != 0)
                {
                    agent.GetNextMove();
                    Score score = new Score();
                    char[,] boardcopy = new char[19, 19];
                    Array.Copy(sharedState[0].GetBoard(), boardcopy, 361);
                    int[] scores = score.getScore(sharedState[0].GetPrisonersB(), sharedState[0].GetPrisonersW(), boardcopy);
                    SetBlackScore(scores[0]);
                    SetWhiteScore(scores[1]);
                    lock(this.isMyTurn)
                    {
                        this.isMyTurn.RemoveAt(0);
                    }
                }
                else
                {
                    if(opAction.Count != 0)
                    {
                        lock (opAction)
                        {
                            List<GUIAction> guiActions;
                            (guiActions, sharedState[0]) = sharedState[0].GetSuccessor(opAction[0]);
                            Debug.Log("color of Op: " + opAction[0].GetClr());
                            foreach (GUIAction guiAction in guiActions)
                            {
                                if (guiAction.isAddition)
                                {
                                    lock (sharedSpawnStones)
                                    {
                                        turn = opAction[0].GetClr() == 'W' ? 1 : 0;
                                        sharedSpawnStones.Add((turn, guiAction.position));
                                    }
                                }
                                else
                                {
                                    lock (sharedRemoveStones)
                                    {
                                        sharedRemoveStones.Add(guiAction.position);
                                    }
                                }
                            }
                            opAction.RemoveAt(0);
                        }

                        lock(this.isMyTurn)
                        {
                            Debug.Log("now my turn..");
                            this.isMyTurn.Add(true);
                        }
                    }
                }
            }
        }

        void StartClient()
        {
            //TODO: add the color
            Client client = new Client(myClr, opAction, myAction, isMyTurn);
            client.Start();
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
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("Your Turn is passed ");
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = 10;
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        

                int xPos = (int)Math.Round((mouseWorldPosition.x/1.2)-1);
                int yPos = (int)Math.Round((mouseWorldPosition.z/1.2)-1);

            
                Vector2 spawningPosition = new Vector2(xPos,yPos);

                BlackClover.Action action = new BlackClover.Action(xPos, 18 - yPos, 'B');
                
                List<BlackClover.Action> possibleActions = BlackClover.Action.PossibleActions(sharedState[0]);

                if (possibleActions.FindIndex(((x) => { return x.GetX() == action.GetX() && x.GetY() == x.GetY(); })) == -1)
                {
                    return;
                }

                SpawnStones(0, spawningPosition);
                if (StoneAdded)
                {
                    //BlackClover.Action action = new BlackClover.Action(xPos, 18 - yPos, 'B');
                    List<GUIAction> guiActions;
                    lock (sharedState)
                    {
                        (guiActions, sharedState[0]) = sharedState[0].GetSuccessor(action);
                    }

                    foreach (GUIAction guiAction in guiActions)
                    {
                        if (!guiAction.isAddition)
                        {
                            lock (sharedRemoveStones)
                            {
                                sharedRemoveStones.Add(guiAction.position);
                                Debug.Log("Removed stone");
                            }
                        }

                    }
                    Score score = new Score();
                    char[,] boardcopy = new char[19, 19];
                    Array.Copy(sharedState[0].GetBoard(), boardcopy, 361);
                    int[] scores = score.getScore(sharedState[0].GetPrisonersB(), sharedState[0].GetPrisonersW(), boardcopy);
                    SetBlackScore(scores[0]);
                    SetWhiteScore(scores[1]);
                    lock(this.isMyTurn)
                    {
                        this.isMyTurn.Add(true);
                    }
                }
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
                if(!isMyTurn[0])
                {
                    GetUserActions();
                }
            }
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
    }
}