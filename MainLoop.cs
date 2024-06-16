using System;
using System.Threading;

namespace ConsoleSnakeGame
{
    public class MainLoop
    {
        private readonly SnakeGameField game;

        public MainLoop(SnakeGameField game)
        {
            this.game = game;
        }

        public void Run()
        {
            try
            {
                game.StartLevel();
                while (!game.GameOver)
                {
                    game.Input();
                    game.Update();
                    game.DrawDelta();
                    Thread.Sleep(game.GetSpeed());
                }
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"An error occurred in the game loop: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey(true);
            }
        }
    }
}
