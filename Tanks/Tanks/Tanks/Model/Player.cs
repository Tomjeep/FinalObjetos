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

        private int row;

        private int column;

        private DataTypes.Direction direction;

        public int Row
        {
            get { return row; }
            set { rowMovement = row = value; }
        }

        public int Column
        {
            get { return column; }
            set { columnMovement = column = value; }
        }

        public Color Color { get; set; }

        public int MatrixLastCell { get; set; }

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
            if (rowMovement < 0)
                rowMovement = 0;
            if (columnMovement < 0)
                columnMovement = 0;
            if (rowMovement > MatrixLastCell)
                rowMovement = MatrixLastCell;
            if (columnMovement > MatrixLastCell)
                columnMovement = MatrixLastCell;
            row = Convert.ToInt32(rowMovement);
            column = Convert.ToInt32(columnMovement);
        }
    }
}
