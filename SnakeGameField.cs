using System;
using System.Collections.Generic;
using static ConsoleSnakeGame.Constants;

namespace ConsoleSnakeGame
{
    public class SnakeGameField
    {
        private readonly int width;
        private readonly int height;
        private int score;
        private int levelScore;
        private int level;
        private bool gameOver;
        private Snake snake;
        private List<Position> food;
        private Random random;
        private Direction currentDirection;
        private Direction nextDirection;
        private HashSet<Position> walls;
        private GameDelta delta;
        private int speed;

        public bool GameOver => gameOver;
        public int Score => score;

        public SnakeGameField(int width, int height)
        {
            this.width = width;
            this.height = height;
            random = new Random();
            snake = new Snake(new Position(width / 2, height / 2));
            food = new List<Position>();
            walls = new HashSet<Position>();
            food.Add(GenerateFood());
            currentDirection = Direction.Right;
            nextDirection = Direction.Right;
            delta = new GameDelta();
            speed = Constants.InitialSpeed;
            level = 1;
            levelScore = FruitsToWin;
        }

        public void StartLevel()
        {
            Console.Clear();
            DrawBoard();
            DrawStatusMessage($"Press any key to start Level {level}");
            Console.ReadKey(true);
            DrawStatusMessage(string.Empty);
        }

        private void DrawBoard()
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pos = new Position(x, y);
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        Console.Write(WallChar);
                    }
                    else if (snake.Contains(pos))
                    {
                        Console.Write(SnakeChar);
                    }
                    else if (food.Contains(pos))
                    {
                        Console.Write(FoodChar);
                    }
                    else if (walls.Contains(pos))
                    {
                        Console.Write(WallChar);
                    }
                    else
                    {
                        Console.Write(EmptyChar);
                    }
                }
                Console.WriteLine();
            }
        }

        public void DrawDelta()
        {
            foreach (var position in delta.ChangedPositions)
            {
                Console.SetCursorPosition(position.X, position.Y);
                if (snake.Contains(position))
                {
                    Console.Write(SnakeChar);
                }
                else if (food.Contains(position))
                {
                    Console.Write(FoodChar);
                }
                else if (walls.Contains(position) || position.X == 0 || position.X == width - 1 || position.Y == 0 || position.Y == height - 1)
                {
                    Console.Write(WallChar);
                }
                else
                {
                    Console.Write(EmptyChar);
                }
            }

            DrawScoreAndLevel();
            delta.ClearChanges();
        }

        private void DrawScoreAndLevel()
        {
            Console.SetCursorPosition(0, height);
            Console.WriteLine($"Score: {score}            ");
            Console.SetCursorPosition(0, height + 1);
            Console.WriteLine($"Level: {level}            ");
        }

        public void DrawStatusMessage(string message)
        {
            Console.SetCursorPosition(0, height + 3);
            Console.WriteLine(message.PadRight(Console.WindowWidth));
        }

        public void Input()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                nextDirection = key switch
                {
                    ConsoleKey.UpArrow or ConsoleKey.W when currentDirection != Direction.Down => Direction.Up,
                    ConsoleKey.DownArrow or ConsoleKey.S when currentDirection != Direction.Up => Direction.Down,
                    ConsoleKey.LeftArrow or ConsoleKey.A when currentDirection != Direction.Right => Direction.Left,
                    ConsoleKey.RightArrow or ConsoleKey.D when currentDirection != Direction.Left => Direction.Right,
                    _ => nextDirection
                };
            }
        }

        public void Update()
        {
            currentDirection = nextDirection;
            var nextPosition = snake.GetNextPosition(currentDirection);

            if (nextPosition.X == 0 || nextPosition.X == width - 1 || nextPosition.Y == 0 || nextPosition.Y == height - 1 || snake.Contains(nextPosition) || walls.Contains(nextPosition))
            {
                gameOver = true;
                DrawStatusMessage("Press any key to exit...");
            }
            else
            {
                var tail = snake.GetTail();

                if (food.Contains(nextPosition))
                {
                    snake.Grow(nextPosition);
                    food.Remove(nextPosition);
                    score++;
                    levelScore--;
                    // Generate a new food instead of the 1 eaten
                    food.Add(GenerateFood());
                    delta.AddChange(food[food.Count - 1]);
                    // 5% chance to generate an additional food item
                    if (random.NextDouble() < ExtraFoodChance)
                    {
                        food.Add(GenerateFood());
                        delta.AddChange(food[food.Count - 1]);
                    }
                    // Increase speed every 5 fruits eaten
                    if (score % 5 == 0)
                    {
                        speed = Math.Max(50, speed - SpeedIncrease);
                    }
                    // Check for level completion
                    if (levelScore == 0)
                    {
                        level++;
                        levelScore = FruitsToWin;
                        AddWall();
                        StartLevel();
                    }
                }
                else
                {
                    snake.Move(nextPosition);
                    delta.AddChange(tail);
                }
                delta.AddChange(nextPosition);
            }
        }

        private Position GenerateFood()
        {
            Position position;
            int tries = 0;
            do
            {
                position = new Position(random.Next(1, width - 1), random.Next(1, height - 1));
                tries++;
            } while ((snake.Contains(position) || food.Contains(position) || walls.Contains(position)) && tries < MaxTries);

            return position;
        }

        private void AddWall()
        {
            for (int tries = 0; tries < MaxTries; tries++)
            {
                bool horizontal = random.Next(2) == 0;
                int length = random.Next(3, 6);
                int x = random.Next(1, width - 1);
                int y = random.Next(1, height - 1);

                List<Position> newWall = new List<Position>();
                for (int i = 0; i < length; i++)
                {
                    Position pos = horizontal ? new Position(x + i, y) : new Position(x, y + i);
                    if (pos.X >= width - 1 || pos.Y >= height - 1 || snake.Contains(pos) || food.Contains(pos))
                    {
                        newWall.Clear();
                        break;
                    }
                    newWall.Add(pos);
                }

                if (newWall.Count == length)
                {
                    foreach (var pos in newWall)
                    {
                        walls.Add(pos);
                        delta.AddChange(pos);
                    }
                    return;
                }
            }
        }

        public int GetSpeed()
        {
            return speed;
        }
    }
}
