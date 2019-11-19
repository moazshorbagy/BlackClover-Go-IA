public class ProjectAction
{
    int X;
    int Y;
    string Color;
    public ProjectAction(int x, int y, string c) { X = x; Y = y; Color = c; }
    public void setX(int x) { if (x >= 0 && x <= 18) X = x; else { Console.WriteLine("Invalid Coordinate."); } }
    public int getX() { return X; }
    public void setY(int x) { if (x >= 0 && x <= 18) Y = x; else { Console.WriteLine("Invalid Coordinate."); } }
    public int getY() { return Y; }
    public void setColor(string x) { if (x == "BLACK" || x == "WHITE") Color = x; else { Console.WriteLine("Invalid Color."); } }
    public string getColor() { return Color; }

}

static public List<ProjectAction> PossibleActions(ProjectState x)
{
    var PossibleActions = new List<ProjectAction>();
    string Color;
    if (x.GetTurn() == 1) Color = "WHITE"; else { Color = "BLACK"; }
    int[,] Board2 = new int[19, 19];
    for (int i = 0; i < 19; i++)
    {

        for (int j = 0; j < 19; j++)
        {
            if (x.GetBoard()[i, j] == -1)
            {
                x.AddStone(i, j);
                Board2 = new int[19, 19];
                if (x.GetLiberty(i, j, Board2) == 0)
                {
                    x.RemoveStone(i, j);
                }
                else
                {
                    x.RemoveStone(i, j);
                    PossibleActions.Add(new ProjectAction(i, j, Color));
                }
            }
        }
    }
    return PossibleActions;
}