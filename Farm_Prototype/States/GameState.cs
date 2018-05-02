using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;

using Comora;

using Farm_Prototype.Content;
using Farm_Prototype.Objects;
using Farm_Prototype.Interface;

namespace Farm_Prototype.States
{
    public class GameState : State
    {
        private string LINE = "###########################################################################";

        private DebugMenu _debugMenu;
        private bool _debug { get; set; } = false;

        private Random _rndGen { get; set; } = new Random();
        private GameContent _gameContent { get; set; }

        private KeyboardState _previousKeyboardState { get; set; }

        private int _mapCount { get; set; } = 5;
        private List<Map> _maps { get; set; }
        private Map _currentMap { get; set; }
        private int _currentMapIndex { get; set; } = 0;
        private Map _nextMap { get; set; }

        private Player _player { get; set; }

        private Camera _camera { get; set; }
        private bool _firstTake { get; set; } = true;

        public GameState(GameInstance game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            _gameContent = new GameContent(content);
            LoadMaps();
            Console.WriteLine(LINE);
            if (_maps.Equals(null) || _maps.Count.Equals(0))
            {
                Console.WriteLine("No maps found, generating maps...");
                GenerateMaps();
                Console.WriteLine(LINE);
                LoadMaps();
                Console.WriteLine(LINE);
            }

            _currentMapIndex = _rndGen.Next(0, _maps.Count - 1);
            _currentMap = _maps[_currentMapIndex];
            Console.WriteLine($"Loading map: {_currentMapIndex + 1}");

            _camera = new Camera(graphicsDevice);
            _camera.Zoom = 1.5f;

            LoadPlayer();

            _debugMenu = new DebugMenu(graphicsDevice, _gameContent)
            {
                GameState = this
            };
        }

        public void LoadMaps()
        {
            _maps = new List<Map>();
            // loop through map count and load maps
            for(int i = 0; i < _mapCount; i++)
            {
                Console.WriteLine($"Loading map: {i + 1}");
                Console.WriteLine(LINE);
                Tile[,] tileArr_ = new Tile[50, 50];
                string data = null;
                try
                {
                    using (var streamReader = new System.IO.StreamReader($"data_map{(i + 1)}.json"))
                    {
                        data = streamReader.ReadToEnd();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error loading maps: {e.Message}");
                }
                // if the data read isn't null or empty, load the map | else, load the next map
                if (string.IsNullOrEmpty(data).Equals(true))
                {
                    continue;
                } else
                {
                    Console.WriteLine($"Loading map: {i + 1}");
                    List<TileData> tdList_ = JsonConvert.DeserializeObject<List<TileData>>(data);

                    foreach(TileData t in tdList_)
                    {
                        var x = (int)t.TileIndex.X;
                        var y = (int)t.TileIndex.Y;

                        if(t.ContainsNPC.Equals(true))
                        {
                            tileArr_[x, y] = new Tile(_gameContent.GetTileTexture(t.TextureIndex), t.Position, t.TileIndex);
                            switch (t.NpcIndex)
                            {
                                case 0:
                                    tileArr_[x, y].TileNPC = new NPC(_gameContent, 1, 2, t.TileIndex, tileArr_);
                                    break;
                                case 1:
                                    tileArr_[x, y].TileNPC = new Vendor(_gameContent, 1, 3, t.TileIndex, tileArr_);
                                    break;
                            }
                        }
                        else if (t.ContainsInner.Equals(true))
                        {
                            tileArr_[x, y] = new Tile(_gameContent.GetTileTexture(t.TextureIndex), t.Position, t.TileIndex, _gameContent.GetTileTexture(t.InnerTextureIndex));
                        }
                        else
                        {
                            tileArr_[x, y] = new Tile(_gameContent.GetTileTexture(t.TextureIndex), t.Position, t.TileIndex);
                        }
                        tileArr_[x, y].OutlineTexture = _gameContent.GetTileTexture(1);
                        tileArr_[x, y].TileData = t;
                    }
                }

                _maps.Add(new Map(tileArr_, 50, 50, 64, 64, _gameContent));
            }
        }

        public void SaveMap()
        {
            List<TileData> newData = new List<TileData>();
            foreach(Tile t in _currentMap.Tiles)
            {
                newData.Add(t.TileData);
            }
            using (var streamWriter = new System.IO.StreamWriter($"data_map{(_currentMapIndex + 1)}.json"))
            {
                streamWriter.WriteLine(JsonConvert.SerializeObject(newData, Formatting.Indented));
            }
            Console.WriteLine("Finished Saving Map.");
        }

        public void GenerateMaps()
        {
            for(int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Generating map: {i + 1}");
                List<TileData> tileData = new List<TileData>();
                for (var x = 0; x < 50; x++)
                {
                    for (var y = 0; y < 50; y++)
                    {
                        var position = new Vector2(x * 32 - y * 32, x * 16 + y * 16);
                        var td = new TileData
                        {
                            TileIndex = new Vector2(x, y),
                            Position = position,
                            TextureIndex = 2
                        };
                        if (_rndGen.Next(1, 100) < 10)
                        {
                            // generate inners
                            td.ContainsInner = true;
                            if (_rndGen.Next(1, 100) > 90)
                            {
                                // generate npc
                                td.ContainsNPC = true;
                                if (_rndGen.Next(1, 100) > 30)
                                {
                                    // generate normal npc
                                    td.NpcIndex = 0;
                                }
                                else
                                {
                                    // generate vendor
                                    td.NpcIndex = 1;
                                    td.TextureIndex = 6;
                                }
                            }
                            else
                            {
                                // generate scenery
                                td.InnerTextureIndex = 3;
                            }
                        }
                        tileData.Add(td);
                    }
                }
                using (var streamWriter = new System.IO.StreamWriter($"data_map{(i + 1)}.json"))
                {
                    streamWriter.WriteLine(JsonConvert.SerializeObject(tileData, Formatting.Indented));
                }
            }
        }

        public void LoadPlayer()
        {
            _player = new Player(_gameContent, _currentMap.Tiles[20, 20].CenterPoint, new Vector2(20, 20), _currentMap.Tiles);
        }

        public override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            HandleInput(gameTime, keyboardState);
            
            if(_nextMap != null)
            {
                _currentMap = _nextMap;
                _nextMap = null;
            }

            if (_debug.Equals(true))
            {
                _debugMenu.Update(gameTime);
                HandleDebugInput(keyboardState);
            }

            _previousKeyboardState = keyboardState;
        }

