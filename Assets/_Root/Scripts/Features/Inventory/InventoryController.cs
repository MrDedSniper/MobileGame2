using System;
using Features.Inventory.Items;
using JetBrains.Annotations;
using Tool;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Features.Inventory
{
    internal class InventoryController : BaseController
    {
        private readonly ResourcePath _viewPath = new ResourcePath("Prefabs/Inventory/InventoryView");
        private readonly ResourcePath _dataSourcePath = new ResourcePath("");

        private readonly InventoryView _view;
        private readonly IInventoryModel _model;
        private readonly ItemRepository _repository;

        public InventoryController(
            [NotNull] Transform placeForUi, 
            [NotNull] IInventoryModel inventoryModel)
        {
            if (placeForUi == null)
            {
                throw new ArgumentException(nameof(placeForUi));
            }
            
            _model = inventoryModel ?? throw new ArgumentException(nameof(inventoryModel));

            _repository = CreateRepository();
            _view = LoadView(placeForUi);

            _view.Display(_repository.Items.Values, OnItemClicked);

            foreach (string itemId in _model.EquippedItems)
            {
                _view.Select(itemId);
            }
        }
        private ItemRepository CreateRepository()
        {
            ItemConfig[] itemConfigs = ContentDataSourceLoader.LoadItemConfigs(_dataSourcePath);
            var repository = new ItemRepository(itemConfigs);
            AddRepository(repository);

            return repository;
        }

        private InventoryView LoadView(Transform placeForUi)
        {
            GameObject prefab = ResourcesLoader.LoadPrefab(_viewPath);
            GameObject objectView = Object.Instantiate(prefab, placeForUi);
            AddGameObject(objectView);

            return objectView.GetComponent<InventoryView>();
        }
        
        private void OnItemClicked(string itemId)
        {
            bool isEquipped = _model.IsEquipped(itemId);
            
            if (isEquipped) UnequipItem(itemId);
            else
            {
                EquipItem(itemId);           
            }
        }

        private void EquipItem(string itemId)
        {
            _view.Select(itemId);
            _model.EquipItem(itemId);
        }

        private void UnequipItem(string itemId)
        {
            _view.Unselect(itemId);
            _model.UnequipItem(itemId);
        }
    }
}