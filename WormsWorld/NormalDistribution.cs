using System;

namespace WormsWorld
{
    public static class NormalDistribution
    {
        public static int NextNormal(Random random, double mu = 0, double sigma = 1)
        {
            var u1 = random.NextDouble();
            var u2 = random.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            var randNormal = mu + sigma * randStdNormal;
            return (int) Math.Round(randNormal);
        }
    }
}