        public void HandleDebugInput(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.F)){
                Console.WriteLine("Applying texture to tile");
                switch (_debugMenu.SelectMenu.SelectedTextureTypeId)
                {
                    case 0:
                        _player.CurrentlyFacedTile().Texture = _debugMenu.SelectMenu.SelectedTexture;
                        _player.CurrentlyFacedTile().TileData.TextureIndex = _debugMenu.SelectMenu.SelectedTextureId;
                        break;
                    case 1:
                        _player.CurrentlyFacedTile().TileData.ContainsInner = true;
                        _player.CurrentlyFacedTile().Texture = _gameContent.GetTileTexture(2);
                        Console.WriteLine($"Changing tile tog grass: {_player.CurrentlyFacedTile().TileData.TextureIndex}");
                        _player.CurrentlyFacedTile().TileData.TextureIndex = 2;
                        _player.CurrentlyFacedTile().InnerTexture = _debugMenu.SelectMenu.SelectedTexture;
                        _player.CurrentlyFacedTile().TileData.InnerTextureIndex = _debugMenu.SelectMenu.SelectedTextureId;
                        break;
                    case 3:
                        // add npc to tile
                        break;
                }
            }
        }

        public void HandleInput(GameTime gameTime, KeyboardState keyboardState)
        {
            if (_firstTake.Equals(true))
            {
                _firstTake = false;
                _previousKeyboardState = keyboardState;
            }

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                // on escape, go back to menu state
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            } else if (keyboardState.IsKeyUp(Keys.LeftControl) && _previousKeyboardState.IsKeyDown(Keys.LeftControl))
            {
                if (_debug.Equals(true))
                {
                    Console.WriteLine($"Closing Debug Menu...");
                    _debug = false;
                } else
                {
                    Console.WriteLine($"Starting Debug Menu...");
                    _debug = true;
                }
            }

            _previousKeyboardState = keyboardState;
        }

        public override void PostUpdate(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            _currentMap.Update(gameTime, keyboardState, _camera);
            _player.Update(gameTime, keyboardState);
            _camera.Update(gameTime);
            _camera.Position = _player.position;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(_camera);

            _currentMap.Draw(gameTime, spriteBatch, _player);

            spriteBatch.End();

            spriteBatch.Begin();
            if (_debug.Equals(true))
            {
                _debugMenu.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
