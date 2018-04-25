using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Farm_Prototype.Interface;

namespace Farm_Prototype.States
{
    public class MenuState : State
    {
        private List<Component> _components;
        
        public MenuState(GameInstance game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            var buttonTexture = _content.Load<Texture2D>("Sprites/UI/UI_Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font_01");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 100),
                Text = "New Game",
                HoverColor = Color.Red
            };
            newGameButton.Click += NewGameButton_Click;

            var loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "Load Game",
                HoverColor = Color.Orange
            };
            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 300),
                Text = "Quit Game",
                HoverColor = Color.Green
            };
            quitGameButton.Click += QuitGameButton_Click;

            _components = new List<Component>()
            {
                newGameButton,
                loadGameButton,
                quitGameButton
            };

            Mouse.SetPosition(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height / 2);
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Quitting game...");
            _game.Exit();
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            // todo load game
            Console.WriteLine("Loading game...");
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            // todo new game
            Console.WriteLine("Starting new game...");
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            // load new state
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if not needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
