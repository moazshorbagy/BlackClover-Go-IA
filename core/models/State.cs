using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MI;
using testProject.core.models;

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
        private char[,] Board;                  //The Board itself. it's 19 * 19. 0 --> Black Stone, 1 --> White Stone, -1 --> Empty Point
        private int wConsecutivePasses;
        private int bConsecutivePasses;
        public ProjectState(int turn, int Prisoners_0, int Prisoners_1, float RemainingTime_0, float RemainingTime_1, float Score_1, float Score_0, char[,] board)
        {
            this.Turn = turn;
            this.RemainingTime_0 = RemainingTime_0;
            this.RemainingTime_1 = RemainingTime_1;
            this.Prisoners_0 = Prisoners_0;
            this.Prisoners_1 = Prisoners_1;
            this.Score_0 = Score_0;
            this.Score_1 = Score_1;
            this.Board = new char[19, 19];
            Array.Copy(board, this.Board, 361);
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
        public int GetTurn()
        {
            return Turn;
        }
        public void RemoveStone(int x, int y)
        {
            Board[x, y] = '\0';
        }
        public ProjectState(int turn, char[,] board)
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
        public int GetLiberty(int x, int y, int[,] mark, char clr, char[,] board)
        {

            if (mark[x, y] == 1) { return 0; }
            if (board[x, y] == '\0') { mark[x, y] = 1; return 1; }
            if (board[x, y] != clr) { mark[x, y] = 1; return 0; }
            mark[x, y] = 1;
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

        public static ProjectState GetSuccessor(ProjectState state, ProjectAction action)
        {
            int newTurn = (1 + state.Turn) % 2;
            //float Score_0, Score_1;
            //float RemainingTime_0, RemainingTime_1;
            int Prisoners_0 = 0, Prisoners_1 = 0;
            char[,] board = new char[19,19];

            int x, y;
            x = action.getX();
            y = action.getY();

            Array.Copy(state.Board, board, 361);

            int[,] mark = new int[19, 19];

            board[x, y] = MapPiece(state.Turn);
            char clr = newTurn == 1 ? 'W' : 'B';
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (state.Board[i, j] == clr)
                    {
                        int liberties = state.GetLiberty(i, j, mark, clr, board);
                        if (liberties == 0)
                        {
                            board[i, j] = '\0';
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
            int[] scores = new int[2];
            scores = Score.getScore(Prisoners_0, Prisoners_1, board);
            return new ProjectState(newTurn, Prisoners_0, Prisoners_1, 0,0,scores[0],scores[1], board);
        }

        public static List<GUIAction> GetSuccessor(ProjectState state, ProjectAction action, bool getGUIActions)
        {
            int newTurn = (1 + state.Turn) % 2;
            char[,] board = new char[19, 19];
            List<GUIAction> guiActions = new List<GUIAction>();
            int x, y;
            x = action.getX();
            y = action.getY();

            Array.Copy(state.Board, board, 361);

            int[,] mark = new int[19, 19];

            board[x, y] = MapPiece(state.Turn);
            guiActions.Add(new GUIAction(x, y, true, state.Turn));
            char clr = newTurn == 1 ? 'W' : 'B';
            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++)
                {
                    if (state.Board[i, j] == clr)
                    {
                        int liberties = state.GetLiberty(i, j, mark, clr, board);
                        if (liberties == 0)
                        {
                            guiActions.Add(new GUIAction(i, j, false, newTurn));
                            board[i, j] = '\0';
                        }
                    }
                }
            }
            return guiActions;
        }

        public static List<ProjectState> GetSuccessors(ProjectState state)
        {
            List<ProjectAction> possibleActions = ProjectAction.PossibleActions(state);
            List<ProjectState> successors = new List<ProjectState>();
            foreach (ProjectAction action in possibleActions)
            {
                successors.Add(GetSuccessor(state, action));
            }
            return successors;
        }

        public bool IsTerminal()
        {
            return false;
        }

        public static char MapPiece(int piece)
        {
            return piece == 1 ? 'W' : (piece == 0 ? 'B' : '\0');
        }
    }
}
