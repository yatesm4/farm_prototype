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
    public class Player
    {
        private Animation idleAnimation;
        private Animation walkAnimation;

        private Animation southAnimation;
        private Animation northAnimation;
        private Animation westAnimation;
        private Animation eastAnimation;

        private AnimationPlayer sprite;

        public Vector2 position { get; set; }
        public Vector2 moveDirection { get; set; }
        public Vector2 scale { get; set; } = new Vector2(1, 1);

        private Vector2 movement;
        private Vector2 velocity;

        private const float MoveAcceleration = 8000.0f;
        private const float MaxMoveSpeed = 500.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        // Constants for controlling vertical movement
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.14f;

        // Input configuration
        private const float MoveStickScale = 1.0f;
        private const float AccelerometerScale = 1.5f;

        private Rectangle localBounds;

        public Player(Microsoft.Xna.Framework.Content.ContentManager Content, Vector2 _position)
        {
            LoadContent(Content);
            Reset(_position);
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {

            southAnimation = new Animation(Content.Load<Texture2D>("Sprites/Characters/01_South"), 0.1f, true);
            northAnimation = new Animation(Content.Load<Texture2D>("Sprites/Characters/01_North"), 0.1f, true);
            westAnimation = new Animation(Content.Load<Texture2D>("Sprites/Characters/01_West"), 0.1f, true);
            eastAnimation = new Animation(Content.Load<Texture2D>("Sprites/Characters/01_East"), 0.1f, true);

            // Calculate bounds within texture size.            
            int width = (int)(southAnimation.FrameWidth * 0.4);
            int left = (southAnimation.FrameWidth - width) / 2;
            int height = (int)(southAnimation.FrameWidth * 0.8);
            int top = southAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);
        }

        public void Reset(Vector2 reset_position)
        {
            position = reset_position;
            velocity = Vector2.Zero;
            sprite.PlayAnimation(southAnimation);
        }

        public void Update(
            GameTime gameTime,
            KeyboardState keyboardState)
        {
            GetInput(keyboardState);

            ApplyPhysics(gameTime);
        }

        private void GetInput(KeyboardState keyboardState)
        {
            movement = new Vector2(0, 0);

            if(keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.D))
            {
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    movement.X = -1.0f;
                    sprite.PlayAnimation(westAnimation);
                }
                else if (keyboardState.IsKeyDown(Keys.D))
                {
                    movement.X = 1.0f;
                    sprite.PlayAnimation(eastAnimation);
                }

                if (keyboardState.IsKeyDown(Keys.W))
                {
                    movement.Y = -1.0f;
                    sprite.PlayAnimation(northAnimation);
                }
                else if (keyboardState.IsKeyDown(Keys.S))
                {
                    movement.Y = 1.0f;
                    sprite.PlayAnimation(southAnimation);
                }
                sprite.Animation.IsLooping = true;
            } else
            {
                sprite.Animation.IsLooping = false;
            }
        }

        public void ApplyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = position;

            velocity.X += movement.X * MoveAcceleration * elapsed;
            velocity.Y += movement.Y * MoveAcceleration * elapsed;

            velocity.X *= GroundDragFactor;
            velocity.Y *= GroundDragFactor;

            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);
            velocity.Y = MathHelper.Clamp(velocity.Y, -MaxMoveSpeed, MaxMoveSpeed);

            position += velocity * elapsed;
            position = new Vector2((float)Math.Round(position.X), (float)Math.Round(position.Y));

            //HandleCollisions();

            // If the collision stopped us from moving, reset the velocity to zero.
            if (position.X == previousPosition.X)
                velocity.X = 0;

            if (position.Y == previousPosition.Y)
                velocity.Y = 0;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.Draw(gameTime, spriteBatch, position, SpriteEffects.None);
        }
    }
}
