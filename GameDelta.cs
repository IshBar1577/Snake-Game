using System.Collections.Generic;

namespace ConsoleSnakeGame
{
    public class GameDelta
    {
        public List<Position> ChangedPositions { get; } = new List<Position>();

        public void AddChange(Position position)
        {
            ChangedPositions.Add(position);
        }

        public void ClearChanges()
        {
            ChangedPositions.Clear();
        }
    }
}
