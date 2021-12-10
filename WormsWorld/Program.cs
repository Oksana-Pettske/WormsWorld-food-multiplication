using System.IO;
using WormsWorld.Entity;

namespace WormsWorld
{
    internal static class Program
    {
        private static void Main()
        {
            using var streamWriter = new StreamWriter("AboutWorms.txt");
            var world = new World(streamWriter);
            world.CreateWorm("John the First", 0, 0);
            world.Start();
        }
    }
}