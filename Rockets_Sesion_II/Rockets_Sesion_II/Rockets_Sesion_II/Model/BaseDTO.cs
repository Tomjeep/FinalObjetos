using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rockets_Sesion_II.Model
{
    /// <summary>
    /// Ulacit
    /// Base Data Transfer Object Definitions
    /// </summary>
    public abstract class BaseDTO
    {
        /// <summary>
        /// Store position of rocket cannon and base (x,y)
        /// </summary>
        public Vector2 Position { get; set; }
    }
}
