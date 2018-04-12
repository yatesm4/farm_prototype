using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm_Prototype.Objects
{
    public class PlayerInventory
    {
        private int _inventoryMaxLimit { get; set; } = 25;
        public int InventoryMaxLimit
        {
            get { return _inventoryMaxLimit; }
            set { _inventoryMaxLimit = value; }
        }

        private ItemStack[] _inventoryItems { get; set; } = new ItemStack[25];
        public ItemStack[] InventoryItems
        {
            get { return _inventoryItems; }
            set { _inventoryItems = value; }
        }

        private int _currencyAmount { get; set; } = 0;
        public int CurrencyAmount
        {
            get { return _currencyAmount; }
            set { _currencyAmount = value; }
        }

        public PlayerInventory(int max_, int currency_)
        {
            InventoryMaxLimit = max_;
            InventoryItems = new ItemStack[InventoryMaxLimit];
            CurrencyAmount = currency_;
        }

        public Item GetItem(int index_)
        {
            if(index_ <= (InventoryMaxLimit - 1) && InventoryItems[index_] != null)
            {
                if (InventoryItems[index_].StackCount > 0 && InventoryItems[index_].Item != null)
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

        public void AddItem(Item item_)
        {
            int index = InventoryItems.Length;
            InventoryItems[index].Item = item_;
        }
    }
}
