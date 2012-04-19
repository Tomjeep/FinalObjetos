using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tanks.Model
{
    class Player
    {
        private float columnMovement;
        
        private float rowMovement;

        private DataTypes.Direction direction;

        public int Row { get; set; }

        public int Column { get; set; }

        public Color Color { get; set; }

        public DataTypes.Direction Direction
        {
            get { return direction; } 
            set 
            { 
                direction = value;
                switch (value)
                {
                    case DataTypes.Direction.Left:
                        columnMovement -= 0.15f;
                        break;
                    case DataTypes.Direction.Right:
                        columnMovement += 0.15f;
                        break;
                    case DataTypes.Direction.Up:
                        rowMovement -= 0.15f;
                        break;
                    case DataTypes.Direction.Down:
                        rowMovement += 0.15f;
                        break;
                }
                EvaluatePlayerMovement();
            }
        }

        private void EvaluatePlayerMovement()
        {
            Row = Convert.ToInt32(rowMovement);
            Column = Convert.ToInt32(columnMovement);
        }
    }
}
