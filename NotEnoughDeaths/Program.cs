// Copyright (c) 2020 PshenoDev. See the MIT license for full information

namespace NotEnoughDeaths
{
    public static class Program
    {
        public static int Main()
        {
            using (GameDesktop game = new GameDesktop())
            {
                if (!game.IsPrimaryInstance)
                {
                    game.ShowWarning("Only one instance of the game can be running at one time.");
                    return 0;
                }

                game.Run();

                return 0;
            }
        }
    }
}
