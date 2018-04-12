using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Farm_Prototype.Objects
{
    /// <summary>
    /// Multiple items (stacked) of the same type
    /// </summary>
    public class ItemStack
    {
        private Item _item { get; set; }
        public Item Item
        {
            get { return _item; }
            set { _item = value; }
        }

        private int _stackCount { get; set; } = 1;
        public int StackCount
        {
            get { return _stackCount; }
            set { _stackCount = value; }
        }

        public int StackValue
        {
            get { return Item.ItemValue * StackCount; }
        }

        public ItemStack(Item item_, int count_)
        {
            Item = item_;
            StackCount = count_;
        }
    }

    /// <summary>
    /// Individual Item Class
    /// </summary>
    public class Item
    {
        private int _itemId { get; set; } = 1;
        public int ItemId
        {
            get { return _itemId; }
            set { _itemId = value; }
        }

        private string _itemName { get; set; } = "Item";
        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }

        private int _itemValue { get; set; } = 0;
        public int ItemValue
        {
            get { return _itemValue; }
            set { _itemValue = value; }
        }

        private Texture2D _itemSprite { get; set; }
        public Texture2D ItemSprite
        {
            get { return _itemSprite; }
            set { _itemSprite = value; }
        }

        public Item(int id_, string name_, Texture2D sprite_, int value_)
        {
            ItemId = id_;
            ItemName = name_;
            ItemSprite = sprite_;
            ItemValue = value_;
        }
    }
}
