using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farm_Prototype.Interface
{
    public class GridCell : Component
    {
        private MouseState _currentMouse;

        private bool _isHovering;

        private MouseState _previousMouse;

        private Vector2 _cellSize = new Vector2(8, 8);

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColor { get; set; } = Color.Black;

        public Color CellColor { get; set; } = Color.Green;

        public Color HoverColor { get; set; } = Color.LightBlue;

        public Vector2 Position { get; set; }

        public Vector2 Scale { get; set; } = new Vector2(1, 1);

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)_cellSize.X, (int)_cellSize.Y);
            }
        }

        public GridCell(Color cellColor_)
        {
            CellColor = cellColor_;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // draw
            var color = CellColor;

            if (_isHovering.Equals(true))
            {
                color = HoverColor;
            }

            var txt = new Texture2D(spriteBatch.GraphicsDevice, (int)_cellSize.X, (int)_cellSize.Y);
            Color[] colordata = new Color[(int)_cellSize.X * (int)_cellSize.Y];
            for(int i = 0; i < colordata.Length; i++)
            {
                colordata[i] = color;
            }
            txt.SetData(colordata);
            spriteBatch.Draw(txt, Rectangle, color);
        }

        public override void Update(GameTime gameTime)
        {
            // update
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
