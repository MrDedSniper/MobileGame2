using System;
using Services;
using Profile;
using Tool;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Ui
{
    internal class MainMenuController : BaseController
    {
        private readonly ResourcePath _resourcePath = new ResourcePath("Prefabs/UI/MainMenu");
        private readonly ProfilePlayer _profilePlayer;
        private readonly MainMenuView _view;

        public MainMenuController(Transform placeForUi, ProfilePlayer profilePlayer)
        {
            _profilePlayer = profilePlayer;
            _view = LoadView(placeForUi);
            _view.Init(new UnityAction(StartGame),
                new UnityAction(OpenSettings), 
                new UnityAction(OpenShed), 
                new UnityAction(PlayRewardedAds), 
                new UnityAction<string>(BuyProduct)
                );

            SubscribeAds();
            SubscribeIAP();
        }

        protected override void OnDispose()
        {
            UnsubscribeAds();
            UnsubscribeIAP();
        }
        private MainMenuView LoadView(Transform placeForUi)
        {
            GameObject prefab = ResourcesLoader.LoadPrefab(_resourcePath);
            GameObject objectView = Object.Instantiate(prefab, placeForUi, false);
            AddGameObject(objectView);

            return objectView.GetComponent<MainMenuView>();
        }

        private void StartGame() =>
            _profilePlayer.CurrentState.Value = GameState.Game;
        
        private void OpenSettings() =>
            _profilePlayer.CurrentState.Value = GameState.Settings;
        
        private void OpenShed() =>
            _profilePlayer.CurrentState.Value = GameState.Shed;
        private void PlayRewardedAds() => ServiceRoster.AdsService.RewardedPlayer.Play();

        private void BuyProduct(string productId) => ServiceRoster.IAPService.Buy(productId);
        
        private void SubscribeAds()
        {
            ServiceRoster.AdsService.RewardedPlayer.Finished += OnAdsFinised;
            ServiceRoster.AdsService.RewardedPlayer.Failed += OnAdsCancelled;
            ServiceRoster.AdsService.RewardedPlayer.Skipped += OnAdsCancelled;
        }
        
        private void SubscribeIAP()
        {
            ServiceRoster.IAPService.PurchaseSucceed.AddListener(OnIAPSucceed);
            ServiceRoster.IAPService.PurchaseFailed.AddListener(OnIAPFailed);
        }
        
        private void UnsubscribeAds()
        {
            ServiceRoster.AdsService.RewardedPlayer.Finished -= OnAdsFinised;
            ServiceRoster.AdsService.RewardedPlayer.Failed -= OnAdsCancelled;
            ServiceRoster.AdsService.RewardedPlayer.Skipped -= OnAdsCancelled;
        }
        
        private void UnsubscribeIAP()
        {
            ServiceRoster.IAPService.PurchaseSucceed.RemoveListener(OnIAPSucceed);
            ServiceRoster.IAPService.PurchaseFailed.RemoveListener(OnIAPFailed);
        }

        private void OnAdsFinised() => Debug.Log("Вы получили награду за просмотр рекламы!");
        private void OnAdsCancelled() => Debug.Log("Вы не получили награду, т.к. не просмотрели рекламу");
        private void OnIAPSucceed() => Debug.Log("Покупка завершена!");
        private void OnIAPFailed() => Debug.Log("Покупка прервана");
    }
}
