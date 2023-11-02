using System;

namespace Lodestone {
    public static class Program {
        [STAThread]
        static void Main() {
            using (var game = new Game1())
                game.Run();
        }
    }
}
