using System;
using System.IO;
using System.Linq;
using WormsWorld.Enum;
using System.Collections.Generic;

namespace WormsWorld.Entity
{
    public class World
    {
        private readonly StreamWriter _streamWriter;

        public readonly List<Worm> Worms = new();
        public readonly List<Food> Foods = new();

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
            Worms.Add(new Worm(this, name, x, y));
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
            var random = new Random();
            var x = NormalDistribution.NextNormal(random);
            var y = NormalDistribution.NextNormal(random);
            return new Cell(x, y);
        }

        private void IsFoodEaten()
        {
            foreach (var worm in Worms)
            {
                var anyFood = Foods.FirstOrDefault(x => x.Position.Equals(worm.Position));
                if (anyFood != null)
                {
                    worm.Vitality += 10;
                    Foods.Remove(anyFood);
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
            var deadWorms = Worms.Where(x => x.Vitality <= 0);
            foreach (var deadWorm in deadWorms)
            {
                Worms.Remove(deadWorm);
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
            var rottenFoods = Foods.Where(x => x.Lifetime <= 0);
            foreach (var rottenFood in rottenFoods)
            {
                Foods.Remove(rottenFood);
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