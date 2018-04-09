using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farm_Prototype.Objects
{
    public class Tile
    {
        public Texture2D texture { get; set; }

        public Vector2 position { get; set; }

        public Vector2 scale { get; set; } = new Vector2(1, 1);

        public Plant innerPlant { get; set; } = null;

        public bool isDecoration { get; set; } = false;

        public int depth
        {
            get { return (int)Math.Round(position.Y * -1); }
        }
    }
}
