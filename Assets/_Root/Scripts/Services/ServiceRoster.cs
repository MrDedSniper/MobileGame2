using System;
using Services.Ads;
using Services.Ads.UnityAds;
using Services.IAP;
using Tool.Analytics;
using UnityEngine;

namespace Services
{
    internal class ServiceRoster : MonoBehaviour
    {
        private static ServiceRoster _instance;

        private static ServiceRoster Instance => _instance ??= FindObjectOfType<ServiceRoster>();

        public static IAnalyticsManager Analytics => Instance._analyticsManager;
        public static IAdsService AdsService => Instance._adsSettings;
        public static IIAPService IAPService => Instance._iapService;

        [SerializeField] private AnalyticsManager _analyticsManager;
        [SerializeField] private UnityAdsService _adsSettings;
        [SerializeField] private IAPService _iapService;

        private void Awake()
        {
            _instance ??= this;
        }
    }
}