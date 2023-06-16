using System;
using System.Collections.Generic;

namespace Features.Inventory.Items
{
    internal interface IItemRepository
    {
        IReadOnlyDictionary<String, IItem> Items { get; }
    }
    
    internal class ItemRepository : BaseRepository<string, IItem, ItemConfig>
    {
        public ItemRepository(IEnumerable<ItemConfig> configs) : base(configs)
        {
        }

        protected override string GetKey(ItemConfig config) => config.Id;

        protected override IItem CreateItem(ItemConfig config) =>
            new Item(config.Id, new ItemInfo(config.Title, config.Icon));
    }
}