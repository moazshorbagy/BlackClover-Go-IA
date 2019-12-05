using System;
using System.Collections.Generic;
using System.Text;

namespace MI
{
    public class Score
    {
        public static int[] getScore(int prisonersB, int prisonersW, char[,] state)
        {
            int scoreW = 0, scoreB = 0;
            int BlackStones = 0;
            int WhiteStones = 0;
            int dir = 0;
            int[] ScoresArr = new int[2];
            bool[,] Visited = new bool[19, 19];
            int territoryCountB = 0;
            int territoryCountW = 0;
            //prisonersB=how many black stones collected
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
                        bool pointouter = false;
                        int posx = row;
                        int posy = col;
                        int firstpointx = posx;
                        int firstpointy = posy;
                        int secondpointx = 0;
                        int secondpointy = 0;
                        int countp1 = 1;
                        int countp2 = 1;
                        if (state[(posx + 1), posy] != ' ')
                        {
                            pointouter = true;
                            outer = state[(posx + 1), posy];
                            secondpointx = posx + 1;
                            secondpointy = posy;
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
                            secondpointx = posx;
                            secondpointy = firstpointy;
                            dir = (3 + 3) % 4;
                        }
                        int currentx = 20;
                        int currenty = 20;
                        int previousx = 20;
                        int previousy = 20;
                        while ((secondpointx != currentx || secondpointy != currenty || firstpointx != previousx || firstpointy != previousy) && (countp1 <= 2 || countp2 <= 2) && (countp1 != 1 || territoryCountB != 1 || countp2 != 2) && (countp1 != 1 || territoryCountW != 1 || countp2 != 2))
                        {
                            previousx = currentx;
                            previousy = currenty;
                            if (dir == 0)
                            {

                                if (state[posx, posy + 1] != ' ')
                                {
                                    if (outer != ' ')
                                    {
                                        if (state[posx, posy + 1] != outer)
                                        {
                                            territoryCountB = 0;
                                            territoryCountW = 0;
                                            break;
                                        }
                                        else
                                        {

                                            if (pointouter == true)
                                            {
                                                if (posx == firstpointx && posy + 1 == firstpointy)
                                                {
                                                    countp1++;
                                                }
                                                else if (posx == secondpointx && posy + 1 == secondpointy)
                                                {
                                                    countp2++;
                                                }
                                            }

                                            dir = 1;
                                        }
                                    }
                                    else
                                    {
                                        outer = state[posx, posy + 1];
                                        if (pointouter == true)
                                        {
                                            if (posx == firstpointx && posy + 1 == firstpointy)
                                            {
                                                countp1++;
                                            }
                                            else if (posx == secondpointx && posy + 1 == secondpointy)
                                            {
                                                countp2++;
                                            }
                                        }
                                        dir = 1;
                                    }
                                }
                                else
                                {
                                    if (pointouter == true)
                                    {
                                        if (posx == firstpointx && posy + 1 == firstpointy)
                                        {
                                            countp1++;
                                        }
                                        else if (posx == secondpointx && posy + 1 == secondpointy)
                                        {
                                            countp2++;
                                        }
                                    }
                                    currentx = posx;
                                    currenty = posy + 1;
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
                                    if (outer != ' ')
                                    {
                                        if (state[posx - 1, posy] != outer)
                                        {
                                            territoryCountB = 0;
                                            territoryCountW = 0;
                                            break;
                                        }
                                        else
                                        {
                                            if (pointouter == true)
                                            {
                                                if (posx - 1 == firstpointx && posy == firstpointy)
                                                {
                                                    countp1++;
                                                }
                                                else if (posx - 1 == secondpointx && posy == secondpointy)
                                                {
                                                    countp2++;
                                                }
                                            }
                                            //currentx = posx - 1;
                                            //currenty = posy;
                                            dir = 2;
                                        }
                                    }
                                    else
                                    {
                                        if (pointouter == true)
                                        {
                                            if (posx - 1 == firstpointx && posy == firstpointy)
                                            {
                                                countp1++;
                                            }
                                            else if (posx - 1 == secondpointx && posy == secondpointy)
                                            {
                                                countp2++;
                                            }
                                        }
                                        outer = state[posx - 1, posy];
                                        //currentx = posx - 1;
                                        //currenty = posy;
                                        dir = 2;
                                    }
                                }
                                else
                                {
                                    if (pointouter == true)
                                    {
                                        if (posx - 1 == firstpointx && posy == firstpointy)
                                        {
                                            countp1++;
                                        }
                                        else if (posx - 1 == secondpointx && posy == secondpointy)
                                        {
                                            countp2++;
                                        }
                                    }
                                    currentx = posx - 1;
                                    currenty = posy;
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
                                    if (outer != ' ')
                                    {
                                        if (state[posx, posy - 1] != outer)
                                        {
                                            territoryCountB = 0;
                                            territoryCountW = 0;
                                            break;
                                        }
                                        else
                                        {
                                            if (pointouter == true)
                                            {
                                                if (posx == firstpointx && posy - 1 == firstpointy)
                                                {
                                                    countp1++;
                                                }
                                                else if (posx == secondpointx && posy - 1 == secondpointy)
                                                {
                                                    countp2++;
                                                }
                                            }
                                            //currentx = posx;
                                            //currenty = posy - 1;
                                            dir = 3;
                                        }
                                    }
                                    else
                                    {
                                        if (pointouter == true)
                                        {
                                            if (posx == firstpointx && posy - 1 == firstpointy)
                                            {
                                                countp1++;
                                            }
                                            else if (posx == secondpointx && posy - 1 == secondpointy)
                                            {
                                                countp2++;
                                            }
                                        }
                                        outer = state[posx, posy - 1];
                                        //currentx = posx;
                                        //currenty = posy - 1;
                                        dir = 3;
                                    }
                                }
                                else
                                {
                                    if (pointouter == true)
                                    {
                                        if (posx == firstpointx && posy - 1 == firstpointy)
                                        {
                                            countp1++;
                                        }
                                        else if (posx == secondpointx && posy - 1 == secondpointy)
                                        {
                                            countp2++;
                                        }
                                    }
                                    currentx = posx;
                                    currenty = posy - 1;
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
                                    if (outer != ' ')
                                    {
                                        if (state[posx + 1, posy] != outer)
                                        {
                                            territoryCountB = 0;
                                            territoryCountW = 0;
                                            break;
                                        }
                                        else
                                        {
                                            if (pointouter == true)
                                            {
                                                if (posx + 1 == firstpointx && posy == firstpointy)
                                                {
                                                    countp1++;
                                                }
                                                else if (posx + 1 == secondpointx && posy == secondpointy)
                                                {
                                                    countp2++;
                                                }
                                            }
                                            //currentx = posx + 1;
                                            //currenty = posy;
                                            dir = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (pointouter == true)
                                        {
                                            if (posx + 1 == firstpointx && posy == firstpointy)
                                            {
                                                countp1++;
                                            }
                                            else if (posx + 1 == secondpointx && posy == secondpointy)
                                            {
                                                countp2++;
                                            }
                                        }
                                        outer = state[posx + 1, posy];
                                        //currentx = posx + 1;
                                        //currenty = posy;
                                        dir = 0;
                                    }
                                }
                                else
                                {
                                    if (pointouter == true)
                                    {
                                        if (posx + 1 == firstpointx && posy == firstpointy)
                                        {
                                            countp1++;
                                        }
                                        else if (posx + 1 == secondpointx && posy == secondpointy)
                                        {
                                            countp2++;
                                        }
                                    }
                                    currentx = posx + 1;
                                    currenty = posy;
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


