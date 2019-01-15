using System;

namespace Crypt
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        public static CryptGame Game { get; internal set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            using (var game = new CryptGame())
            {
                Game = game;
                Game.Run();
            }
        }
    }
#endif
}
