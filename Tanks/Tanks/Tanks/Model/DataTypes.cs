using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tanks.Model
{
    class DataTypes
    {
        public enum Direction
        {
            Left = 90, 
            Right = 270, 
            Down = 0, 
            Up = 180

        }

        public enum CellType
        {
            Trees, Stone, Player
        }
    }
}
