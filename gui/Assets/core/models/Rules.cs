using System;
using System.Collections.Generic;

namespace BlackClover
{
    public class Action
    {
        int X;
        int Y;
        string Color;
        public Action(int x, int y, string c) { X = x; Y = y; Color = c; }
        public void setX(int x) { if (x >= 0 && x <= 18) X = x; else { Console.WriteLine("Invalid Coordinate."); } }
        public int getX() { return X; }
        public void setY(int x) { if (x >= 0 && x <= 18) Y = x; else { Console.WriteLine("Invalid Coordinate."); } }
        public int getY() { return Y; }
        public void setColor(string x) { if (x == "BLACK" || x == "WHITE") Color = x; else { Console.WriteLine("Invalid Color."); } }
        public string getColor() { return Color; }

        //this function takes a state and returns all possible actions using the getlibrty function 
        static public List<Action> PossibleActions(State x)
        {
            var PossibleActions = new List<Action>();
            string Color;
            int OppTurn;
            int capture = 0;
            if (x.GetTurn() == 1) { OppTurn = 0; Color = "WHITE"; } else { OppTurn = 1; Color = "BLACK"; }
            int[,] Board2 = new int[19, 19];
            char turnClr = x.GetTurn() == 1 ? 'W' : 'B';
            char oppClr = OppTurn == 1 ? 'W' : 'B';
            for (int i = 0; i < 19; i++)
            {

                for (int j = 0; j < 19; j++)
                {
                    if (x.GetBoard()[i, j] == '\0')
                    {
                        x.AddStone(i, j);
                        Board2 = new int[19, 19];

                        if (GetLiberty(i, j, turnClr, x.GetBoard(), Board2) == 0)
                        {
                            for (int k = 0; k < 19; k++)
                            {
                                for (int l = 0; l < 19; l++)
                                {
                                    if (GetLiberty(k, l, oppClr, x.GetBoard(), Board2) == 0)
                                    {
                                        capture = 1;
                                        break;
                                    }
                                }
                            }
                            if (capture == 0) x.RemoveStone(i, j);
                            else
                            {
                                x.RemoveStone(i, j);
                                PossibleActions.Add(new Action(i, j, Color));
                            }
                        }
                        else
                        {
                            x.RemoveStone(i, j);
                            PossibleActions.Add(new Action(i, j, Color));
                        }
                    }
                }
            }
            return PossibleActions;
        }

        //This function calcutes the Liberty of any stone
        //It takes the board and turn and the position of the stone as inputs and mark is to track visited positions in the board
        static public int GetLiberty(int x, int y, char clr, char[,] Board, int[,] mark)
        {

            if (mark[x, y] == 1) { return 0; }
            if (Board[x, y] == '\0') { mark[x, y] = 1; return 1; }
            if (Board[x, y] != clr) { mark[x, y] = 1; return 0; }
            mark[x, y] = 1;
            if (x == 0 && y == 0)
            {
                return GetLiberty(x + 1, y, clr, Board, mark) + GetLiberty(x, y + 1, clr, Board, mark);
            }
            if (x == 18 && y == 0)
            {
                return GetLiberty(x - 1, y, clr, Board, mark) + GetLiberty(x, y + 1, clr, Board, mark);
            }
            if (x == 0 && y == 18)
            {
                return GetLiberty(x + 1, y, clr, Board, mark) + GetLiberty(x, y - 1, clr, Board, mark);
            }
            if (x == 18 && y == 18)
            {
                return GetLiberty(x - 1, y, clr, Board, mark) + GetLiberty(x, y - 1, clr, Board, mark);
            }
            if (x == 0)
            {
                return GetLiberty(x + 1, y, clr, Board, mark) + GetLiberty(x, y + 1, clr, Board, mark) + GetLiberty(x, y - 1, clr, Board, mark);
            }
            if (x == 18)
            {
                return GetLiberty(x - 1, y, clr, Board, mark) + GetLiberty(x, y + 1, clr, Board, mark) + GetLiberty(x, y - 1, clr, Board, mark);
            }
            if (y == 0)
            {
                return GetLiberty(x + 1, y, clr, Board, mark) + GetLiberty(x - 1, y, clr, Board, mark) + GetLiberty(x, y + 1, clr, Board, mark);
            }
            if (y == 18)
            {
                return GetLiberty(x + 1, y, clr, Board, mark) + GetLiberty(x - 1, y, clr, Board, mark) + GetLiberty(x, y - 1, clr, Board, mark);
            }
            return GetLiberty(x + 1, y, clr, Board, mark) + GetLiberty(x - 1, y, clr, Board, mark) + GetLiberty(x, y - 1, clr, Board, mark) + GetLiberty(x, y + 1, clr, Board, mark);
        }
    }
}