using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIClasses
{
    public class ProjectState
    {
        private int Turn;                       //Indicating which Player to play(0 --> Black & 1 --> White). Initially 0.
        private float RemainingTime_0;          //The Remaining Time for Black Player. Initially 900 seconds.
        private float RemainingTime_1;          //The Remaining Time for White Player. Initially 900 seconds.
        private int Prisoners_0;                //No of Prisoners Captured by Black Player.
        private int Prisoners_1;                //No of Prisoners Captured by White Player.
        private float Score_0;                  //Score of Black Player.
        private float Score_1;                  //Score of White Player.
        private int[,] Board;                   //The Board itself. it's 19 * 19. 0 --> Black Stone, 1 --> White Stone, -1 --> Empty Point
        public ProjectState(int turn, int Prisoners_0, int Prisoners_1, float RemainingTime_0, float RemainingTime_1, float Score_1, float Score_0, int[,] board)
        {
            this.Turn = turn;
            this.RemainingTime_0 = RemainingTime_0;
            this.RemainingTime_1 = RemainingTime_1;
            this.Prisoners_0 = Prisoners_0;
            this.Prisoners_1 = Prisoners_1;
            this.Score_0 = Score_0;
            this.Score_1 = Score_1;
            Array.Copy(board, this.Board, 361);
            this.Board = new int[19, 19];
        }

        public void SetBoard(int[,] board)
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
        public int[,] GetBoard()
        {
            return Board;
        }
        public bool AddStone(int x, int y)
        {
            if (Board[x, y] != -1)
            {
                return false;
            }
            Board[x, y] = Turn;
            return true;
        }
        public int GetTurn()
        {
            return Turn;
        }
        public void RemoveStone(int x, int y)
        {
            Board[x, y] = -1;
        }
        public ProjectState(int turn, int[,] board)
        {
            Board = board;
            Turn = turn;
        }
        public int BlackStones()
        {
            int c = 0;
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (Board[i, j] == 0)
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
                    if (Board[i, j] == 1)
                    {
                        c++;
                    }
                }
            }
            return c;
        }

        public void DecrementTime(int Player, float time)
        {
            if (Player == 0)
            {
                RemainingTime_0 -= time;
            }
            if (Player == 1)
            {
                RemainingTime_1 -= time;
            }
        }
        public int GetLiberty(int x, int y, int[,] mark)
        {

            if (mark[x, y] == 1) { return 0; }
            if (Board[x, y] == -1) { mark[x, y] = 1; return 1; }
            if (Board[x, y] != Turn) { mark[x, y] = 1; return 0; }
            mark[x, y] = 1;
            if (x == 0 && y == 0)
            {
                return GetLiberty(x + 1, y, mark) + GetLiberty(x, y + 1, mark);
            }
            if (x == 18 && y == 0)
            {
                return GetLiberty(x - 1, y, mark) + GetLiberty(x, y + 1, mark);
            }
            if (x == 0 && y == 18)
            {
                return GetLiberty(x + 1, y, mark) + GetLiberty(x, y - 1, mark);
            }
            if (x == 18 && y == 18)
            {
                return GetLiberty(x - 1, y, mark) + GetLiberty(x, y - 1, mark);
            }
            if (x == 0)
            {
                return GetLiberty(x + 1, y, mark) + GetLiberty(x, y + 1, mark) + GetLiberty(x, y - 1, mark);
            }
            if (x == 18)
            {
                return GetLiberty(x - 1, y, mark) + GetLiberty(x, y + 1, mark) + GetLiberty(x, y - 1, mark);
            }
            if (y == 0)
            {
                return GetLiberty(x + 1, y, mark) + GetLiberty(x - 1, y, mark) + GetLiberty(x, y + 1, mark);
            }
            if (y == 18)
            {
                return GetLiberty(x + 1, y, mark) + GetLiberty(x - 1, y, mark) + GetLiberty(x, y - 1, mark);
            }
            return GetLiberty(x + 1, y, mark) + GetLiberty(x - 1, y, mark) + GetLiberty(x, y - 1, mark) + GetLiberty(x, y + 1, mark);
        }

        public static ProjectState GetSuccessor(ProjectState state, ProjectAction action)
        {
            int newTurn = (1 + state.Turn) % 2;
            float Score_0, Score_1;
            float RemainingTime_0, RemainingTime_1;
            int Prisoners_0 = 0, Prisoners_1 = 0;
            int[,] board = new int[,];

            int x, y;
            x = action.getX;
            y = action.getY;

            Array.Copy(state.Board, board, 361);

            int[,] mark = new int[19, 19];

            state.Board[x, y] = state.turn;
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (state.Board[i, j] == state.turn)
                    {
                        int liberties = ProjectState.GetLiberty(i, j, mark);
                        if (liberties == 0)
                        {
                            board[i, j] = -1;
                            if (state.Turn == 1)
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
            return new ProjectState(newTurn, Prisoners_0, Prisoners_1, 0,0,0,0, board);
        }
    }
}
