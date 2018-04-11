using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Farm_Prototype.Objects
{
    public class Player
    {

        private Animation idleAnimation;
        private Animation walkAnimation;

        private Animation headAnimation;
        private Animation southWestBodyAnimation;
        private Animation northWestBodyAnimation;
        private Animation southEastBodyAnimation;
        private Animation northEastBodyAnimation;

        private AnimationPlayer bodySprite;
        private AnimationPlayer headSprite;
        bool headFront = true;
        bool lastInputWasLeft = true;

        SoundEffect footstep;
        int footstepCooldown;

        public Vector2 position { get; set; }
        public int depth
        {
            get { return (int)Math.Round(position.Y * -1); }
        }

        Tile[,] gameTiles;
        public Tile currentTile { get; set; }
        public Tile destTile { get; set; }

        // movement properties
        bool isMoving = false;
        int movementCooldown = 0;

        public Vector2 moveDirection { get; set; }
        public Vector2 scale { get; set; } = new Vector2(1, 1);

        private Vector2 movement;
        private Vector2 velocity;
        private float distance;
        private Vector2 direction;

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

        public Player(Microsoft.Xna.Framework.Content.ContentManager Content, Vector2 _position, Vector2 tileIndex, Tile[,] tiles)
        {
            gameTiles = tiles;
            currentTile = gameTiles[(int)tileIndex.X, (int)tileIndex.Y];
            LoadContent(Content);
            Reset(currentTile.centerPoint);
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            /***
             * technically you could have the sprite loaded based on custom input, if you have more than 1 character made
             * and you could load the sprites dynamically like this
             * int sprite_index = 1;
             * string spriteSouthWest = "0" + sprite_index + "_SouthWest";
             * southWestBodyAnimation = new Animation(Content.Load<Texture2D>("Sprites/Characters/Body/"+spriteSouthWest), 0.13f, true);
             ***/
            // Load the spritesheet for each direction
            southWestBodyAnimation = new Animation(Content.Load<Texture2D>("Sprites/Characters/Body/01_SouthWest"), 0.13f, true);
            southEastBodyAnimation = new Animation(Content.Load<Texture2D>("Sprites/Characters/Body/01_SouthEast"), 0.13f, true);
            northWestBodyAnimation = new Animation(Content.Load<Texture2D>("Sprites/Characters/Body/01_NorthWest"), 0.13f, true);
            northEastBodyAnimation = new Animation(Content.Load<Texture2D>("Sprites/Characters/Body/01_NorthEast"), 0.13f, true);
            // load the head spritesheet
            headAnimation = new Animation(Content.Load<Texture2D>("Sprites/Characters/Head/01"), 0.1f, false);
            // set the animation to be still for the head, so we can load each frame in it individually
            headAnimation.IsStill = true;

            footstep = Content.Load<SoundEffect>("Sounds/Effects/footstep");

            // Calculate bounds within texture size.            
            int width = (int)(southWestBodyAnimation.FrameWidth * 0.4);
            int left = (southWestBodyAnimation.FrameWidth - width) / 2;
            int height = (int)(southWestBodyAnimation.FrameWidth * 0.8);
            int top = southWestBodyAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);
        }

        public void Reset(Vector2 reset_position)
        {
            position = reset_position;
            velocity = Vector2.Zero;
            bodySprite.PlayAnimation(southWestBodyAnimation);
            headSprite.PlayAnimation(headAnimation);
        }

        public void Update(
            GameTime gameTime,
            KeyboardState keyboardState)
        {
            // update player object
            if(isMoving == false && movementCooldown <= 0)
            {
                GetInput(keyboardState);
            } else if (movementCooldown > 0)
            {
                movementCooldown--;
            }
            if(isMoving == true)
            {
                ApplyPhysics(gameTime);

            }
        }

        private void GetInput(KeyboardState keyboardState)
        {
            // reset the movement input
            movement = new Vector2(0, 0);

            // if there is any keyboard movement
            if((keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.D)) && isMoving == false && movementCooldown <= 0)
            {
                // set the head to be in front of the body
                headFront = true;

                // make sure the animation isnt still
                bodySprite.Animation.IsStill = false;

                if (keyboardState.IsKeyDown(Keys.A))
                {
                    // Move Left
                    movement = new Vector2(-1, 0);

                    bodySprite.PlayAnimation(southWestBodyAnimation);
                    headSprite.FrameIndex = 2;
                    lastInputWasLeft = true;
                }
                else if (keyboardState.IsKeyDown(Keys.D))
                {
                    // Move Right
                    movement = new Vector2(1, 0);

                    bodySprite.PlayAnimation(southEastBodyAnimation);
                    headSprite.FrameIndex = 1;
                    lastInputWasLeft = false;
                }
                else if (keyboardState.IsKeyDown(Keys.W))
                {
                    // Move Up
                    movement = new Vector2(0, -1);

                    if (lastInputWasLeft == true)
                    {
                        bodySprite.PlayAnimation(northWestBodyAnimation);
                        headSprite.FrameIndex = 3;
                    }
                    else
                    {
                        bodySprite.PlayAnimation(northEastBodyAnimation);
                        headSprite.FrameIndex = 0;
                    }
                    // Make sure the head renders behind the body sprite 
                    headFront = false;
                }
                else if (keyboardState.IsKeyDown(Keys.S))
                {
                    // Move Down
                    movement = new Vector2(0, 1);

                    if (lastInputWasLeft == true)
                    {
                        bodySprite.PlayAnimation(southWestBodyAnimation);
                        headSprite.FrameIndex = 2;
                    }
                    else
                    {
                        bodySprite.PlayAnimation(southEastBodyAnimation);
                        headSprite.FrameIndex = 1;
                    }

                }

                // Set the animation loop to true
                bodySprite.Animation.IsLooping = true;

                // handle footstep sounds
                // if the footstep cooldown is back to 0
                if(footstepCooldown <= 0)
                {
                    // play a footstep sound and reset the cooldown to 20 update cycles
                    footstep.Play();
                    footstepCooldown += 20;
                }
                // if the footstep cooldown is above 0
                if(footstepCooldown > 0)
                {
                    // decrement the cooldown
                    footstepCooldown--;
                }

                int destX, destY;
                destX = (int)currentTile.tileIndex.X + (int)movement.X;
                destY = (int)currentTile.tileIndex.Y + (int)movement.Y;
                destTile = gameTiles[(destX), (destY)];

                distance = Vector2.Distance(currentTile.centerPoint, destTile.centerPoint);
                direction = Vector2.Normalize(destTile.centerPoint - currentTile.centerPoint);

                Console.WriteLine("Current Tile: {0}, Destination Tile: {1}", currentTile.tileIndex.ToString(), destTile.tileIndex.ToString());
                Console.WriteLine("Distance: {0}, Direction: {1}", distance.ToString(), direction.ToString());

                destTile.drawDebug = true;

                isMoving = true;
                movementCooldown += 25;
            }
            else
            {
                // if there isnt any keyboard input
                if(bodySprite.Animation.IsLooping == true)
                {
                    // set the body sprites animation to still and reset its frameindex to 0, as well as turn of looping
                    bodySprite.Animation.IsStill = true;
                    bodySprite.FrameIndex = 0;
                    bodySprite.Animation.IsLooping = false;
                }
                isMoving = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(headFront == true)
            {
                bodySprite.Draw(gameTime, spriteBatch, position, SpriteEffects.None);
                headSprite.Draw(gameTime, spriteBatch, position - new Vector2(0, 11), SpriteEffects.None);
            } else
            {
                headSprite.Draw(gameTime, spriteBatch, position - new Vector2(0, 11), SpriteEffects.None);
                bodySprite.Draw(gameTime, spriteBatch, position, SpriteEffects.None);
            }
        }

        public void ApplyPhysics(GameTime gameTime)
        {
            position += direction * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            float current_distance = Vector2.Distance(position, destTile.centerPoint);
            Console.WriteLine("Current Distance: {0}", current_distance);
            if(current_distance < 1)
            {
                position = destTile.centerPoint;
                currentTile = destTile;
                isMoving = false;
                currentTile.drawDebug = false;
            }
        }

    }
}
