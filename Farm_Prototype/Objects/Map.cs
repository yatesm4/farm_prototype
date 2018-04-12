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
    public class Map
    {
        private Tile[,] _tiles;
        public Tile[,] Tiles
        {
            get { return _tiles; }
            set { _tiles = value; }
        }

        int width, height, tw, th;
        Texture2D[] textures;

        SpriteFont font;

        public Map(Tile[,] tiles_, int width_, int height_, int tx_, int ty_, Texture2D[] textures_, SpriteFont font_)
        {
            Tiles = tiles_;
            width = width_;
            height = height_;
            tw = tx_;
            th = ty_;
            textures = textures_;
            font = font_;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    Tiles[x, y].Update(gameTime, keyboardState);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Player player_)
        {
            List<Tile> depthArr = new List<Tile>();

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    Tiles[x, y].DrawInnerDelayed = false;


                    if (Tiles[x, y].Position.Y + 48 > player_.position.Y + 16)
                    {
                        /**
                     * IF THE TILE CONTAINS AN INNER TILE AND THE PLAYER'S Y POSITION IS LESS THAN THE Y OF THE TILE
                     * ADD THE TILE TO A LATTER DEPTH LIST
                     **/

                        depthArr.Add(Tiles[x, y]);
                    }
                    else
                    {
                        /**
                     * ELSE, RENDER THE TILE NOW (BEFORE THE PLAYER)
                     **/

                        Tiles[x, y].Draw(gameTime, spriteBatch);
                        Tiles[x, y].DrawNPC(gameTime, spriteBatch);

                        if (Tiles[x, y].Position.Y + 64 > player_.position.Y + 16)
                        {
                            depthArr.Add(Tiles[x, y]);
                            Tiles[x, y].DrawInnerDelayed = true;
                        }
                        else
                        {
                            Tiles[x, y].DrawInner(gameTime, spriteBatch);
                        }
                    }
                }
            }


            player_.Draw(gameTime, spriteBatch);


            foreach (Tile t in depthArr)
            {
                if (t.DrawInnerDelayed == true)
                {
                    t.DrawInner(gameTime, spriteBatch);
                }
                else
                {
                    t.Draw(gameTime, spriteBatch);
                    t.DrawNPC(gameTime, spriteBatch);
                    t.DrawInner(gameTime, spriteBatch);
                }

            }

            /*
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    Tiles[x, y].DrawInnerDelayed = false;
                    string message = "X: " + x + ", Y: " + y;
                    Vector2 textMiddlePoint = font.MeasureString(message) / 2;
                    spriteBatch.DrawString(font, message, Tiles[x, y].Position + new Vector2(32, 32), Color.Black, 0, textMiddlePoint, 0.3f, SpriteEffects.None, 0.5f);
                }
            }
            */
        }
    }
}
