using System.Collections.Generic;

namespace ConsoleSnakeGame
{
    // Represents the a list of changes in the game board done in a single "frame"
    // Using this information, the game can draw only the changes to the console and avoid flickering
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
