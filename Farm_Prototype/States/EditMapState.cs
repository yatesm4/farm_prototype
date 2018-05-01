using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using Newtonsoft.Json;

using Farm_Prototype.Interface;
using Farm_Prototype.Objects;
using Farm_Prototype.Content;


namespace Farm_Prototype.States
{
    public class EditMapState : State
    {
        private GameContent _gameContent { get; set; }

        private List<Component> _components { get; set; } = new List<Component>();

        private List<GridCell> _gridCells { get; set; } = new List<GridCell>();

        private List<TileData> _map { get; set; } = new List<TileData>();

        public CellDataDisplay CellDataDisplay { get; set; }

        public EditMapState(GameInstance game, GraphicsDevice graphicsDevice, ContentManager content, List<TileData> map) : base(game, graphicsDevice, content)
        {
            _map = map;
            _gameContent = new GameContent(content);
            LoadCells();
            CellDataDisplay = new CellDataDisplay(graphicsDevice, _gameContent)
            {
                Position = new Vector2((800 - (384)), 8)
            };
            _components.Add(CellDataDisplay);
        }

        private void LoadCells()
        {
            for(int w = 0; w < 50; w++)
            {
                for(int h = 0; h < 50; h++)
                {
                    // query corresponding tile
                    Color cellColor_ = Color.Green;
                    TileData td = (from a in _map
                                   where a.TileIndex.Equals(new Vector2(w,h))
                                   select a).SingleOrDefault<TileData>();
                    switch (td.TextureIndex)
                    {
                        case 2:
                            // tile is grass
                            break;
                        case 6:
                            // tile is bench
                            cellColor_ = Color.DarkGray;
                            break;
                        default:
                            cellColor_ = Color.Green;
                            break;
                    }

                    if (td.ContainsInner)
                    {
                        switch (td.InnerTextureIndex)
                        {
                            case 3:
                                // tile is tree
                                cellColor_ = Color.DarkGreen;
                                break;
                        }
                    }

                    if (td.ContainsNPC)
                    {
                        switch (td.NpcIndex)
                        {
                            case 0:
                                // npc is normal npc
                                cellColor_ = Color.Blue;
                                break;
                            case 1:
                                // npc is vendor
                                cellColor_ = Color.Orange;
                                break;
                        }
                    }
                    if(td.TileIndex.Equals(new Vector2(20, 20)))
                    {
                        cellColor_ = Color.Red;
                    }
                    var cell = new GridCell(cellColor_, _graphicsDevice)
                    {
                        Position = new Vector2(8 + (w * 8), 8 + (h * 8)),
                        TileData = td
                    };
                    cell.Click += delegate
                    {
                        GridCell_Click(cell);
                    };
                    _gridCells.Add(cell);
                    _components.Add(cell);
                }
            }
        }

        private void GridCell_Click(GridCell cell)
        {
            Console.WriteLine($"Changing cell:>");
            Console.WriteLine($"{JsonConvert.SerializeObject(cell.TileData, Formatting.Indented)}");
            CellDataDisplay.NextCell = cell;
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
            //
        }

        public override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                // on escape, go back to menu state
                _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
            }

            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
