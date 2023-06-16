using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Features.Inventory;
using Features.Shed.Upgrade;
using Game.Car;
using Profile;
using Tool;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Features.Shed
{
    internal interface IShedController
    {
        
    }
    internal class ShedController : BaseController, IShedController
    {
        private readonly ResourcePath _viewPath = new ResourcePath("Prefabs/Shed/ShedView");
        private readonly ResourcePath _dataSourcePath = new ResourcePath("Configs/Shed/UpgradeItemConfigDataSource");

        private readonly ShedView _view;
        private readonly ProfilePlayer _profilePlayer;
        private readonly InventoryController _inventoryController;
        private readonly UpgradeHandlerRepository _upgradeHandlerRepository;

        public ShedController([NotNull] Transform placeForUi, [NotNull] ProfilePlayer profilePlayer)
        {
            if (placeForUi == null)
            {
                throw new ArgumentException(nameof(placeForUi));
            }
            
            _profilePlayer = profilePlayer ?? throw new ArgumentException(nameof(profilePlayer));

            _upgradeHandlerRepository = CreateRepository();
            _inventoryController = CreateInventoryController(placeForUi);
            _view = LoadView(placeForUi);
            
            _view.Init(Apply, Back);
        }
        private UpgradeHandlerRepository CreateRepository()
        {
            UpgradeItemConfig[] upgradeConfigs = ContentDataSourceLoader.LoadUpgradeItemConfigs(_dataSourcePath);
            var repository = new UpgradeHandlerRepository(upgradeConfigs);
            AddRepository(repository);

            return repository;
        }
        private InventoryController CreateInventoryController(Transform placeForUi)
        {
            var inventoryController = new InventoryController(placeForUi, _profilePlayer.Inventory);
            AddController(inventoryController);

            return inventoryController;
        }
        
        private ShedView LoadView(Transform placeForUi)
        {
            GameObject prefab = ResourcesLoader.LoadPrefab(_viewPath);
            GameObject objectView = UnityEngine.Object.Instantiate(prefab, placeForUi, false);
            AddGameObject(objectView);

            return objectView.GetComponent<ShedView>();
        }
        
        private void Apply()
        {
            _profilePlayer.CurrentCar.Restore();

            UpgradeWithEquippedItems(_profilePlayer.CurrentCar, _profilePlayer.Inventory.EquippedItems,
                _upgradeHandlerRepository.Items);

            _profilePlayer.CurrentState.Value = GameState.Start;
            Log($"Apply. Current Speed: {_profilePlayer.CurrentCar.Speed}. " + 
                $"Current Jump Heigh: {_profilePlayer.CurrentCar.JumpHeight}");
        }

        private void Back()
        {
            _profilePlayer.CurrentState.Value = GameState.Start;
            Log($"Back. Current Speed: {_profilePlayer.CurrentCar.Speed}. " + 
                $"Current Jump Heigh: {_profilePlayer.CurrentCar.JumpHeight}");
        }
        
        private void UpgradeWithEquippedItems(IUpgradable upgradable, IReadOnlyList<string> equippedItems,
            IReadOnlyDictionary<string, IUpgradeHandler> upgradeHandlers)
        {
            foreach (string itemId in equippedItems)
            {
                if (upgradeHandlers.TryGetValue(itemId, out IUpgradeHandler handler))
                {
                    handler.Upgrade(upgradable);
                }
            }
        }

        private void Log(string messege) => Debug.Log($"[{GetType().Name}] {messege}");

    }
}