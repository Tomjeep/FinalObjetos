using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Tanks.Model
{
    class CannonBall
    {
        private const float speed = 4.5f;

        private Vector2 position;

        public Vector2 Position
        {
            get
            {
                Vector2 response = position;
                MoveBullet();
                return response;
            } 
            set { position = value; }
        }        

        public DataTypes.Direction Direction { get; set; }


        private void MoveBullet()
        {
            switch (Direction)
            {
                    case DataTypes.Direction.Right:
                    position.X += 4.5f;
                    break;
                    case DataTypes.Direction.Left:
                    position.X -= 4.5f;
                    break;
                    case DataTypes.Direction.Up:
                    position.Y -= 4.5f;
                    break;
                    case DataTypes.Direction.Down:
                    position.Y += 4.5f;
                    break;
            }
        }
    }
}
