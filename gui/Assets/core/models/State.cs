using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlackClover
{
    public class State
    {
        private int Turn;                       //Indicating which Player to play(0 --> Black & 1 --> White). Initially 0.
        public int Prisoners_0;                //No of Prisoners Captured by Black Player.
        public int Prisoners_1;                //No of Prisoners Captured by White Player.
        private char[,] Board;                  //The Board itself. it's 19 * 19. 0 --> Black Stone, 1 --> White Stone, -1 --> Empty Point
        private int consecutivePasses;

        public State(int turn, char[,] board)
        {
            Board = board;
            Turn = turn;
            consecutivePasses = 0;
            Prisoners_0 = 0;
            Prisoners_1 = 0;
        }

        public State(int turn, int Prisoners_0, int Prisoners_1, int consecutivePasses, char[,] board)
        {
            Turn = turn;
            this.Prisoners_0 = Prisoners_0;
            this.Prisoners_1 = Prisoners_1;
            this.consecutivePasses = consecutivePasses;
            Board = new char[19, 19];
            Array.Copy(board, Board, 361);
        }

        public void SetBoard(char[,] board)
        {
            Board = board;
        }

        public void SetTurn(int turn)
        {
            if (turn == 0 || turn == 1)
            {
                Turn = turn;
            }
        }
        public char[,] GetBoard()
        {
            return Board;
        }
        public bool AddStone(int x, int y)
        {
            if (Board[x, y] != '\0')
            {
                return false;
            }
            Board[x, y] = Turn == 1 ? 'W' : 'B';
            return true;
        }

        public int GetPrisonersW()
        {
            return Prisoners_1;
        }
        public int GetPrisonersB()
        {
            return Prisoners_1;
        }
        public int GetTurn()
        {
            return Turn;
        }
        public void RemoveStone(int x, int y)
        {
            Board[x, y] = '\0';
        }

        public int BlackStones()
        {
            int c = 0;
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (Board[i, j] == 'B')
                    {
                        c++;
                    }
                }
            }
            return c;
        }
        public int WhiteStones()
        {
            int c = 0;
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (Board[i, j] == 'W')
                    {
                        c++;
                    }
                }
            }
            return c;
        }

        public int GetLiberty(int x, int y, bool[,] mark, char clr, char[,] board)
        {

            if (mark[x, y] == true) { return 0; }
            if (board[x, y] == '\0') { mark[x, y] =true; return 1; }
            if (board[x, y] != clr) { mark[x, y] = true; return 0; }
            mark[x, y] = true;
            if (x == 0 && y == 0)
            {
                return GetLiberty(x + 1, y, mark, clr, board) + GetLiberty(x, y + 1, mark, clr, board);
            }
            if (x == 18 && y == 0)
            {
                return GetLiberty(x - 1, y, mark, clr, board) + GetLiberty(x, y + 1, mark, clr, board);
            }
            if (x == 0 && y == 18)
            {
                return GetLiberty(x + 1, y, mark, clr, board) + GetLiberty(x, y - 1, mark, clr, board);
            }
            if (x == 18 && y == 18)
            {
                return GetLiberty(x - 1, y, mark, clr, board) + GetLiberty(x, y - 1, mark, clr, board);
            }
            if (x == 0)
            {
                return GetLiberty(x + 1, y, mark, clr, board) + GetLiberty(x, y + 1, mark, clr, board) + GetLiberty(x, y - 1, mark, clr, board);
            }
            if (x == 18)
            {
                return GetLiberty(x - 1, y, mark, clr, board) + GetLiberty(x, y + 1, mark, clr, board) + GetLiberty(x, y - 1, mark, clr, board);
            }
            if (y == 0)
            {
                return GetLiberty(x + 1, y, mark, clr, board) + GetLiberty(x - 1, y, mark, clr, board) + GetLiberty(x, y + 1, mark, clr, board);
            }
            if (y == 18)
            {
                return GetLiberty(x + 1, y, mark, clr, board) + GetLiberty(x - 1, y, mark, clr, board) + GetLiberty(x, y - 1, mark, clr, board);
            }
            return GetLiberty(x + 1, y, mark, clr, board) + GetLiberty(x - 1, y, mark, clr, board) + GetLiberty(x, y - 1, mark, clr, board) + GetLiberty(x, y + 1, mark, clr, board);
        }

        public (List<GUIAction>, State) GetSuccessor(Action action)
        {
            int newTurn = (1 + Turn) % 2;
            int Prisoners_0 = 0, Prisoners_1 = 0;
            char[,] board = new char[19,19];

            int x, y;
            x = action.getX();
            y = action.getY();

            Array.Copy(Board, board, 361);
            List<GUIAction> guiActions = new List<GUIAction>();
            guiActions.Add(new GUIAction(x, y, true, Turn));
            board[x, y] = MapPiece(Turn);
            Board[x, y] = MapPiece(Turn);
            char clr = newTurn == 1 ? 'W' : 'B';
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (Board[i, j] == clr)
                    {
                        bool[,] mark = new bool[19, 19];
                        int liberties = GetLiberty(i, j, mark, clr, Board);
                        if (liberties == 0)
                        {
                            guiActions.Add(new GUIAction(i, j, false, newTurn));
                            board[i, j] = '\0';
                            if (Turn == 1)
                            {
                                Prisoners_1 += 1;
                            }
                            else
                            {
                                Prisoners_0 += 1;
                            }
                        }
                    }
                }
            }

            int passes = 0;
            if(action.getY() == -1 && action.getX() == -1)
            {
                passes = consecutivePasses + 1;
            }

            return (guiActions, new State(newTurn, Prisoners_0, Prisoners_1, passes, board));
        }

        public bool IsTerminal()
        {
            if(consecutivePasses > 1 || Action.PossibleActions(this).Count == 0)
            {
                return true;
            }
            return false;
        }

        public char GetWinner()
        {
            return 'B';
        }

        public static char MapPiece(int piece)
        {
            return piece == 1 ? 'W' : (piece == 0 ? 'B' : '\0');
        }
    }
}
