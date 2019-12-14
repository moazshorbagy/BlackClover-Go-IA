using System;
using System.Collections.Generic;
using System.Text;

namespace BlackClover
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
        char[,] mystate;
        int Bterritory = 0;
        int Wterritory = 0;
        int t = 0;
        List<System.Drawing.Point> listB = new List<System.Drawing.Point>();
        List<System.Drawing.Point> listW = new List<System.Drawing.Point>();
        List<System.Drawing.Point> listBBorders = new List<System.Drawing.Point>();
        List<System.Drawing.Point> listWBorders = new List<System.Drawing.Point>();
        public int[] getScore(int prisonersB, int prisonersW, char[,] state)
        { //prisonersB=how many black stones collected
          //prisonersW=how many white stones collected 
            scoreW = 0;
            scoreB = 0;
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

                        List<System.Drawing.Point> temp = new List<System.Drawing.Point>();
                        if (posx + 1 < 19)
                        {
                            if (state[(posx + 1), posy] != ' ')
                            {
                                pointouter = true;
                                outer = state[(posx + 1), posy];
                                System.Drawing.Point point = new System.Drawing.Point((posx + 1), posy);
                                if (!temp.Contains(point))
                                {
                                    temp.Add(point);
                                }
                                secondpointx = posx + 1;
                                secondpointy = posy;
                                if (outer == 'W')
                                    territoryCountW++;
                                if (outer == 'B')
                                    territoryCountB++;
                                Visited[posx, posy] = true;
                                Visited[posx + 1, posy] = true;
                                t = 1;
                                dir = 0;
                            }
                            else
                            {
                                posx = posx + 1;
                                secondpointx = posx;
                                secondpointy = firstpointy;
                                t = 1;
                                dir = (3 + 3) % 4;
                            }
                        }
                        else
                        {
                            pointouter = true;
                            secondpointx = posx + 1;
                            secondpointy = posy;
                            if (outer == 'W')
                                territoryCountW++;
                            if (outer == 'B')
                                territoryCountB++;
                            Visited[posx, posy] = true;
                            dir = 0;
                        }
                        int currentx = 20;
                        int currenty = 20;
                        int previousx = 20;
                        int previousy = 20;


                        while ((secondpointx != currentx || secondpointy != currenty || firstpointx != previousx || firstpointy != previousy) && (countp1 <= 2 || countp2 <= 2) && (countp1 != 1 || territoryCountB != 1 || countp2 != 2) && (countp1 != 1 || territoryCountW != 1 || countp2 != 2) && (countp1 != 1 || territoryCountB != 1 || countp2 != 2) && (countp1 != 1 || t != 1 || countp2 != 2))
                        {
                            previousx = currentx;
                            previousy = currenty;
                            if (dir == 0)
                            {
                                if (posy + 1 < 19)
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
                                                System.Drawing.Point point1 = new System.Drawing.Point(posx, posy + 1);
                                                if (!temp.Contains(point1))
                                                {
                                                    temp.Add(point1);
                                                }
                                                Visited[posx, posy + 1] = true;
                                                dir = 1;
                                            }
                                        }
                                        else
                                        {
                                            System.Drawing.Point point = new System.Drawing.Point(posx, posy + 1);
                                            if (!temp.Contains(point))
                                            {
                                                temp.Add(point);
                                            }
                                            Visited[posx, posy + 1] = true;
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
                            else if (dir == 1)
                            {
                                if (posx - 1 >= 0)
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
                                                System.Drawing.Point point = new System.Drawing.Point(posx - 1, posy);
                                                if (!temp.Contains(point))
                                                {
                                                    temp.Add(point);
                                                }
                                                Visited[posx - 1, posy] = true;
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
                                            System.Drawing.Point point = new System.Drawing.Point(posx - 1, posy);

                                            if (!temp.Contains(point))
                                            {
                                                temp.Add(point);
                                            }
                                            Visited[posx - 1, posy] = true;
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
                                    dir = 2;
                                }
                            }
                            else if (dir == 2)
                            {
                                if (posy - 1 >= 0)
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

                                                System.Drawing.Point point = new System.Drawing.Point(posx, posy - 1);
                                                if (!temp.Contains(point))
                                                {
                                                    temp.Add(point);
                                                }
                                                Visited[posx, posy - 1] = true;
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
                                            System.Drawing.Point point = new System.Drawing.Point(posx, posy - 1);

                                            if (!temp.Contains(point))
                                            {
                                                temp.Add(point);
                                            }
                                            Visited[posx, posy - 1] = true;
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
                                    dir = 3;
                                }
                            }
                            else if (dir == 3)
                            {
                                if (posx + 1 < 19)
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
                                                System.Drawing.Point point = new System.Drawing.Point(posx + 1, posy);

                                                if (!temp.Contains(point))
                                                {
                                                    temp.Add(point);
                                                }
                                                Visited[posx + 1, posy] = true;
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
                                            System.Drawing.Point point = new System.Drawing.Point(posx + 1, posy);

                                            if (!temp.Contains(point))
                                            {
                                                temp.Add(point);
                                            }
                                            Visited[posx + 1, posy] = true;
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
                                    dir = 0;
                                }
                            }
                            if ((secondpointx == currentx && secondpointy == currenty && previousx == firstpointx && previousy == firstpointy && (outer == 'B' || outer == 'W'))
                                || (countp1 == 2 && countp2 == 2) || ((countp1 == 1 && territoryCountW == 1 && countp2 == 2) ||
                                ((countp1 == 1 && territoryCountB == 1 && countp2 == 2)) || (countp1 == 1 && t == 1 && countp2 == 2)))
                            {
                                System.Drawing.Point point = new System.Drawing.Point(firstpointx, firstpointy);
                                System.Drawing.Point check1 = new System.Drawing.Point((temp[0].X) - 1, (temp[0].Y) - 1);
                                System.Drawing.Point check2 = new System.Drawing.Point((temp[0].X) - 1, (temp[0].Y));
                                if (outer == 'B' && temp.Count >= 4 && ((temp[temp.Count - 1].X == check1.X && temp[temp.Count - 1].Y == check1.Y) || (temp[temp.Count - 1].X == check2.X && temp[temp.Count - 1].Y == check2.Y)))
                                {
                                    listB.Add(point);
                                    listBBorders.AddRange(temp);
                                }
                                else if (outer == 'W' && temp.Count >= 4 && ((temp[temp.Count - 1].X == check1.X && temp[temp.Count - 1].Y == check1.Y) || (temp[temp.Count - 1].X == check2.X && temp[temp.Count - 1].Y == check2.Y)))
                                {
                                    listW.Add(point);
                                    listWBorders.AddRange(temp);
                                }
                                else if (firstpointx == 0 || firstpointy == 0 || firstpointx == 18 || firstpointy == 18)
                                {
                                    if (outer == 'B')
                                    {
                                        listB.Add(point);
                                        listBBorders.AddRange(temp);
                                    }
                                    if (outer == 'W')
                                    {
                                        listW.Add(point);
                                        listWBorders.AddRange(temp);
                                    }
                                }
                            }
                        }
                        territoryCountB = 0;
                        territoryCountW = 0;
                    }
                }
            }
            if (WhiteStones != 0 && BlackStones != 0)
            {
                for (int i = 0; i < listBBorders.Count; i++)
                {
                    state[listBBorders[i].X, listBBorders[i].Y] = '1';
                }

                for (int i = 0; i < listWBorders.Count; i++)
                {
                    state[listWBorders[i].X, listWBorders[i].Y] = '2';
                }
                scoreW += WhiteStones;
                scoreB += BlackStones;

                mystate = state;
                for (int i = 0; i < 19; i++)
                {
                    for (int j = 0; j < 19; j++)
                    {
                        Console.Write(mystate[i, j]);
                    }
                    Console.WriteLine();
                }

                for (int i = 0; i < listB.Count; i++)
                {
                    RegionFillingB(listB[i], '0', '1');

                }
                Console.WriteLine("Black territories");
                Console.WriteLine(Bterritory);

                Console.WriteLine(listW.Count);
                for (int i = 0; i < listW.Count; i++)
                {
                    Console.WriteLine(listW[i]);
                    RegionFillingW(listW[i], '0', '2');
                }
                Console.WriteLine("White territories");
                Console.WriteLine(Wterritory);
                scoreW += Wterritory;
                scoreB += Bterritory;
                ScoresArr[0] = scoreB;
                ScoresArr[1] = scoreW;

            }
            else if (BlackStones == 0)
            {
                scoreW += 361;
                ScoresArr[0] = scoreB;
                ScoresArr[1] = scoreW;
            }
            else if (WhiteStones == 0)
            {
                scoreB += 361;
                ScoresArr[0] = scoreB;
                ScoresArr[1] = scoreW;
            }
            return ScoresArr;
        }

        public void RegionFillingB(System.Drawing.Point listB, char fill_color, char boundarycolor)
        {
            if (listB.X < 19 && listB.X >= 0 && listB.Y < 19 && listB.Y >= 0)
            {

                if (mystate[listB.X, listB.Y] != '1' && mystate[listB.X, listB.Y] != fill_color && mystate[listB.X, listB.Y] != 'W')
                {
                    if (mystate[listB.X, listB.Y] == ' ')
                    {
                        Bterritory++;
                        mystate[listB.X, listB.Y] = fill_color;
                    }
                    if (mystate[listB.X, listB.Y] == 'B')
                    {
                        mystate[listB.X, listB.Y] = fill_color;
                    }
                    System.Drawing.Point point1 = new System.Drawing.Point(listB.X + 1, listB.Y);
                    System.Drawing.Point point2 = new System.Drawing.Point(listB.X, listB.Y + 1);
                    System.Drawing.Point point3 = new System.Drawing.Point(listB.X - 1, listB.Y);
                    System.Drawing.Point point4 = new System.Drawing.Point(listB.X, listB.Y - 1);
                    if (mystate[listB.X, listB.Y] == 'W') { Bterritory = 0; return; }

                    RegionFillingB(point1, fill_color, '1');
                    RegionFillingB(point2, fill_color, '1');
                    RegionFillingB(point3, fill_color, '1');
                    RegionFillingB(point4, fill_color, '1');
                }
            }
        }
        public void RegionFillingW(System.Drawing.Point listW, char fill_color, char boundarycolor)
        {
            if (listW.X < 19 && listW.X >= 0 && listW.Y < 19 && listW.Y >= 0)
            {
                if (mystate[listW.X, listW.Y] != '2' && mystate[listW.X, listW.Y] != fill_color)
                {
                    if (mystate[listW.X, listW.Y] == ' ') { Wterritory++; mystate[listW.X, listW.Y] = fill_color; }
                    if (mystate[listW.X, listW.Y] == 'W') { mystate[listW.X, listW.Y] = fill_color; }
                    if (mystate[listW.X, listW.Y] == 'B') { Wterritory = 0; return; }
                    System.Drawing.Point point1 = new System.Drawing.Point(listW.X + 1, listW.Y);
                    System.Drawing.Point point2 = new System.Drawing.Point(listW.X, listW.Y + 1);
                    System.Drawing.Point point3 = new System.Drawing.Point(listW.X - 1, listW.Y);
                    System.Drawing.Point point4 = new System.Drawing.Point(listW.X, listW.Y - 1);


                    RegionFillingW(point1, fill_color, '1');
                    RegionFillingW(point2, fill_color, '1');
                    RegionFillingW(point3, fill_color, '1');
                    RegionFillingW(point4, fill_color, '1');
                }
            }
        }
    }
}

