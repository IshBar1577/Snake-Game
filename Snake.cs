using System.Collections.Generic;

namespace ConsoleSnakeGame
{
    public class Snake
    {
        private readonly LinkedList<Position> body;
        private readonly HashSet<Position> bodySet;

        public Snake(Position initialPosition)
        {
            body = new LinkedList<Position>();
            bodySet = new HashSet<Position>();
            AddSegment(initialPosition);
        }

        public bool Contains(Position position)
        {
            return bodySet.Contains(position);
        }

        public Position GetNextPosition(Direction direction)
        {
            var head = body.First.Value;
            return direction switch
            {
                Direction.Up => new Position(head.X, head.Y - 1),
                Direction.Down => new Position(head.X, head.Y + 1),
                Direction.Left => new Position(head.X - 1, head.Y),
                Direction.Right => new Position(head.X + 1, head.Y),
                _ => head
            };
        }

        public void Move(Position nextPosition)
        {
            var tail = body.Last.Value;
            body.RemoveLast();
            bodySet.Remove(tail);

            AddSegment(nextPosition);
        }

        public void Grow(Position nextPosition)
        {
            AddSegment(nextPosition);
        }

        private void AddSegment(Position position)
        {
            body.AddFirst(position);
            bodySet.Add(position);
        }

        public Position GetTail()
        {
            return body.Last.Value;
        }
    }
}
