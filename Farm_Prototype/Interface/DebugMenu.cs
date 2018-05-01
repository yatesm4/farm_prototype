﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;

using Comora;

using Farm_Prototype.Content;
using Farm_Prototype.Objects;

namespace Farm_Prototype.Interface
{
    public class DebugMenu : Component
    {
        private GameContent _content { get; set; }
        private SpriteFont _font { get; set; }
        private Camera _camera { get; set; }

        private bool _isHovering { get; set; } = false;

        private MouseState _previousMouse;
        private MouseState _currentMouse;

        private Vector2 _displaySize { get; set; }

        public Texture2D Texture { get; set; }
        public Texture2D HoverTexture { get; set; }

        public Color DisplayColor { get; set; } = Color.DarkSlateGray;
        public Color[] DisplayColorData { get; set; }
        public Color HoverColor { get; set; } = Color.LightSlateGray;
        public Color[] HoverColorData { get; set; }

        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; } = new Vector2(1, 1);

        public List<SelectionCell> SelectionCells = new List<SelectionCell>();

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)_displaySize.X, (int)_displaySize.Y);
            }
        }

        public string Header = "Debug Menu";

        public DebugMenu(GraphicsDevice graphicsDevice_, GameContent content_)
        {
            int height_ = 48;
            int width_ = 780;

            Position = new Vector2(10,10);
            _displaySize = new Vector2(width_, height_);

            Console.WriteLine($"Debug Menu Created:>");
            Console.WriteLine($"Size: {_displaySize}");
            Console.WriteLine($"Posiiton: {Position}");

            SetColorData(graphicsDevice_);
            _content = content_;
            _font = _content.GetFont(1);

            LoadSelections(graphicsDevice_, content_);
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

        public void LoadSelections(GraphicsDevice graphicsDevice_, GameContent content_)
        {
            var pos = new Vector2(Position.X + 8, Position.Y + 8);
            int id = 1;
            foreach(ContentData<Texture2D> t in _content.TileTextures)
            {
                if (t.Id.Equals(1))
                    continue;
                SelectionCells.Add(new SelectionCell(graphicsDevice_, pos, t.Data, id));
                pos += new Vector2(48, 0);
                id++;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // draw here
            spriteBatch.Draw(Texture, Rectangle, Color.White);
            //Console.WriteLine($"Debug menu is visible");

            foreach(SelectionCell c in SelectionCells)
            {
                c.Draw(gameTime, spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            // update here
            foreach(SelectionCell c in SelectionCells)
            {
                c.Update(gameTime);
            }
        }
    }
}
