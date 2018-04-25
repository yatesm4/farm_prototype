using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Comora;

namespace Farm_Prototype.Objects
{
    public class TileData
    {
        public int TextureIndex { get; set; } = 0;
        public Vector2 TileIndex { get; set; } = new Vector2(0, 0);
        public Vector2 Position { get; set; } = new Vector2(0, 0);

        public bool ContainsInner { get; set; } = false;
        public int InnerTextureIndex { get; set; } = 0;

        public bool ContainsNPC { get; set; } = false;
        public int NpcIndex { get; set; } = 0;
    }

    public class Tile
    {
        private TileData _tileData { get; set; }
        public TileData TileData
        {
            get { return _tileData; }
            set { _tileData = value; }
        }

        private Texture2D _outlineTexture { get; set; }
        public Texture2D OutlineTexture
        {
            get { return _outlineTexture; }
            set { _outlineTexture = value; }
        }
        public bool ShowOutline { get; set; } = false;
        public int OutlineCooldown { get; set; } = 0;
        private bool _isHovered { get; set; } = false;

        private Texture2D _texture { get; set; }
        private Texture2D _innerTexture { get; set; } = null;
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }
        public Texture2D InnerTexture
        {
            get { return _innerTexture; }
            set { _innerTexture = value; }
        }
        public bool DrawInnerDelayed { get; set; } = false;
        private Vector2 _tileIndex { get; set; }
        public Vector2 TileIndex
        {
            get { return _tileIndex; }
            set { _tileIndex = value; }
        }

        private NPC tileNPC { get; set; }
        public NPC TileNPC
        {
            get { return tileNPC; }
            set { tileNPC = value; }
        }

        private Plant _innerPlant { get; set; } = null;
        public Plant InnerPlant
        {
            get { return _innerPlant; }
            set { _innerPlant = value; }
        }
        private bool _isDecoration { get; set; } = false;
        public bool IsDecoration
        {
            get { return _isDecoration; }
            set { _isDecoration = value; }
        }


        private Vector2 _position { get; set; }
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public Vector2 CenterPoint
        {
            get { return Position + new Vector2(32, 44); }
        }
        public Vector2 Scale { get; set; } = new Vector2(1, 1);
        public bool DrawDebug { get; set; } = false;

        public Tile(Texture2D texture_, Vector2 position_, Vector2 tileIndex_)
        {
            Texture = texture_;
            Position = position_;
            TileIndex = tileIndex_;
        }
        public Tile(Texture2D texture_, Vector2 position_, Vector2 tileIndex_, Texture2D innerTexture_)
        {
            Texture = texture_;
            InnerTexture = innerTexture_;
            Position = position_;
            TileIndex = tileIndex_;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, Camera camera)
        {
            // DebugUpdate(gameTime, keyboardState, camera);
        }

        public void DebugUpdate(GameTime gameTime, KeyboardState keyboardState, Camera camera)
        {
            // debug hovering only works when the camera zoom is set to 1.0f

            var ms = Mouse.GetState();
            var b = camera.GetBounds();
            var mr = new Rectangle(b.X + ms.Position.X, b.Y + 142 + ms.Position.Y, 1, 1);

            if (mr.Intersects(new Rectangle((int)Position.X + 16, (int)Position.Y + 16, Texture.Width / 2, Texture.Height / 2)))
            {
                // mouse is hovering
                Console.WriteLine($"Tile {TileIndex} is currently hovered");
                ShowOutline = true;
                OutlineCooldown = 25;
            }

            if (TileNPC != null)
            {
                TileNPC.Update(gameTime, keyboardState);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position: Position, scale: Scale, layerDepth: 0.4f);
            if ((ShowOutline == true & OutlineCooldown > 0))
            {
                Console.WriteLine($"Drawing outline for tile {TileIndex}");
                spriteBatch.Draw(OutlineTexture, position: Position, scale: Scale, layerDepth: 0.4f);
                OutlineCooldown--;
            } else if (OutlineCooldown == 0)
            {
                ShowOutline = false;
            }
        }

        public void DrawInner(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(InnerTexture != null)
            {
                spriteBatch.Draw(InnerTexture, position: Position, scale: Scale, layerDepth: 0.4f);
            }
        }

        public void DrawNPC(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(TileNPC != null)
            {
                TileNPC.Draw(gameTime, spriteBatch);
            }
        }
    }
}
