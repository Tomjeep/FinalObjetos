using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rockets_Sesion_II.Model
{
    internal class Smoke : BaseDTO
    {
        public Smoke(Vector2 position)
        {
            Position = position;
            TimesRemaining = 50;
        }

        public int TimesRemaining { get; set; }
    }
}
