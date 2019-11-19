using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIClasses {
    class ProjectState {
        private int Turn;                       //Indicating which Player to play(0 --> Black & 1 --> White). Initially 0.
        private float RemainingTime_0;          //The Remaining Time for Black Player. Initially 900 seconds.
        private float RemainingTime_1;          //The Remaining Time for White Player. Initially 900 seconds.
        private int Prisoners_0;                //No of Prisoners Captured by Black Player.
        private int Prisoners_1;                //No of Prisoners Captured by White Player.
        private float Score_0;                  //Score of Black Player.
        private float Score_1;                  //Score of White Player.
        private int[,] Board;                   //The Board itself. it's 19 * 19. 0 --> Black Stone, 1 --> White Stone, -1 --> Empty Point
        public ProjectState() {
            Turn = 0;
            RemainingTime_0 = 900;
            RemainingTime_1 = 900;
            Prisoners_0 = 0;
            Prisoners_1 = 0;
            Score_0 = 0;
            Score_1 = 0;
            Board = new int[19, 19];
            for (int i = 0; i < 19; i++) {
                for (int j = 0; j < 19; j++) {
                    Board[i, j] = -1;
                }
            }
        }
        public int[,] GetBoard() {
            return Board;
        }
        public bool AddStone(int x, int y) {
            if(Board[x,y] != -1) {
                return false;
            }
            Board[x, y] = Turn;
            return true;
        }
        public int GetTurn() {
            return Turn;
        }
        public void RemoveStone(int x, int y) {
            Board[x, y] = -1;
        }
        public ProjectState(int turn, int[,] board){
            Board = board;
            Turn = turn;
        }
        public int BlackStones() {
            int c = 0;
            for (int i = 0; i < 19; i++) {
                for (int j = 0; j < 19; j++) {
                    if (Board[i, j] == 0) {
                        c++;
                    }
                }
            }
            return c;
        }
        public int WhiteStones() {
            int c = 0;
            for (int i = 0; i < 19; i++) {
                for (int j = 0; j < 19; j++) {
                    if (Board[i, j] == 1) {
                        c++;
                    }
                }
            }
            return c;
        }

        public void DecrementTime(int Player, float time) { 
            if(Player == 0) {
                RemainingTime_0 -= time;
            }
            if (Player == 1) {
                RemainingTime_1 -= time;
            }
        }
        public int GetLiberty(int x, int y, int[,] mark) {

            if (mark[x, y] == 1) { return 0; }
            if (Board[x, y] == -1) { mark[x, y] = 1; return 1; }
            if (Board[x, y] != Turn) { mark[x, y] = 1; return 0; }
            mark[x, y] = 1;
            if (x == 0 && y == 0) {
                return GetLiberty(x + 1, y, mark) + GetLiberty(x, y + 1, mark);
            }
            if (x == 18 && y == 0) {
                return GetLiberty(x - 1, y, mark) + GetLiberty(x, y + 1, mark);
            }
            if (x == 0 && y == 18) {
                return GetLiberty(x + 1, y, mark) + GetLiberty(x, y - 1, mark);
            }
            if (x == 18 && y == 18) {
                return GetLiberty(x - 1, y, mark) + GetLiberty(x, y - 1, mark);
            }
            if (x == 0) {
                return GetLiberty(x + 1, y, mark) + GetLiberty(x, y + 1, mark) + GetLiberty(x, y - 1, mark);
            }
            if (x == 18) {
                return GetLiberty(x - 1, y, mark) + GetLiberty(x, y + 1, mark) + GetLiberty(x, y - 1, mark);
            }
            if (y == 0) {
                return GetLiberty(x + 1, y, mark) + GetLiberty(x - 1, y, mark) + GetLiberty(x, y + 1, mark);
            }
            if (y == 18) {
                return GetLiberty(x + 1, y, mark) + GetLiberty(x - 1, y, mark) + GetLiberty(x, y - 1, mark);
            }
            return GetLiberty(x + 1, y, mark) + GetLiberty(x - 1, y, mark) + GetLiberty(x, y - 1, mark) + GetLiberty(x, y + 1, mark);
        }
    }
}
