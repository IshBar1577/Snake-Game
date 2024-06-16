namespace ConsoleSnakeGame
{
    public static class Constants
    {
        public const char SnakeChar = 'O';
        public const char FoodChar = '*';
        public const char EmptyChar = '·';
        public const char WallChar = '#';
        public const int BoardWidth = 30;
        public const int BoardHeight = 20;
        public const int InitialSpeed = 200;
        public const int SpeedIncrease = 50;
        public const int MaxTries = 10;
        public const double ExtraFoodChance = 0.05;
        public const int FruitsToWin = 10;
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
}
