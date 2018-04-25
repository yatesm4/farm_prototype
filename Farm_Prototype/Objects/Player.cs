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

using Farm_Prototype.Content;

namespace Farm_Prototype.Objects
{
    public class Player
    {
        #region SPRITE / DRAW PROPS
        // sprite / draw properties
        public AnimationPlayer selectCursor;

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

        SpriteFont font;
        #endregion

        #region SOUND PROPS
        // sound properties
        SoundEffect footstep;
        int footstepCooldown;
        #endregion

        #region TILE PROPS
        // tile properties
        Tile[,] gameTiles;
        public Tile currentTile { get; set; }
        public Tile destTile { get; set; }
        public Vector2 directionFacing { get; set; } = new Vector2(1, 0);
        #endregion

        #region MOVEMENT PROPS
        // movement properties
        public Vector2 position { get; set; }
        public int depth
        {
            get { return (int)Math.Round(position.Y * -1); }
        }

        bool isMoving = false;
        int movementCooldown = 0;

        private Vector2 movement;
        private Vector2 velocity;
        private float distance;
        private Vector2 direction;

        private Rectangle localBounds;
        #endregion

        #region INVENTORY PROPS
        // inventory properties
        private PlayerInventory playerInventory { get; set; }
        #endregion

        #region INTERACTION PROPS
        private int interactionCooldown = 0;
        #endregion

        #region CONSTRUCTOR
        public Player(GameContent content, Vector2 _position, Vector2 tileIndex, Tile[,] tiles)
        {
            font = content.GetFont(1);
            gameTiles = tiles;
            currentTile = gameTiles[(int)tileIndex.X, (int)tileIndex.Y];
            LoadContent(content);
            Reset(currentTile.CenterPoint);

            // load inventory
            playerInventory = new PlayerInventory(50, 1000);
        }
        #endregion

        #region LOAD
        public void LoadContent(GameContent Content)
        {
            selectCursor.PlayAnimation(new Animation(Content.GetUiTexture(5), 0.1f, true));
            /***
             * technically you could have the sprite loaded based on custom input, if you have more than 1 character made
             * and you could load the sprites dynamically like this
             * int sprite_index = 1;
             * string spriteSouthWest = "0" + sprite_index + "_SouthWest";
             * southWestBodyAnimation = new Animation(Content.Load<Texture2D>("Sprites/Characters/Body/"+spriteSouthWest), 0.13f, true);
             ***/
            // Load the spritesheet for each direction
            southWestBodyAnimation = new Animation(Content.GetBodyTexture(1), 0.15f, true);
            southEastBodyAnimation = new Animation(Content.GetBodyTexture(2), 0.15f, true);
            northWestBodyAnimation = new Animation(Content.GetBodyTexture(3), 0.15f, true);
            northEastBodyAnimation = new Animation(Content.GetBodyTexture(4), 0.15f, true);
            // load the head spritesheet
            headAnimation = new Animation(Content.GetHeadTexture(2), 0.1f, false);
            // set the animation to be still for the head, so we can load each frame in it individually
            headAnimation.IsStill = true;

            footstep = Content.GetSoundEffect(1);

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
            headSprite.PlayAnimation(headAnimation);
            headSprite.FrameIndex = 1;
            bodySprite.PlayAnimation(southEastBodyAnimation);
        }
        #endregion

        #region UPDATE
        public void Update(
            GameTime gameTime,
            KeyboardState keyboardState)
        {
            // update player object
            if(isMoving == false && movementCooldown <= 0)
            {
                GetMovementInput(keyboardState);
                if(isMoving == false)
                {
                    GetInteractionInput(keyboardState);
                }
            } else if (movementCooldown > 0)
            {
                movementCooldown--;
            }
            if(isMoving == true)
            {
                // handle footstep sounds
                // if the footstep cooldown is back to 0
                if (footstepCooldown <= 0)
                {
                    // play a footstep sound and reset the cooldown to 20 update cycles
                    footstep.Play();
                    footstepCooldown += 20;
                }
                // if the footstep cooldown is above 0
                if (footstepCooldown > 0)
                {
                    // decrement the cooldown
                    footstepCooldown--;
                }

                ApplyPhysics(gameTime);
            }

        }
        #endregion

        #region HANDLE INPUT
        private void GetInteractionInput(KeyboardState keyboardState)
        {
            if(interactionCooldown <= 0)
            {
                if (keyboardState.IsKeyDown(Keys.E))
                {
                    if (gameTiles[(int)currentTile.TileIndex.X + (int)directionFacing.X, (int)currentTile.TileIndex.Y + (int)directionFacing.Y].TileNPC != null)
                    {
                        HandleNPCInteraction(gameTiles[(int)currentTile.TileIndex.X + (int)directionFacing.X, (int)currentTile.TileIndex.Y + (int)directionFacing.Y]);
                        // the next tile in the direction the player is facing contains an NPC
                    }
                }
            } else
            {
                interactionCooldown--;
            }
            
        }

