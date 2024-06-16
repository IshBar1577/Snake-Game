using NUnit.Framework;
using NUnit.Framework.Legacy;
using ConsoleSnakeGame;

namespace ConsoleSnakeGameTests
{
    [TestFixture]
    public class PositionTests
    {
        private Position pos1;
        private Position pos2;
        private Position pos3;

        [SetUp]
        public void Setup()
        {
            pos1 = new Position(1, 2);
            pos2 = new Position(2, 3);
            pos3 = new Position(2, 3);
        }

        [Test]
        public void Equals_SamePosition_ReturnsTrue()
        {
            ClassicAssert.True(pos2.Equals(pos3));
        }

        [Test]
        public void Equals_DifferentPosition_ReturnsFalse()
        {
            ClassicAssert.False(pos1.Equals(pos2));
        }

        [Test]
        public void GetHashCode_SamePosition_SameHashCode()
        {
            ClassicAssert.AreEqual(pos2.GetHashCode(), pos3.GetHashCode());
        }

        [Test]
        public void GetHashCode_DifferentPosition_DifferentHashCode()
        {
            ClassicAssert.AreNotEqual(pos1.GetHashCode(), pos2.GetHashCode());
        }
    }
}
