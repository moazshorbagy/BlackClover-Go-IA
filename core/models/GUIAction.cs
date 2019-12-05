using System.Numerics;

namespace testProject.core.models
{
    public class GUIAction
    {
        public Vector2 position;
        public bool isAddition;
        public int turn;
        public GUIAction(int x, int y, bool isAddition, int turn)
        {
            position = new Vector2(x, y);
            this.isAddition = isAddition;
            this.turn = turn;
        }
    }
}
