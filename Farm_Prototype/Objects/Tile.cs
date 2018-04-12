using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Farm_Prototype.Objects
{
    public class Tile
    {

        private Texture2D _outlineTexture { get; set; }
        public Texture2D OutlineTexture
        {
            get { return _outlineTexture; }
            set { _outlineTexture = value; }
        }
        public bool ShowOutline { get; set; } = false;
        public int OutlineCooldown { get; set; } = 0;

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

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if(TileNPC != null)
            {
                TileNPC.Update(gameTime, keyboardState);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position: Position, scale: Scale, layerDepth: 0.4f);
            if (ShowOutline == true & OutlineCooldown > 0)
            {
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
