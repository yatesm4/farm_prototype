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

        private Objects.Animation selectArrowAnimation;
        private AnimationPlayer selectArrowSprite;

        SpriteFont sprFont;

        Texture2D spr_Man;
        Texture2D tile_Wood;

        Texture2D tile_Glow;

        Texture2D tile_Grass;
        Texture2D tile_Grass_Tree;
        Texture2D tile_Road;
        Texture2D tile_Sidewalk;

        Texture2D tile_Floor;
        Texture2D tile_Room;
        Texture2D tile_Room_01;
        Texture2D tile_Room_01_Floor;

        Texture2D tile_Grass_Bench;

        // Tiles

        Map game_Map;
        Tile[,] tile_Arr;

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

            selectArrowAnimation = new Objects.Animation(Content.Load<Texture2D>("Sprites/UI/UI_DownArrow"), 0.1f, true);
            selectArrowSprite.PlayAnimation(selectArrowAnimation);

            Texture2D[] textures_Arr = new Texture2D[8];

            // TODO: use this.Content to load your game content here
            tile_Wood = Content.Load<Texture2D>("tile_Wood");

            textures_Arr[0] = tile_Glow = Content.Load<Texture2D>("Sprites/Environment/Ground_Glow");

            textures_Arr[1] = tile_Grass = Content.Load<Texture2D>("Sprites/Environment/Ground_Grass");
            textures_Arr[2] = tile_Grass_Tree = Content.Load<Texture2D>("Sprites/Environment/Ground_Tree");
            textures_Arr[3] = tile_Road = Content.Load<Texture2D>("Sprites/Environment/Ground_Road");
            textures_Arr[4] = tile_Sidewalk = Content.Load<Texture2D>("Sprites/Environment/Floor_Sidewalk");

            tile_Room = Content.Load<Texture2D>("Sprites/Environment/Structures/Room_Base");
            tile_Floor = Content.Load<Texture2D>("Sprites/Environment/Structures/Floor_Base");

            textures_Arr[5] = tile_Room_01 = Content.Load<Texture2D>("Sprites/Environment/Structures/Room_01");
            textures_Arr[6] = tile_Room_01_Floor = Content.Load<Texture2D>("Sprites/Environment/Structures/Room_01_Floor");

            textures_Arr[7] = tile_Grass_Bench = Content.Load<Texture2D>("Sprites/Environment/Ground_Grass_Bench");



            sprFont = Content.Load<SpriteFont>("Fonts/Font_01");

            LoadMap(textures_Arr);

            LoadPlayer();
        }

        private void LoadMap(Texture2D[] textures_)
        {
            
            tile_Arr = new Tile[50, 50];
            for (var x = 0; x < 50; x++)
            {
                for (var y = 0; y < 50; y++)
                {
                    if (rnd.Next(1, 100) > 10)
                    {
                        tile_Arr[x, y] = new Tile(tile_Grass, new Vector2(x * 32 - y * 32, x * 16 + y * 16), new Vector2(x, y));
                    } else
                    {
                        if(rnd.Next(1,100) > 90)
                        {
                            tile_Arr[x, y] = new Tile(tile_Grass_Bench, new Vector2(x * 32 - y * 32, x * 16 + y * 16), new Vector2(x, y));
                            tile_Arr[x, y].TileNPC = new NPC(Content, 1, 3, tile_Arr[x, y].TileIndex, tile_Arr);
                        }
                        else
                        {
                            tile_Arr[x, y] = new Tile(tile_Grass, new Vector2(x * 32 - y * 32, x * 16 + y * 16), new Vector2(x, y), tile_Grass_Tree);
                        }
                    }
                    tile_Arr[x, y].OutlineTexture = tile_Glow;
                }
            }
            game_Map = new Map(tile_Arr, 50, 50, 64, 64, textures_, sprFont);
        }

        private void LoadPlayer()
        {
            player = new Player(Content, tile_Arr[20,20].CenterPoint, new Vector2(20,20), tile_Arr);
            player.LoadContent(Content);
            player.selectCursor = selectArrowSprite;
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
            game_Map.Update(gameTime, keyboardState);
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

            /*
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
            */
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

            game_Map.Draw(gameTime, spriteBatch, player);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
