using System;
using System.Collections.Generic;

namespace BlackClover
{
    public class Action
    {
        int X;
        int Y;
        char Color;
        public Action(int x, int y, char c) { X = x; Y = y; Color = c; }
        public void SetX(int x) { if (x >= 0 && x <= 18) X = x; }
        public int GetX() { return X; }
        public void SetY(int x) { if (x >= 0 && x <= 18) Y = x; }
        public int GetY() { return Y; }
        public void SetClr(char x) { if (x == 'B' || x == 'W') Color = x; }
        public char GetClr() { return Color; }

        //this function takes a state and returns all possible actions using the getlibrty function 
        static public List<Action> PossibleActions(State x)
        {
            var possibleActions = new List<Action>();
            int turn = x.GetTurn();
            bool[,] mark;
            char [,] stateBoard = x.GetBoard();
            char clr = turn == 1 ? 'W' : 'B';
            int libterties;

            for(int i = 0; i < 19; i++)
            {
                for(int j = 0; j < 19; j++)
                {
                    if (stateBoard[i,j] == '\0')
                    { 
                        mark = new bool[19,19];
                        x.AddStone(i,j, clr);
                        libterties = x.GetLiberty(i,j, mark, clr, stateBoard);
                        if (libterties != 0)
                        {
                            possibleActions.Add(new Action(i, j, clr));
                        }
                        x.RemoveStone(i,j);
                    }
                }
            }
            //for (int i = 0; i < 19; i++)
            //{

            //    for (int j = 0; j < 19; j++)
            //    {
            //        if (x.GetBoard()[i, j] == '\0')
            //        {
            //            stateBoard = x.GetBoard();
            //            x.AddStone(i, j);
            //            mark = new bool[19, 19];

            //            if (x.GetLiberty(i, j, mark, turnClr, stateBoard) == 0)
            //            {
            //                for (int k = 0; k < 19; k++)
            //                {
            //                    for (int l = 0; l < 19; l++)
            //                    {
            //                        if (x.GetLiberty(k, l, mark, oppClr, stateBoard) == 0)
            //                        {
            //                            capture = 1;
            //                            break;
            //                        }
            //                    }
            //                }
            //                if (capture == 0) x.RemoveStone(i, j);
            //                else
            //                {
            //                    x.RemoveStone(i, j);
            //                    possibleActions.Add(new Action(i, j, Color));
            //                }
            //            }
            //            else
            //            {
            //                x.RemoveStone(i, j);
            //                possibleActions.Add(new Action(i, j, Color));
            //            }
            //        }
            //    }
            //}
            return possibleActions;
        }
    }
}