        private void HandleNPCInteraction(Tile npcTile)
        {
            if(npcTile.TileNPC is Vendor)
            {
                Console.WriteLine("Interacted with a Vendor NPC at tile: {0}", npcTile.TileIndex.X, npcTile.TileIndex.Y);
            }
            else
            {
                Console.WriteLine("Interacted with an NPC at tile: {0}", npcTile.TileIndex.X, npcTile.TileIndex.Y);
            }
            interactionCooldown += 25;
        }

        private void GetMovementInput(KeyboardState keyboardState)
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
                    bodySprite.PlayAnimation(northWestBodyAnimation);
                    headSprite.FrameIndex = 3;
                    headFront = false;
                }
                else if (keyboardState.IsKeyDown(Keys.D))
                {
                    // Move Right
                    movement = new Vector2(1, 0);
                    bodySprite.PlayAnimation(southEastBodyAnimation);
                    headSprite.FrameIndex = 1;
                }
                else if (keyboardState.IsKeyDown(Keys.W))
                {
                    // Move Up
                    movement = new Vector2(0, -1);
                    bodySprite.PlayAnimation(northEastBodyAnimation);
                    headSprite.FrameIndex = 0;
                    // Make sure the head renders behind the body sprite 
                    headFront = false;
                }
                else if (keyboardState.IsKeyDown(Keys.S))
                {
                    // Move Down
                    movement = new Vector2(0, 1);
                    bodySprite.PlayAnimation(southWestBodyAnimation);
                    headSprite.FrameIndex = 2;

                }

                directionFacing = movement;

                if (keyboardState.IsKeyDown(Keys.LeftShift) && keyboardState.IsKeyDown(Keys.RightShift))
                {
                    movement *= 3;
                } else if (keyboardState.IsKeyDown(Keys.LeftShift))
                {
                    movement *= 2;
                }

                // Set the animation loop to true
                bodySprite.Animation.IsLooping = true;

                HandleNextTile();
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
        #endregion

        #region DRAW
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

        public void DrawCursor(GameTime gameTime, SpriteBatch spriteBatch)
        {
            try
            {
                if (directionFacing != Vector2.Zero && isMoving == false)
                {
                    Tile selectTile = gameTiles[(int)currentTile.TileIndex.X + (int)directionFacing.X, (int)currentTile.TileIndex.Y + (int)directionFacing.Y];
                    selectCursor.Draw(gameTime, spriteBatch, selectTile.CenterPoint - new Vector2(0, 20), SpriteEffects.None);
                }
            } catch (Exception e)
            {
                // cant place select cursor over tile
            }
        }
        #endregion

        #region HANDLE TILES
        public Tile CurrentlyFacedTile()
        {
            Tile t = gameTiles[(int)currentTile.TileIndex.X + (int)directionFacing.X, (int)currentTile.TileIndex.Y + (int)directionFacing.Y];
            return t;
        }
        public void HandleNextTile()
        {
            int destX, destY;
            destX = (int)currentTile.TileIndex.X + (int)movement.X;
            destY = (int)currentTile.TileIndex.Y + (int)movement.Y;

            if (destX >= 49 || destY >= 49 || destX <= 0 || destY <= 0)
            {
                // movement is outside of map bounds
                return;
            }

            destTile = gameTiles[(destX), (destY)];

            if (destTile.TileNPC != null || destTile.InnerTexture != null)
            {

                destTile.ShowOutline = true;
                destTile.OutlineCooldown = 25;
                destTile = null;
                return;
            }

            distance = Vector2.Distance(currentTile.CenterPoint, destTile.CenterPoint);
            direction = Vector2.Normalize(destTile.CenterPoint - currentTile.CenterPoint);
            destTile.DrawDebug = true;
            isMoving = true;
            movementCooldown += 25;
        }
        #endregion

        #region PHYSICS
        public void ApplyPhysics(GameTime gameTime)
        {
            position += direction * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            float current_distance = Vector2.Distance(position, destTile.CenterPoint);
            if(current_distance < 1)
            {
                position = destTile.CenterPoint;
                currentTile = destTile;
                isMoving = false;
                currentTile.DrawDebug = false;
            }
        }
        #endregion

        #region DEBUG
        void DebugDirection()
        {
            string dir = "SOUTHWEST";
            switch (directionFacing.X)
            {
                case -1:
                    dir = "NORTHWEST";
                    break;
                case 1:
                    dir = "SOUTHEAST";
                    break;
            }
            switch (directionFacing.Y)
            {
                case -1:
                    dir = "NORTHEAST";
                    break;
                case 1:
                    dir = "SOUTHWEST";
                    break;
            }
            Console.WriteLine("Direction facing {0}", dir);
        }
        #endregion

    }
}
