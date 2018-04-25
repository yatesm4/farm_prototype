using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Farm_Prototype.Content;

namespace Farm_Prototype.Objects
{
    public class SpeechBubble
    {
        private Texture2D _bubbleSprite;
        public Texture2D BubbleSprite
        {
            get { return _bubbleSprite; }
            set { _bubbleSprite = value; }
        }

        private GameContent _content;
        public GameContent Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private int _iconIndex = 3;
        public int IconIndex
        {
            get { return _iconIndex; }
            set { _iconIndex = value; }
        }

        public Texture2D BubbleIcon
        {
            get { return Content.GetUiTexture(IconIndex); }
        }

        private Vector2 _position = new Vector2(0, 0);
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public SpeechBubble(GameContent content_, int iconIndex_, Vector2 position_)
        {
            Content = content_;
            BubbleSprite = Content.GetUiTexture(2);
            IconIndex = iconIndex_;
            Position = position_;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BubbleSprite, position: Position);
            spriteBatch.Draw(BubbleIcon, position: Position);
        }


    }
}
