using System;
using WormsWorld.Entity;

namespace WormsWorld
{
    public static class WormMover
    {
        public static void Move(Worm worm)
        {
            var closestFoodCell = FindClosestFoodCell(worm);
            if (Math.Abs(closestFoodCell.X) > Math.Abs(worm.Position.X))
            {
                worm.Position.X++;
            } else if (Math.Abs(closestFoodCell.X) < Math.Abs(worm.Position.X))
            {
                worm.Position.X--;
            } else if (Math.Abs(closestFoodCell.Y) > Math.Abs(worm.Position.Y))
            {
                worm.Position.Y++;
            } else if (Math.Abs(closestFoodCell.Y) > Math.Abs(worm.Position.Y))
            {
                worm.Position.Y--;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private static Cell FindClosestFoodCell(Worm worm)
        {
            var minDistance = int.MaxValue;
            var closestFoodCell = new Cell();
            foreach (var food in worm.World.Foods)
            {
                var deltaX = Math.Abs(food.Position.X - worm.Position.X);
                var deltaY = Math.Abs(food.Position.Y - worm.Position.Y);
                if (deltaX + deltaY < minDistance)
                {
                    closestFoodCell.X = deltaX;
                    closestFoodCell.Y = deltaY;
                    minDistance = deltaX + deltaY;
                }
            }
            return closestFoodCell;
        }
    }
}