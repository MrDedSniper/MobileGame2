using Tool;
using Profile;
using System;
using System.Collections.Generic;
using UnityEngine;
using Features.Inventory;
using Features.Shed.Upgrade;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

namespace Features.Shed
{
    internal interface IShedController
    {
    }
    internal class ShedController : BaseController, IShedController
    {
        private readonly IShedView _view;
        private readonly ProfilePlayer _profilePlayer;
        private readonly IUpgradeHandlersRepository _upgradeHandlersRepository;

        public ShedController(
            [NotNull] IShedView view,
            [NotNull] ProfilePlayer profilePlayer,
            [NotNull] IUpgradeHandlersRepository upgradeHandlersRepository)
        {
            _view = view ?? throw new ArgumentException(nameof(view));
            _profilePlayer = profilePlayer ?? throw new AggregateException(nameof(profilePlayer));
            _upgradeHandlersRepository = upgradeHandlersRepository ?? throw new ArgumentException(nameof(upgradeHandlersRepository));
            
            _view.Init(Apply, Back);

        }

        protected override void OnDispose()
        {
            _view.Deinit();
            base.OnDispose();
        }

        private void Apply()
        {
            _profilePlayer.CurrentCar.Restore();

            UpgradeWithEquippedItems(
                _profilePlayer.CurrentCar,
                _profilePlayer.Inventory.EquippedItems,
                _upgradeHandlersRepository.Items);

            _profilePlayer.CurrentState.Value = GameState.Start;
            Debug.Log("Apply. " +
                $"Current Speed: {_profilePlayer.CurrentCar.Speed}. " + 
                $"Current Speed: {_profilePlayer.CurrentCar.JumpHeight}");
        }

        private void Back()
        {
            _profilePlayer.CurrentState.Value = GameState.Start;
            Debug.Log("Back. " +
                      $"Current Speed: {_profilePlayer.CurrentCar.Speed}. " + 
                      $"Current Speed: {_profilePlayer.CurrentCar.JumpHeight}");
        }

        private void UpgradeWithEquippedItems(
            IUpgradable upgradable,
            IReadOnlyList<string> equippedItems,
            IReadOnlyDictionary<string, IUpgradeHandler> upgradeHandlers)
        {
            foreach (string itemId in equippedItems)
                if (upgradeHandlers.TryGetValue(itemId, out IUpgradeHandler handler))
                    handler.Upgrade(upgradable);
        }
    }
}