using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Farm_Prototype.Content;

namespace Farm_Prototype.Objects
{
    public class NPC
    {
        // rng generator
        Random rnd = new Random();

        // sprite settings
        Animation bodyAnimation;
        Animation headAnimation;
        AnimationPlayer bodySprite;
        AnimationPlayer headSprite;

        // tile settings
        Vector2 tileIndex;
        Tile[,] tileArray;
        Tile currentTile { get; set; }
        public Tile CurrentTile
        {
            get { return currentTile; }
            set { currentTile = value; }
        }

        // local bounds
        private Rectangle localBounds;

        // speech bubble settings
        public SpeechBubble speechBubble;
        public bool IsHovered = false;

        private Vector2 position { get; set; }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public NPC(GameContent Content, int npcIndex, int headIndex, Vector2 tileIndex, Tile[,] tiles)
        {
            // set the npc's tile
            tileArray = tiles;
            try
            {
                currentTile = tileArray[(int)tileIndex.X, (int)tileIndex.Y];
            } catch (Exception e)
            {
                Console.WriteLine("Couldn't set current tile of npc");
            }

            // load npc's content
            LoadContent(Content, npcIndex, headIndex);
            Reset(currentTile != null ? currentTile.CenterPoint : new Vector2(0,0));

            // load speech bubble
            speechBubble = new SpeechBubble(Content, 3, Position - new Vector2(-8, 30));
        }

        public void LoadContent(GameContent Content, int npcIndex, int headIndex)
        {
            bodyAnimation = new Animation(Content.GetNpcTexture(npcIndex), 0.15f, true);
            headAnimation = new Animation(Content.GetHeadTexture(headIndex), 0.1f, false);
            headAnimation.IsStill = true;

            // Calculate bounds within texture size.            
            int width = (int)(bodyAnimation.FrameWidth * 0.4);
            int left = (bodyAnimation.FrameWidth - width) / 2;
            int height = (int)(bodyAnimation.FrameWidth * 0.8);
            int top = bodyAnimation.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);
        }

        public void Reset(Vector2 reset_position)
        {
            Position = reset_position;
            headSprite.PlayAnimation(headAnimation);
            headSprite.FrameIndex = 2;
            bodySprite.PlayAnimation(bodyAnimation);
        }

        public void Update(
            GameTime gameTime,
            KeyboardState keyboardState)
        {
            // update npc object
            if(rnd.Next(1,1000) < 50)
            {
                if(headSprite.FrameIndex == 1)
                {
                    headSprite.FrameIndex = 2;
                } else
                {
                    headSprite.FrameIndex = 1;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            bodySprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None);
            headSprite.Draw(gameTime, spriteBatch, Position - new Vector2(0, 11), SpriteEffects.None);
            if(IsHovered == true)
            {
                speechBubble.Draw(gameTime, spriteBatch);
            }
            //Console.WriteLine($"NPC Drawn");
        }
    }
}
