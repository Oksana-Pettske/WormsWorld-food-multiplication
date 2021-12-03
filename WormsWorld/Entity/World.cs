using System.Collections.Generic;
using WormsWorld.Enum;
using System.Linq;
using System.IO;
using System;

namespace WormsWorld.Entity
{
    public class World
    {
        private readonly StreamWriter _streamWriter;

        public readonly List<Worm> Worms = new List<Worm>();
        public readonly List<Food> Foods = new List<Food>();

        public World(StreamWriter streamWriter)
        {
            _streamWriter = streamWriter;
        }
        
        public void Start()
        {
            for (var day = 0; day < 100; day++)
            {
                GenerateFood();
                IsFoodEaten();
                WormAct();
                IsWormDead();
                CutFoodLifetime();
                IsFoodRotten();
                WriteHistory();
            }
        }
        
        public void CreateWorm(string name, int x, int y)
        {
            Worms.Add(new Worm(this, new WormMover(), name, x, y));
        }

        private void GenerateFood()
        {
            Cell cell;
            var done = true;
            while (true)
            {
                cell = GenerateCell();
                foreach (var food in Foods)
                {
                    if (cell.Equals(food.Position))
                    {
                        done = false;
                        break;
                    }
                    done = true;
                }
                if (done)
                {
                    break;
                }
            }
            Foods.Add(new Food(cell));
        }

        private static Cell GenerateCell()
        {
            Random random = new Random();
            int x = NormalDistribution.NextNormal(random);
            int y = NormalDistribution.NextNormal(random);
            return new Cell(x, y);
        }

        private void IsFoodEaten()
        {
            foreach (var worm in Worms)
            {
                foreach (var food in Foods.ToList().Where(food => food.Position.Equals(worm.Position)))
                {
                    worm.Vitality += 10;
                    Foods.Remove(food);
                }
            }
        }

        private void WormAct()
        {
            foreach (var worm in Worms.ToList())
            {
                var action = Worm.GetWormAction();
                switch (action)
                {
                    case WormAction.Move:
                        worm.Move();
                        break;
                    case WormAction.Stay:
                        break;
                    case WormAction.Multiply:
                        worm.Multiply(worm);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                worm.Vitality--;
            }
        }

        private void IsWormDead()
        {
            foreach (var worm in Worms.ToList().Where(worm => worm.Vitality <= 0))
            {
                Worms.Remove(worm);
            }
        }

        private void CutFoodLifetime()
        {
            foreach (var food in Foods)
            {
                food.Lifetime--;
            }
        }

        private void IsFoodRotten()
        {
            foreach (var food in Foods.ToList().Where(food => food.Lifetime <= 0))
            {
                Foods.Remove(food);
            }
        }
        
        private void WriteHistory()
        {
            _streamWriter.Write("Worms: [");
            foreach (var worm in Worms)
            {
                _streamWriter.Write(worm.Name + " (" + worm.Position.X + ", " + worm.Position.Y + ")], ");
            }
            _streamWriter.Write("Food: [");
            var i = 0;
            foreach (var food in Foods.ToList())
            {
                _streamWriter.Write("(" + food.Position.X + ", " + food.Position.Y + ")");
                i++;
                if (i < Foods.Count)
                {
                    _streamWriter.Write(", ");
                }
            }
            _streamWriter.Write("]\n");
        }
    }
}