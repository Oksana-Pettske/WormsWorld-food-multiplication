using System;
using System.IO;
using System.Linq;
using WormsWorld.Enum;

namespace WormsWorld.Entity
{
    public class Worm
    {
        public World World { get; }
        
        public readonly string Name;
        public Cell Position;
        public int Vitality;

        private readonly WormMover _wormMover;

        public Worm(World world, WormMover wormMover, string name, int x, int y)
        {
            World = world;
            _wormMover = wormMover;
            Name = name;
            Position.X = x;
            Position.Y = y;
            Vitality = 10;
        }

        public void Move()
        {
            WormMover.Move(this);
        }

        public static WormAction GetWormAction()
        {
            var values = System.Enum.GetValues(typeof(WormAction));
            var random = new Random();
            return (WormAction) values.GetValue(random.Next(values.Length));
        }

        public void Multiply(Worm worm)
        {
            if (worm.Vitality <= 10) return;
            worm.Vitality -= 10;
            var direction = GetMultiplyDirection();
            var name = GenerateWormName();
            while (IsWormNameExist(name))
            {
                name = GenerateWormName();
            }
            switch (direction)
            {
                case Direction.Up:
                    if (IsCellFree(new Cell(worm.Position.X, worm.Position.Y + 1)))
                    {
                        World.CreateWorm(name, worm.Position.X, worm.Position.Y + 1);
                    }
                    break;
                case Direction.Right:
                    if (IsCellFree(new Cell(worm.Position.X + 1, worm.Position.Y)))
                    {
                        World.CreateWorm(name, worm.Position.X + 1, worm.Position.Y);
                    }
                    break;
                case Direction.Down:
                    if (IsCellFree(new Cell(worm.Position.X, worm.Position.Y - 1)))
                    {
                        World.CreateWorm(name, worm.Position.X, worm.Position.Y - 1);
                    }
                    break;
                case Direction.Left:
                    if (IsCellFree(new Cell(worm.Position.X - 1, worm.Position.Y)))
                    {
                        World.CreateWorm(name, worm.Position.X - 1, worm.Position.Y);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static Direction GetMultiplyDirection()
        {
            var values = System.Enum.GetValues(typeof(Direction));
            var random = new Random();
            return (Direction) values.GetValue(random.Next(values.Length));
        }
        
        private static string GenerateWormName()
        {
            string[] name =
            {
                "Elizabeth", "Victoria", "Eadmund", "William", "Stephen",
                "Matilda", "Richard", "Charles", "Harold", "Edward",
                "Philip", "George", "Henry", "Louis", "James",
                "John", "Jane", "Mary", "Mary", "Anne"
            };
            string[] nickname =
            {
                "the Magnificent", "the Peaceable", "the Confessor", "the Conqueror",
                "the Lionheart", "the Glorious", "the Bastard", "Longshanks",
                "Curtmantle", "the Martyr", "Beauclerc", "Godwinson",
                "Forkbeard", "the Great", "the Elder", "Ironside",
                "Lackland", "Harefoot", "All-Fair", "Rufus"
            };
            var random = new Random();
            return (string) name.GetValue(random.Next(name.Length)) + " " +
                   (string) nickname.GetValue(random.Next(nickname.Length));
        }

        private bool IsWormNameExist(string name)
        {
            return World.Worms.Any(worm => worm.Name.Equals(name));
        }

        private bool IsCellFree(Cell cell)
        {
            if (World.Worms.Any(worm => worm.Position.Equals(cell)))
            {
                return false;
            }
            if (World.Foods.Any(food => food.Position.Equals(cell)))
            {
                return false;
            }
            return true;
        }
    }
}