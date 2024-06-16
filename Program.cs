using Microsoft.VisualBasic;
using System;

namespace ConsoleSnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.CursorVisible = false;
                Console.SetWindowSize(Constants.BoardWidth, Constants.BoardHeight + 5);

                SnakeGameField game = new SnakeGameField(Constants.BoardWidth, Constants.BoardHeight);
                MainLoop mainLoop = new MainLoop(game);
                mainLoop.Run();

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
