using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Farm_Prototype.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Newtonsoft.Json;

using Farm_Prototype.Objects;
using Farm_Prototype.Content;

namespace Farm_Prototype.States
{
    public class EditMapsListState : State
    {
        private GameContent _gameContent { get; set; }

        private List<Component> _components { get; set; } = new List<Component>();

        private List<List<TileData>> _maps { get; set; } = new List<List<TileData>>();
        private List<TileData> _clickedMap { get; set; }

        public EditMapsListState(GameInstance game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            _gameContent = new GameContent(content);

            // load maps
            LoadMaps();

            int x = 50;
            int y = 50;
            int i = 0;
            // loop through each loaded map
            foreach(List<TileData> map in _maps)
            {
                var button = new Button(_gameContent.GetUiTexture(1), _gameContent.GetFont(1))
                {
                    Position = new Vector2(x, y + (100 * i)),
                    Text = "Map " + (i+1).ToString(),
                    HoverColor = Color.Red
                };
                button.Click += delegate
                {
                    Console.WriteLine($"Map {i + 1} clicked");
                    Map_Click(map);
                };
                _components.Add(button);
                i++;
            }
        }

        public void LoadMaps()
        {
            // loop through 5 possible maps
            for (int i = 1; i < 5; i++)
            {
                string path = $"data_map{i}.json";
                try
                {
                    string data = null;
                    using (var streamReader = new StreamReader(path))
                    {
                        data = streamReader.ReadToEnd();
                    }
                    if ((string.IsNullOrEmpty(data)).Equals(true))
                    {
                        Console.WriteLine($"Map data corrupted at path: {path}");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine($"Loading map at path: {path}");
                        _maps.Add(JsonConvert.DeserializeObject<List<TileData>>(data));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"No map found at path: {path}");
                    continue;
                }
            }
        }

        private void Map_Click(List<TileData> map)
        {
            _game.ChangeState(new EditMapState(_game, _graphicsDevice, _content, map));
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
            // post update
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
