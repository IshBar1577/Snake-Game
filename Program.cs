using Microsoft.VisualBasic;
using System;

namespace ConsoleSnakeGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.CursorVisible = false;
                // Set the console window size to the board size + extra space for the score and other announcements
                Console.SetWindowSize(Math.Max(Constants.BoardWidth, 50), Constants.BoardHeight + 5);
                // Initialise the game, the thread and run it and start the game loop
                SnakeGameField game = new SnakeGameField(Constants.BoardWidth, Constants.BoardHeight);
                MainLoop mainLoop = new MainLoop(game);
                mainLoop.Run();
                // Write the final score to the console
                Console.SetCursorPosition(0, Constants.BoardHeight + 2);
                Console.WriteLine($"Game Over! Score: {game.Score}");
                Console.ReadKey(true);
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(true);
            }
            finally
            {
                Console.CursorVisible = true;
            }
        }
    }
}
