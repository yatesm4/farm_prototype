using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Farm_Prototype.Content;
using Farm_Prototype.Objects;

namespace Farm_Prototype.Interface
{
    public class CellDataDisplay : Component
    {
        private GameContent _content { get; set; }
        private SpriteFont _font { get; set; }

        private Tile _currentTile { get; set; }

        private GridCell _currentCell { get; set; } = null;
        public GridCell CurrentCell
        {
            get { return _currentCell; }
            set { _currentCell = value; }
        }
        private GridCell _nextCell { get; set; } = null;
        public GridCell NextCell
        {
            get { return _nextCell; }
            set { _nextCell = value; }
        }

        private bool _isHovering;

        private MouseState _previousMouse;
        private MouseState _currentMouse;

        private Vector2 _displaySize { get; set; } = new Vector2(376, 400);

        public Texture2D Texture { get; set; }
        public Texture2D HoverTexture { get; set; }
        public Color DisplayColor { get; set; } = Color.LightGreen;
        public Color HoverColor { get; set; } = Color.LightSeaGreen;

        public Color[] DisplayColorData { get; set; }
        public Color[] HoverColorData { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; } = new Vector2(1, 1);

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)_displaySize.X, (int)_displaySize.Y);
            }
        }

        public string Header = "Map Editor";
        public string SubHeader = "To edit a tile, select one within the grid to the left.";
        public string TileHeader = $"Current Tile: None Selected";

        public CellDataDisplay(GraphicsDevice graphicsDevice_, GameContent content)
        {
            SetColorData(graphicsDevice_);
            _content = content;
            _font = _content.GetFont(1);
        }

        public void SetColorData(GraphicsDevice graphicsDevice_)
        {
            Texture = new Texture2D(graphicsDevice_, (int)_displaySize.X, (int)_displaySize.Y);
            HoverTexture = new Texture2D(graphicsDevice_, (int)_displaySize.X, (int)_displaySize.Y);
            DisplayColorData = new Color[(int)_displaySize.X * (int)_displaySize.Y];
            HoverColorData = new Color[(int)_displaySize.X * (int)_displaySize.Y];
            for(int i = 0; i < DisplayColorData.Length; i++)
            {
                DisplayColorData[i] = DisplayColor;
                HoverColorData[i] = HoverColor;
            }
            Texture.SetData(DisplayColorData);
            HoverTexture.SetData(HoverColorData);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var txt = Texture;
            if (_isHovering.Equals(true))
            {
                txt = HoverTexture;
            }

            // draw container
            spriteBatch.Draw(txt, Rectangle, Color.White);

            #region DRAW TEXT
            float x;
            float y;
            Vector2 origin;
            // draw Header
            x = (Rectangle.X + (Rectangle.Width / 2));
            y = Rectangle.Y + 12;
            origin = new Vector2(_font.MeasureString(Header).X / 2, _font.MeasureString(Header).Y / 2);
            spriteBatch.DrawString(_font, Header, new Vector2(x, y), Color.Black, 0, origin, new Vector2(1.5f,1.5f), SpriteEffects.None, 1);

            // draw SubHeader
            y = (Rectangle.Y + Rectangle.Height) - 12;
            origin = new Vector2(_font.MeasureString(SubHeader).X / 2, _font.MeasureString(SubHeader).Y / 2);
            spriteBatch.DrawString(_font, SubHeader, new Vector2(x, y), Color.Black, 0, origin, new Vector2(0.7f,0.7f), SpriteEffects.None, 1);

            // draw TileHeader
            y = Rectangle.Y + 32;
            origin = new Vector2(_font.MeasureString(TileHeader).X / 2, _font.MeasureString(TileHeader).Y / 2);
            spriteBatch.DrawString(_font, TileHeader, new Vector2(x, y), Color.Black, 0, origin, Scale, SpriteEffects.None, 1);
            #endregion

            if(CurrentCell != null)
            {
                //_currentTile.Draw(gameTime, spriteBatch);
                //_currentTile.DrawInner(gameTime, spriteBatch);
                //_currentTile.DrawNPC(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            // update cell properties
            if(NextCell != null)
            {
                CurrentCell = NextCell;
                DataChange();
                NextCell = null;
            }

            #region CHECK FOR MOUSE UPDATES
            // check mouse updates
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;
            }
            #endregion
        }

        public void DataChange()
        {
            TileHeader = $"Current Tile: {CurrentCell.TileData.TileIndex}";
            TileData t = CurrentCell.TileData;

            #region SET TILE BASED ON PROPERTIES
            if (t.ContainsNPC.Equals(true))
            {
                _currentTile = new Tile(_content.GetTileTexture(t.TextureIndex), t.Position, t.TileIndex);
                switch (t.NpcIndex)
                {
                    case 0:
                        _currentTile.TileNPC = new NPC(_content, 1, 2, t.TileIndex, new Tile[50, 50])
                        {
                            CurrentTile = _currentTile
                        };
                        break;
                    case 1:
                        _currentTile.TileNPC = new Vendor(_content, 1, 3, t.TileIndex, new Tile[50, 50])
                        {
                            CurrentTile = _currentTile
                        };
                        break;
                }
            }
            else if (t.ContainsInner.Equals(true))
            {
                _currentTile = new Tile(_content.GetTileTexture(t.TextureIndex), t.Position, t.TileIndex, _content.GetTileTexture(t.InnerTextureIndex));
            }
            else
            {
                _currentTile = new Tile(_content.GetTileTexture(t.TextureIndex), t.Position, t.TileIndex);
            }
            _currentTile.Position = new Vector2((Rectangle.X + (Rectangle.Width / 2)), Rectangle.Y + 64);
            #endregion
        }
    }
}
