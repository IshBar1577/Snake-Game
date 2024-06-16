using System;
using System.Collections.Generic;
using static ConsoleSnakeGame.Constants;

namespace ConsoleSnakeGame
{
    // Represents the game field and all its components
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
            // Generate the initial food
            food.Add(GenerateFood());
            currentDirection = Direction.Right;
            nextDirection = Direction.Right;
            delta = new GameDelta();
            speed = Constants.InitialSpeed;
            level = 1;
            levelScore = FruitsToWin;
        }

        // Draw the board and the initial message to start the level
        public void StartLevel()
        {
            Console.Clear();
            DrawBoard();
            DrawStatusMessage($"Press any key to start Level {level}");
            Console.ReadKey(true);
            DrawStatusMessage(string.Empty);
        }

        // Clear the screen and draw the board from scratch
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

        // Draw only the changes that happened since the last snake movement, avoiding flickering
        public void DrawDelta()
        {
            foreach (var position in delta.ChangedPositions)
            {
                // Position the cusror at the right spot
                Console.SetCursorPosition(position.X, position.Y);
                // Draw the right character
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

        // Update the score and level display
        private void DrawScoreAndLevel()
        {
            Console.SetCursorPosition(0, height);
            Console.WriteLine($"Score: {score}            ");
            Console.SetCursorPosition(0, height + 1);
            Console.WriteLine($"Level: {level}            ");
        }

        // Draw a message at the bottom of the screen, such as game over or level starts
        public void DrawStatusMessage(string message)
        {
            Console.SetCursorPosition(0, height + 3);
            Console.WriteLine(message.PadRight(Console.WindowWidth));
        }

        // Handle the user input. The user can change the snake's direction via the arrow keys, numpad arrow keys or WASD
        // Ctrl+C can also be used to exit the game
        public void Input()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;

                nextDirection = key switch
                {
                    ConsoleKey.NumPad8 or ConsoleKey.UpArrow or ConsoleKey.W when currentDirection != Direction.Down => Direction.Up,
                    ConsoleKey.NumPad2 or ConsoleKey.DownArrow or ConsoleKey.S when currentDirection != Direction.Up => Direction.Down,
                    ConsoleKey.NumPad4 or ConsoleKey.LeftArrow or ConsoleKey.A when currentDirection != Direction.Right => Direction.Left,
                    ConsoleKey.NumPad6 or ConsoleKey.RightArrow or ConsoleKey.D when currentDirection != Direction.Left => Direction.Right,
                    _ => nextDirection
                };
            }
        }

        // Update the game state, based on the user input and the current state of the snake
        public void Update()
        {
            currentDirection = nextDirection;
            var nextPosition = snake.GetNextPosition(currentDirection);
            // Check if the snake hit a wall, itself or an internal wall
            if (nextPosition.X == 0 || nextPosition.X == width - 1 || nextPosition.Y == 0 || nextPosition.Y == height - 1 || snake.Contains(nextPosition) || walls.Contains(nextPosition))
            {
                gameOver = true;
                DrawStatusMessage("Press any key to exit...");
            }
            else
            {
                // The snake has survived another step
                var tail = snake.GetTail();
                // Check if the snake ate a fruit
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
                        // The board gets another wall every level, making it harder to navigate
                        AddWall();
                        StartLevel();
                    }
                }
                // Else, the snake moves normally
                else
                {
                    snake.Move(nextPosition);
                    delta.AddChange(tail);
                }
                delta.AddChange(nextPosition);
            }
        }

        // Generate a new food item at a random position on the board
        private Position GenerateFood()
        {
            Position position;
            int tries = 0;
            // A simplistic mechanism to generate food - try for N times (configurable) and skip if none were found
            do
            {
                position = new Position(random.Next(1, width - 1), random.Next(1, height - 1));
                tries++;
            } while ((snake.Contains(position) || food.Contains(position) || walls.Contains(position)) && tries < MaxTries);

            return position;
        }

        // Add a wall to the board, making it harder to navigate
        private void AddWall()
        {
            // A simplistic mechanism to find a free spot for the wall - try for N times (configurable) and skip if none were found
            for (int tries = 0; tries < MaxTries; tries++)
            {
                // Randomize the wall's orientation, length and position
                bool horizontal = random.Next(2) == 0;
                int length = random.Next(3, 6);
                int x = random.Next(1, width - 1);
                int y = random.Next(1, height - 1);
                // Add 2 - 4 wall segments in a row, making sure none lands on the snake or food
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
                // If all was good, add the wall to the relevant data structures and exit
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
