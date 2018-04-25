using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Comora;

using Farm_Prototype.Content;

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
        
        private GameContent _content { get; set; }
        public GameContent Content
        {
            get { return _content; }
            set { _content = value; }
        }

        private SpriteFont font;

        public Map(Tile[,] tiles_, int width_, int height_, int tx_, int ty_, GameContent content_)
        {
            Tiles = tiles_;
            width = width_;
            height = height_;
            tw = tx_;
            th = ty_;
            Content = content_;
            font = Content.GetFont(1);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, Camera camera)
        {
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    Tiles[x, y].Update(gameTime, keyboardState, camera);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Player player_)
        {
            // handle management of the currently faced tile
            Tile facedTile = player_.CurrentlyFacedTile();

            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    if (Tiles[x, y].TileIndex.Equals(facedTile.TileIndex))
                    {
                        if(Tiles[x, y].TileNPC != null)
                        {
                            Tiles[x, y].TileNPC.IsHovered = true;
                        }
                    }
                    else
                    {
                        if (Tiles[x, y].TileNPC != null)
                        {
                            Tiles[x, y].TileNPC.IsHovered = false;
                        }
                    }
                }
            }

            // draw each tile and sort based on depth
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

            // draw player cursor
            player_.DrawCursor(gameTime, spriteBatch);

            /*
            for (int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    string message = "X: " + Tiles[x,y].TileIndex.X + ", Y: " + Tiles[x, y].TileIndex.Y;
                    Vector2 textMiddlePoint = font.MeasureString(message) / 2;
                    spriteBatch.DrawString(font, message, Tiles[x, y].Position + new Vector2(32, 38), Color.Black, 0, textMiddlePoint, 0.5f, SpriteEffects.None, 0.5f);
                }
            }
            */
            
        }
    }
}
