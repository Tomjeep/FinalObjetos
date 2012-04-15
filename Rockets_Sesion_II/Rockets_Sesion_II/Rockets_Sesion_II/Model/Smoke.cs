using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rockets_Sesion_II.Model
{
    internal class Smoke : BaseDTO
    {

        //Vars
        private int timesRemaining;

        //Properties
        public Color Color { get; set; }


        //Methods
        public Smoke(Vector2 position)
        {
            Position = position;
            timesRemaining = 50;
            Color = new Color(220, 108, 14, 150);
        }

        public bool ShouldBeDeleted()
        {
            AdjustColor();
            return timesRemaining-- < 0;
        }

        private void AdjustColor()
        {
            if (timesRemaining < 20)
            {
                Color = Color.White;
            }
            else
            {
                Color = new Color(250, 108, 14, timesRemaining * 3);
            }
        }

        
    }
}
