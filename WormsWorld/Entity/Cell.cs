﻿using System;

namespace WormsWorld.Entity
{
    public struct Cell : IEquatable<Cell>
    {
        public int X;
        public int Y;
        
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public bool Equals(Cell other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}