using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Farm_Prototype.Content;

namespace Farm_Prototype.Objects
{
    public class Vendor : NPC
    {
        public Vendor(GameContent Content, int npcIndex, int headIndex, Vector2 tileIndex, Tile[,] tiles) : base(Content, npcIndex, headIndex, tileIndex, tiles)
        {
            speechBubble.IconIndex = 4;
        }

        private int _inventoryCount { get; set; } = 10;
        public int InventoryCount
        {
            get { return _inventoryCount; }
            set { _inventoryCount = value; }
        }

        private ItemStack[] _inventoryItems { get; set; } = new ItemStack[10];
        public ItemStack[] InventoryItems
        {
            get { return _inventoryItems; }
            set { _inventoryItems = value; }
        }

        public Item GetItem(int index_)
        {
            if(index_ <= (InventoryCount - 1) && InventoryItems[index_] != null)
            {
                if(InventoryItems[index_].StackCount > 0 && InventoryItems[index_].Item != null)
                {
                    Item ret_Item = InventoryItems[index_].Item;
                    InventoryItems[index_].StackCount--;
                    return ret_Item;
                } else
                {
                    return null;
                }
            } else
            {
                return null;
            }
        }
    }
}
