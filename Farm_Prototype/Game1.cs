using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;

using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Tweening;

using Comora;

using Farm_Prototype.Objects;

namespace Farm_Prototype
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Random rnd = new Random();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Camera camera;
        float cameraZoom = 2.0f;

        // Controller settings

        int scr_Width;
        int scr_Height;

        int tiles_Width;
        int tiles_Height;

        Vector2 position = new Vector2();
        Vector2 velocity = new Vector2(0, 0);

        // Sprites

        Texture2D spr_Man;
        Texture2D tile_Wood;

        Texture2D tile_Grass;
        Texture2D tile_Grass_Tree;
        Texture2D tile_Road;
        Texture2D tile_Sidewalk;

        Texture2D tile_Floor;
        Texture2D tile_Room;
        Texture2D tile_Room_01;
        Texture2D tile_Room_01_Floor;

        SpriteFont font;

        // Tiles

        List<Tile> tile_List = new List<Tile>();

        // Objects

        private KeyboardState keyboardState;

        Player player;
        List<Object> allObjectsList = new List<object>();
        List<Plant> plants = new List<Plant>();
        int planting_cooldown = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            scr_Width = graphics.GraphicsDevice.Viewport.Width;
            scr_Height = graphics.GraphicsDevice.Viewport.Height;

            tiles_Width = 1000 / 32;
            tiles_Height = 1000 / 32;

            System.Diagnostics.Debug.WriteLine("tile_Width = " + tiles_Width);
            System.Diagnostics.Debug.WriteLine("tile_Height = " + tiles_Height);

            position = new Vector2(scr_Width / 2, scr_Height / 2);

            camera = new Camera(this.graphics.GraphicsDevice);
            camera.Zoom = cameraZoom;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            tile_Wood = Content.Load<Texture2D>("tile_Wood");

            tile_Grass = Content.Load<Texture2D>("Sprites/Environment/Ground_Grass");
            tile_Grass_Tree = Content.Load<Texture2D>("Sprites/Environment/Ground_Tree");
            tile_Road = Content.Load<Texture2D>("Sprites/Environment/Ground_Road");
            tile_Sidewalk = Content.Load<Texture2D>("Sprites/Environment/Floor_Sidewalk");
            
            tile_Room = Content.Load<Texture2D>("Sprites/Environment/Structures/Room_Base");
            tile_Floor = Content.Load<Texture2D>("Sprites/Environment/Structures/Floor_Base");

            tile_Room_01 = Content.Load<Texture2D>("Sprites/Environment/Structures/Room_01");
            tile_Room_01_Floor = Content.Load<Texture2D>("Sprites/Environment/Structures/Room_01_Floor");

            font = Content.Load<SpriteFont>("Fonts/Font_01");

            LoadPlayer();

            LoadTiles();
        }

        private void LoadTiles()
        {
            for (var i = 0; i < tiles_Height; i++)
            {
                for (var j = 0; j < tiles_Width; j++)
                {
                    Vector2 pos = new Vector2((j * 64) + (-i * 32), (i * 64) - (i * 48));
                    System.Diagnostics.Debug.WriteLine("Tile Position: " + pos.ToString());
                    int rnd_int = rnd.Next(1, 100);
                    Tile _tile;
                    if(rnd_int > 30)
                    {
                        _tile = new Tile
                        {
                            texture = tile_Grass,
                            position = pos
                        };
                        tile_List.Add(_tile);
                    }
                    else
                    {
                        if(rnd_int < 15)
                        {
                            if(rnd_int < 6)
                            {
                                _tile = new Tile
                                {
                                    texture = tile_Room_01_Floor,
                                    position = pos
                                };
                                tile_List.Add(_tile);
                            } else
                            {
                                _tile = new Tile
                                {
                                    texture = tile_Room_01,
                                    position = pos
                                };
                                tile_List.Add(_tile);
                            }
                        } else
                        {
                            _tile = new Tile
                            {
                                texture = tile_Grass,
                                position = pos
                            };
                            tile_List.Add(_tile);

                            _tile = new Tile
                            {
                                texture = tile_Grass_Tree,
                                isDecoration = true,
                                position = pos
                            };
                            tile_List.Add(_tile);
                        }
                    }
                }
                tiles_Width++;
            }
            
            foreach(Tile t in tile_List)
            {
                if(t.texture == tile_Room_01_Floor || t.texture == tile_Room_01)
                {
                    try
                    {
                        tile_List[tile_List.IndexOf(t) + 10].texture = tile_Sidewalk;
                    } catch (Exception e)
                    {
                        // out of bounds
                    }
                }
                
            }

            /*

            tile_List[407].texture = tile_Room_01;
            tile_List[366].texture = tile_Room_01;
            tile_List[326].texture = tile_Room_01;
            tile_List[287].texture = tile_Room_01;
            tile_List[249].texture = tile_Room_01;

            Plant plant;
            plant = new Plant(Content, tile_List[407].position + new Vector2(32, 64));
            plants.Add(plant);
            tile_List[407].innerPlant = plant;

            plant = new Plant(Content, tile_List[366].position + new Vector2(32, 64));
            plants.Add(plant);
            tile_List[366].innerPlant = plant;

            plant = new Plant(Content, tile_List[326].position + new Vector2(32, 64));
            plants.Add(plant);
            tile_List[326].innerPlant = plant;

            plant = new Plant(Content, tile_List[287].position + new Vector2(32, 64));
            plants.Add(plant);
            tile_List[287].innerPlant = plant;

            plant = new Plant(Content, tile_List[249].position + new Vector2(32, 64));
            plants.Add(plant);
            tile_List[249].innerPlant = plant;


            tile_List[450].texture = tile_Room_01_Floor;
            tile_List[408].texture = tile_Room_01_Floor;
            tile_List[367].texture = tile_Room_01_Floor;
            tile_List[327].texture = tile_Room_01_Floor;
            tile_List[288].texture = tile_Room_01_Floor;

            tile_List[494].texture = tile_Sidewalk;
            tile_List[451].texture = tile_Sidewalk;
            tile_List[409].texture = tile_Sidewalk;
            tile_List[368].texture = tile_Sidewalk;
            tile_List[328].texture = tile_Sidewalk;

            tile_List[539].texture = tile_Road;
            tile_List[495].texture = tile_Road;
            tile_List[452].texture = tile_Road;
            tile_List[410].texture = tile_Road;
            tile_List[369].texture = tile_Road;

            tile_List[585].texture = tile_Sidewalk;
            tile_List[540].texture = tile_Sidewalk;
            tile_List[496].texture = tile_Sidewalk;
            tile_List[453].texture = tile_Sidewalk;
            tile_List[411].texture = tile_Sidewalk;

            */

        }

        private void LoadPlayer()
        {
            player = new Player(Content, new Vector2(scr_Width / 2, scr_Height / 2));
            allObjectsList.Add(player);
            player.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            HandleInput(gameTime);

            player.Update(gameTime, keyboardState);

            camera.Update(gameTime);
            //camera.Position = Mouse.GetState().Position.ToVector2();
            
            camera.Position = player.position;

            

            base.Update(gameTime);
        }

        void HandleInput(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(planting_cooldown <= 0)
            {
                if (keyboardState.IsKeyDown(Keys.E))
                {
                    allObjectsList.Add(new Plant(Content, player.position));
                    planting_cooldown += 60;
                }
            }
            
            if(planting_cooldown > 0)
            {
                planting_cooldown--;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            bool playerDrawn = false;
            // TODO: Add your drawing code here
            spriteBatch.Begin(camera);


            foreach(Tile _tile in tile_List)
            {
                //System.Diagnostics.Debug.WriteLine("Tile Position: " + _tile.position);
                spriteBatch.Draw(_tile.texture, position: _tile.position, scale: _tile.scale, layerDepth: 0.4f);
                if(_tile.innerPlant != null)
                {
                    _tile.innerPlant.Draw(gameTime, spriteBatch);
                }
                if(playerDrawn == false)
                {
                    if(_tile.texture == tile_Grass_Tree || _tile.isDecoration == true)
                    {
                        if ((player.position.Y > _tile.position.Y + 10 && player.position.Y < _tile.position.Y + 64) && (player.position.X > _tile.position.X + 24 && player.position.X < _tile.position.X + 36))
                        {
                            playerDrawn = true;
                            player.Draw(gameTime, spriteBatch);
                        }
                    } else
                    {
                        if ((player.position.Y > _tile.position.Y + 17 && player.position.Y < _tile.position.Y + 56) && (player.position.X > _tile.position.X + 16 && player.position.X < _tile.position.X + 48))
                        {
                            playerDrawn = true;
                            player.Draw(gameTime, spriteBatch);
                        }
                    }
                    
                    
                }
            }

            if(playerDrawn == false)
            {
                player.Draw(gameTime, spriteBatch);
            }

            /* DEBUG SHOW TILE INDEX
             *
            foreach(Tile _tile in tile_List)
            {
                Vector2 textMiddlePoint = font.MeasureString(_tile.position.ToString()) / 2;
                spriteBatch.DrawString(font, (tile_List.IndexOf(_tile) + 1).ToString(), _tile.position + new Vector2(32, 48), Color.Black, 0, textMiddlePoint, 0.25f, SpriteEffects.None, 0.5f);
            }
            */
            
            

            //DrawObjects(gameTime, spriteBatch);


            spriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawObjects(GameTime gameTime, SpriteBatch spriteBatch)
        {

            foreach(var o in plants)
            {
                o.Draw(gameTime, spriteBatch);
            }
        }

        public static bool IsInPolygon(Point[] poly, Point point)
        {
            var coef = poly.Skip(1).Select((p, i) =>
                                            (point.Y - poly[i].Y) * (p.X - poly[i].X)
                                          - (point.X - poly[i].X) * (p.Y - poly[i].Y))
                                    .ToList();

            if (coef.Any(p => p == 0))
                return true;

            for (int i = 1; i < coef.Count(); i++)
            {
                if (coef[i] * coef[i - 1] < 0)
                    return false;
            }
            return true;
        }
    }
}
