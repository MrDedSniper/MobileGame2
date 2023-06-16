using UnityEngine.Advertisements;

namespace Services.Ads.UnityAds
{
    internal interface IUnityAdsListener
    {
        void OnUnityAdsDidError(string message);
        void OnUnityAdsDidStart(string placementId);
        void OnUnityAdsDidFinish(string placementId, ShowResult showResult);
        void OnUnityAdsReady(string placementId);
    }
}