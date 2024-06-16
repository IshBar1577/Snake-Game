using NUnit.Framework;
using NUnit.Framework.Legacy;
using ConsoleSnakeGame;

namespace ConsoleSnakeGameTests
{
    [TestFixture]
    public class SnakeTests
    {
        private Snake snake;

        [SetUp]
        public void Setup()
        {
            snake = new Snake(new Position(2, 2));
        }

        [Test]
        public void GetNextPosition_Up_ReturnsCorrectPosition()
        {
            var nextPosition = snake.GetNextPosition(Direction.Up);
            ClassicAssert.AreEqual(new Position(2, 1), nextPosition);
        }

        [Test]
        public void GetNextPosition_Down_ReturnsCorrectPosition()
        {
            var nextPosition = snake.GetNextPosition(Direction.Down);
            ClassicAssert.AreEqual(new Position(2, 3), nextPosition);
        }

        [Test]
        public void GetNextPosition_Left_ReturnsCorrectPosition()
        {
            var nextPosition = snake.GetNextPosition(Direction.Left);
            ClassicAssert.AreEqual(new Position(1, 2), nextPosition);
        }

        [Test]
        public void GetNextPosition_Right_ReturnsCorrectPosition()
        {
            var nextPosition = snake.GetNextPosition(Direction.Right);
            ClassicAssert.AreEqual(new Position(3, 2), nextPosition);
        }

        [Test]
        public void Move_UpdatesHeadAndTail()
        {
            var newPosition = new Position(3, 2);
            snake.Move(newPosition);
            ClassicAssert.AreEqual(newPosition, snake.GetHead());
            ClassicAssert.AreEqual(newPosition, snake.GetTail());
        }

        [Test]
        public void Grow_IncreasesLength()
        {
            var newPosition = new Position(3, 2);
            snake.Grow(newPosition);
            ClassicAssert.AreEqual(2, snake.GetSize());
        }

        [Test]
        public void IsPosition_SnakeBodyPosition_ReturnsTrue()
        {
            var newPosition = new Position(3, 2);
            snake.Grow(newPosition);
            ClassicAssert.True(snake.Contains(newPosition));
            ClassicAssert.AreEqual(newPosition, snake.GetHead());
            ClassicAssert.AreEqual(new Position(2, 2), snake.GetTail());
        }

        [Test]
        public void IsPosition_NotSnakeBodyPosition_ReturnsFalse()
        {
            var newPosition = new Position(3, 2);
            ClassicAssert.False(snake.Contains(newPosition));
        }
    }
}
