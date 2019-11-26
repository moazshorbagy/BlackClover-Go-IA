using System;
using System.Collections.Generic;
using System.Text;

namespace MI
{
    public class Score
    {
        int scoreW = 0;
        int scoreB = 0;
        int BlackStones = 0;
        int WhiteStones = 0;
        int dir = 0;
        int[] ScoresArr = new int[2];
        bool[,] Visited = new bool[19, 19];
        int territoryCountB = 0;
        int territoryCountW = 0;
        public int[] getScore(int prisonersB, int prisonersW, char[,] state)
        { //prisonersB=how many black stones collected
          //prisonersW=how many white stones collected 
            scoreW += prisonersB; //score of white stone + black stones prisoners
            scoreB += prisonersW;
            for (int row = 0; row < state.GetLength(0); row++)
            {
                for (int col = 0; col < state.GetLength(1); col++)
                {
                    if (state[row, col] == 'B')
                    {
                        BlackStones++;
                    }
                    if (state[row, col] == 'W')
                    {
                        WhiteStones++;
                    }
                    char outer = ' ';
                    if (state[row, col] == ' ' && Visited[row, col] != true)
                    {

                        int posx = row;
                        int posy = col;
                        int initposy = posy;
                        int initposx = posx;


                        if (state[(posx + 1), posy] != ' ')
                        {
                            outer = state[(posx + 1), posy];

                            if (outer == 'W')
                                territoryCountW++;
                            if (outer == 'B')
                                territoryCountB++;
                            Visited[posx, posy] = true;
                            dir = 0;
                        }
                        else
                        {
                            posx = posx + 1;
                            initposx = posx;

                            dir = (dir + 3) % 4;
                        }

                        int counter = 0;
                        while (posx != row || posy != col || counter < 2 || dir != 0)
                        {
                            if (initposx == posx && initposy == posy)
                            {
                                counter++;
                            }

                            if (dir == 0)
                            {

                                if (state[posx, posy + 1] != ' ')
                                {
                                    if (state[posx, posy + 1] != outer)
                                    {
                                        territoryCountB = 0;
                                        territoryCountW = 0;
                                        break;
                                    }
                                    else
                                    {
                                        dir = 1;
                                    }
                                }
                                else
                                {
                                    posy = posy + 1;
                                    dir = (dir + 3) % 4;

                                    if (outer == 'W' && Visited[posx, posy] != true)
                                        territoryCountW++;
                                    if (outer == 'B' && Visited[posx, posy] != true)
                                        territoryCountB++;
                                    Visited[posx, posy] = true;
                                }
                            }
                            else if (dir == 1)
                            {

                                if (state[posx - 1, posy] != ' ')
                                {
                                    if (state[posx - 1, posy] != outer)
                                    {
                                        territoryCountB = 0;
                                        territoryCountW = 0;
                                        break;
                                    }
                                    else
                                        dir = 2;
                                }
                                else
                                {
                                    posx = posx - 1;
                                    dir = (dir + 3) % 4;

                                    if (outer == 'W' && Visited[posx, posy] != true)
                                        territoryCountW++;
                                    if (outer == 'B' && Visited[posx, posy] != true)
                                        territoryCountB++;
                                    Visited[posx, posy] = true;
                                }
                            }
                            else if (dir == 2)
                            {

                                if (state[posx, posy - 1] != ' ')
                                {
                                    if (state[posx, posy - 1] != outer)
                                    {
                                        territoryCountB = 0;
                                        territoryCountW = 0;
                                        break;
                                    }
                                    else
                                        dir = 3;
                                }
                                else
                                {
                                    posy = posy - 1;
                                    dir = (dir + 3) % 4;

                                    if (outer == 'W' && Visited[posx, posy] != true)
                                        territoryCountW++;
                                    if (outer == 'B' && Visited[posx, posy] != true)
                                        territoryCountB++;
                                    Visited[posx, posy] = true;
                                }
                            }
                            else if (dir == 3)
                            {

                                if (state[posx + 1, posy] != ' ')
                                {
                                    if (state[posx + 1, posy] != outer)
                                    {
                                        territoryCountB = 0;
                                        territoryCountW = 0;
                                        break;
                                    }
                                    else
                                        dir = 0;
                                }
                                else
                                {
                                    posx = posx + 1;
                                    dir = (dir + 3) % 4;
                                    if (outer == 'W' && Visited[posx, posy] != true)
                                        territoryCountW++;
                                    if (outer == 'B' && Visited[posx, posy] != true)
                                        territoryCountB++;
                                    Visited[posx, posy] = true;
                                }
                            }
                        }
                        scoreW += territoryCountW;
                        scoreB += territoryCountB;
                        territoryCountB = 0;
                        territoryCountW = 0;
                    }
                }
            }
            scoreW += WhiteStones;
            scoreB += BlackStones;

            ScoresArr[0] = scoreW;
            ScoresArr[1] = scoreB;
            return ScoresArr;
        }
    }
}


