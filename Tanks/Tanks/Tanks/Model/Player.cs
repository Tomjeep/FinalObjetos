using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tanks.Model
{
    class Player
    {
        public int Id { get; set; }

        public bool IsAlive { get; set; }

        public float RowDisplacement { get; set; }

        public float ColumnDisplacement { get; set; }

        public float ReloadTime { get; set; }

        public int Row { get; set; }

        public int Column { get; set; }

        public Color Color { get; set; }

        public DataTypes.Direction Direction { get; set; }

        public CannonBall CannonBall { get; set; }

        public void Shoot (Vector2 position)
        {
            CannonBall = new CannonBall()
                             {
                                 Position = position,
                                 Direction = Direction
                             };
        }
        
    }
}
