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
    public class NPC
    {

        Random rnd = new Random();

        private Animation bodyAnimation;
        private Animation headAnimation;
        private AnimationPlayer bodySprite;
        private AnimationPlayer headSprite;

        Vector2 tileIndex;
        Tile[,] tileArray;
        Tile currentTile;

        private Rectangle localBounds;

        private Vector2 position { get; set; }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public NPC(Microsoft.Xna.Framework.Content.ContentManager Content, int npcIndex, int headIndex, Vector2 tileIndex, Tile[,] tiles)
        {
            tileArray = tiles;
            currentTile = tileArray[(int)tileIndex.X, (int)tileIndex.Y];
            LoadContent(Content, npcIndex, headIndex);
            Reset(currentTile.CenterPoint);
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content, int npcIndex, int headIndex)
        {
            string npc_path = "Sprites/Characters/NPCs/0" + npcIndex;
            string head_path = "Sprites/Characters/Head/0" + headIndex;
            bodyAnimation = new Animation(Content.Load<Texture2D>(npc_path), 0.15f, true);
            headAnimation = new Animation(Content.Load<Texture2D>(head_path), 0.1f, false);
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
        }
    }
}
