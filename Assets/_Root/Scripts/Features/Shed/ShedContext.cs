using System;
using Features.Inventory;
using Features.Shed.Upgrade;
using Profile;
using Tool;
using UnityEngine;

namespace Features.Shed
{
    internal class ShedContext : BaseController
    {
        private static readonly ResourcePath _viewPath = new("Prefabs/Shed/ShedView");
        private static readonly ResourcePath _dataSourcePath = new("Configs/Shed/UpgradeItemConfigDataSource");

        public ShedContext(Transform placeForUi, ProfilePlayer profilePlayer)
        {
            if (placeForUi == null)
            {
                throw new ArgumentNullException(nameof(placeForUi));
            }
            
            CreateController(profilePlayer, placeForUi);
        }

        private ShedController CreateController(ProfilePlayer profilePlayer, Transform placeForUi)
        {
            InventoryContext inventoryContext = CreateInventoryContext(placeForUi, profilePlayer.Inventory);
            UpgradeHandlersRepository shedRepository = CreateRepository();
            ShedView shedView = LoadView(placeForUi);

            return new ShedController(shedView, profilePlayer, shedRepository);
        }

        private InventoryContext CreateInventoryContext(Transform placeForUi, IInventoryModel model)
        {
            var context = new InventoryContext(placeForUi, model);
            AddContext(context);

            return context;
        }

        private UpgradeHandlersRepository CreateRepository()
        {
            UpgradeItemConfig[] upgradeConfigs = LoadConfigs();
            var repository = new UpgradeHandlersRepository(upgradeConfigs);
            AddRepository(repository);

            return repository;
        }

        private UpgradeItemConfig[] LoadConfigs() => ContentDataSourceLoader.LoadUpgradeItemConfigs(_dataSourcePath);

        private ShedView LoadView(Transform placeForUi)
        {
            GameObject prefab = ResourcesLoader.LoadPrefab(_viewPath);
            GameObject objectView = UnityEngine.Object.Instantiate(prefab, placeForUi, false);
            AddGameObject(objectView);

            return objectView.GetComponent<ShedView>();
        }
    }
}