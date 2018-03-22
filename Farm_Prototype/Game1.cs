using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Farm_Prototype.Physics;
using Farm_Prototype.Objects;

namespace Farm_Prototype
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

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

        // Tiles

        List<Tile> tile_List = new List<Tile>();

        // Objects

        Lerper lerp = new Lerper();

        Player player;

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

            tiles_Width = scr_Width / 32;
            tiles_Height = scr_Height / 32;

            System.Diagnostics.Debug.WriteLine("tile_Width = " + tiles_Width);
            System.Diagnostics.Debug.WriteLine("tile_Height = " + tiles_Height);

            position = new Vector2(scr_Width / 2, scr_Height / 2);

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
            spr_Man = Content.Load<Texture2D>("spr_Man");
            tile_Wood = Content.Load<Texture2D>("tile_Wood");

            LoadTiles();

            LoadPlayer();
        }

        private void LoadTiles()
        {
            for (var i = 0; i < tiles_Height; i++)
            {
                for (var j = 0; j < tiles_Width; j++)
                {
                    Tile _tile = new Tile
                    {
                        texture = tile_Wood,
                        position = new Vector2(j * 32, i * 32)
                    };
                    tile_List.Add(_tile);
                }
            }
        }

        private void LoadPlayer()
        {
            player = new Player
            {
                sprite = spr_Man,
                position = new Vector2(scr_Width / 2, scr_Height / 2),
                scale = new Vector2(3, 3)
            };
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                player.position = new Vector2(player.position.X, lerp.Lerp(player.position.Y, player.position.Y - 32));
            } else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                player.position = new Vector2(player.position.X, lerp.Lerp(player.position.Y, player.position.Y + 32));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                player.position = new Vector2(lerp.Lerp(player.position.X, player.position.X - 32), player.position.Y);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                player.position = new Vector2(lerp.Lerp(player.position.X, player.position.X + 32), player.position.Y);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            foreach(Tile _tile in tile_List)
            {
                System.Diagnostics.Debug.WriteLine("Tile Position: " + _tile.position);
                spriteBatch.Draw(_tile.texture, position: _tile.position, scale: _tile.scale);
            }

            spriteBatch.Draw(player.sprite, position: player.position, scale: player.scale);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
