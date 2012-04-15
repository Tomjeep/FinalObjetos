using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rockets_Sesion_II.Model
{
    /// <summary>
    /// Ulacit
    /// DTO that will contain information regrading a user in the game
    /// </summary>
    public class Player : BaseDTO
    {
        /// <summary>
        /// Store position of rocket cannon and base (x,y)
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// Will indicate status of the current player
        /// </summary>
        public bool IsAlive { get; set; }
        /// <summary>
        /// Stores information of the angle used to shoot the rocket
        /// </summary>
        public float Angle { get; set; }
        /// <summary>
        /// Stores inforrmation of the power of each cannon!
        /// </summary>
        public float Power { get; set; }
        /// <summary>
        /// Color foreach player
        /// </summary>
        public Color Color { get; set; }
    }
}
