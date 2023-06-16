using System;
using System.Collections.Generic;
using Features.Inventory.Items;
using UnityEngine;

namespace Features.Inventory
{
    public interface IInventoryView
    {
        void Display(IEnumerable<IItem> itemsCollection, Action<string> itemClicked);
        void Clear();
        void Select(string id);
        void Unselect(string id);
    }

    public class InventoryView : MonoBehaviour, IInventoryView
    {
        [SerializeField] private GameObject _itemViewPrefab;
        [SerializeField] private Transform _placeForItems;

        private readonly Dictionary<string, ItemView> _itemViews = new();

        public void Display(IEnumerable<IItem> itemsCollection, Action<string> itemClicked)
        {
            Clear();
            
            foreach (IItem item in itemsCollection) 
                _itemViews[item.Id] = CreateItemView(item, itemClicked);
        }
        public void Clear()
        {
            foreach (ItemView item in _itemViews.Values) 
                DestroyItemView(item);
            
            _itemViews.Clear();
        }

        public void Select(string id) => _itemViews[id].Select();

        public void Unselect(string id) => _itemViews[id].Unselect();
        
        private ItemView CreateItemView(IItem item, Action<string> itemClicked)
        {
            GameObject objectView = Instantiate(_itemViewPrefab, _placeForItems);
            ItemView itemView = objectView.GetComponent<ItemView>();
            
            itemView.Init(item,() => itemClicked?.Invoke(item.Id));
            return itemView;
        }
        
        private void DestroyItemView(ItemView itemView)
        {
            itemView.Deinit();
            Destroy(itemView.gameObject);
        }
        
    }
}