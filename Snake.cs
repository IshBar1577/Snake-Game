using System.Collections.Generic;

namespace ConsoleSnakeGame
{
    // Represents the snake in the game
    public class Snake
    {
        // Sequencial representation of the snake's body - access to the head and tail is O(1)
        private readonly LinkedList<Position> body;
        // Random access representation of the snake's body - query any position is O(1) / O(log n)
        private readonly HashSet<Position> bodySet;

        public Snake(Position initialPosition)
        {
            body = new LinkedList<Position>();
            bodySet = new HashSet<Position>();
            AddSegment(initialPosition);
        }

        // Check if the snake contains a given position
        public bool Contains(Position position)
        {
            return bodySet.Contains(position);
        }

        // Get the next position of the snake's head based on a given direction
        public Position GetNextPosition(Direction direction)
        {
            var head = GetHead();
            return direction switch
            {
                Direction.Up => new Position(head.X, head.Y - 1),
                Direction.Down => new Position(head.X, head.Y + 1),
                Direction.Left => new Position(head.X - 1, head.Y),
                Direction.Right => new Position(head.X + 1, head.Y),
                _ => head
            };
        }

        // Actually move the snake to the next position
        // This member function assumes that the next position is was provided by GetNextPosition
        public void Move(Position nextPosition)
        {
            var tail = GetTail();
            body.RemoveLast();
            bodySet.Remove(tail);

            AddSegment(nextPosition);
        }

        // Adds a new segment to the snake's body, without removing the tail (the snake ate a fruit)
        // This member function assumes that the next position is was provided by GetNextPosition
        public void Grow(Position nextPosition)
        {
            AddSegment(nextPosition);
        }

        // Internally add to both data structures
        private void AddSegment(Position position)
        {
            body.AddFirst(position);
            bodySet.Add(position);
        }

        public Position GetHead()
        {
            return body.First.Value;
        }

        public Position GetTail()
        {
            return body.Last.Value;
        }

        public int GetSize()
        {
            return body.Count;
        }
    }
}
