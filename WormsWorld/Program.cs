using System.IO;
using WormsWorld.Entity;

namespace WormsWorld
{
    class Program
    {
        static void Main()
        {
            StreamWriter streamWriter = new StreamWriter("AboutWorms.txt");
            World world = new World(streamWriter);
            world.CreateWorm("John the First", 0, 0);
            world.Start();
            streamWriter.Close();
        }
    }
}