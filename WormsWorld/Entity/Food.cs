namespace WormsWorld.Entity
{
    public class Food
    {
        public Cell Position;
        public int Lifetime = 10;

        public Food(Cell position)
        {
            Position = position;
        }
    }
}