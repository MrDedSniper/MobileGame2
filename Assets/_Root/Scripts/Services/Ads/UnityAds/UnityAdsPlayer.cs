using System;
using UnityEngine;
using UnityEngine.Advertisements;

 namespace Services.Ads.UnityAds
{
   internal abstract class UnityAdsPlayer : IAdsPlayer, IUnityAdsShowListener, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsListener
    {
        public event Action Started;
        public event Action Finished;
        public event Action Failed;
        public event Action Skipped;
        public event Action BecomeReady;

        protected readonly string Id = "5302786";
        protected readonly string _rewardPlace = "Reward Android";
        protected readonly string _interstitialPlace = "Interstitial Android";

        private Action _callbackSuccessShowAds;


        protected UnityAdsPlayer(string id)
        {
            Id = id;
            //Advertisement.AddListener(this);
            Advertisement.Initialize(Id, true, this);
        }

        public void ShowInterstitial()
        {
            _callbackSuccessShowAds = null;
            Advertisement.Show(_interstitialPlace, this);
        }

        public void ShowRewarded(Action successShow)
        {
            _callbackSuccessShowAds = successShow;
            Advertisement.Show(_rewardPlace, this);
        }


        public void Play()
        {
            Load();
            OnPlaying();
            Load();
            
            Debug.Log("Play");
        }

        protected abstract void OnPlaying();
        protected abstract void Load();


        void IUnityAdsListener.OnUnityAdsReady(string placementId)
        {
            if (IsIdMy(placementId) == false)
                return;

            Log("Ready");
            BecomeReady?.Invoke();
        }

        void IUnityAdsListener.OnUnityAdsDidError(string message) =>
            Error($"Error: {message}");

        void IUnityAdsListener.OnUnityAdsDidStart(string placementId)
        {
            if (IsIdMy(placementId) == false)
                return;

            Log("Started");
            Started?.Invoke();
        }

        void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            if (IsIdMy(placementId) == false)
                return;

            switch (showResult)
            {
                case ShowResult.Finished:
                    Log("Finished");
                    Finished?.Invoke();
                    break;

                case ShowResult.Failed:
                    Error("Failed");
                    Failed?.Invoke();
                    break;

                case ShowResult.Skipped:
                    Log("Skipped");
                    Skipped?.Invoke();
                    break;
            }
        }


        private bool IsIdMy(string id) => Id == id;

        private void Log(string message) => Debug.Log(WrapMessage(message));
        private void Error(string message) => Debug.LogError(WrapMessage(message));
        private string WrapMessage(string message) => $"[{GetType().Name}] {message}";
        
        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            throw new NotImplementedException();
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            throw new NotImplementedException();
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            throw new NotImplementedException();
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                _callbackSuccessShowAds?.Invoke();
                _callbackSuccessShowAds = null;
            }
        }

        public void OnInitializationComplete()
        {
            throw new NotImplementedException();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            throw new NotImplementedException();
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            throw new NotImplementedException();
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            throw new NotImplementedException();
        }
    }
}
