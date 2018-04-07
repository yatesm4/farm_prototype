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
    public class Plant
    {
        private Animation growthAnimation;
        private AnimationPlayer sprite;

        public Vector2 position;
        public int depth
        {
            get { return (int)Math.Round(position.Y * -1); }
        }
        public Vector2 scale { get; set; } = new Vector2(1, 1);

        private Rectangle localBounds;

        public Plant(Microsoft.Xna.Framework.Content.ContentManager Content, Vector2 _position)
        {
            LoadContent(Content);
            Reset(_position);
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            growthAnimation = new Animation(Content.Load<Texture2D>("Sprites/Plants/plant_basicweed"), 20f, false);

            int width = (int)(growthAnimation.FrameWidth * 0.4);
            int left = (growthAnimation.FrameWidth - width) / 2;
            int height = (int)(growthAnimation.FrameWidth * 0.8);
            int top = growthAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);
        }

        public void Reset(Vector2 reset_position)
        {
            position = reset_position;
            sprite.PlayAnimation(growthAnimation);
        }

        public void Update(GameTime gameTime)
        {
            // do something here
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, position, SpriteEffects.None);
        }
    }
}
