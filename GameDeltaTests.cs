using NUnit.Framework;
using NUnit.Framework.Legacy;
using ConsoleSnakeGame;
using System.Collections.Generic;

namespace ConsoleSnakeGameTests
{
    [TestFixture]
    public class GameDeltaTests
    {
        private GameDelta delta;

        [SetUp]
        public void Setup()
        {
            delta = new GameDelta();
        }

        [Test]
        public void AddChange_AddsPositionToChanges()
        {
            var pos1 = new Position(1, 1);
            var pos2 = new Position(2, 3);
            delta.AddChange(pos1);
            ClassicAssert.Contains(pos1, delta.ChangedPositions);
            delta.AddChange(pos2);
            ClassicAssert.Contains(pos1, delta.ChangedPositions);
            ClassicAssert.Contains(pos2, delta.ChangedPositions);
        }

        [Test]
        public void ClearChanges_RemovesAllChanges()
        {
            var pos1 = new Position(1, 1);
            var pos2 = new Position(2, 3);
            delta.AddChange(pos1);
            delta.AddChange(pos2);
            delta.ClearChanges();
            ClassicAssert.IsEmpty(delta.ChangedPositions);
        }
    }
}
