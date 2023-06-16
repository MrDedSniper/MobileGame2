using System.Collections.Generic;
using Tool.Analytics.UnityAnalytics;
using UnityEngine;

namespace Tool.Analytics
{
    internal interface IAnalyticsManager
    {
        void SendGameStarted();

        void SendTransaction(string productId, decimal amount, string currency);
    }
    
    internal class AnalyticsManager : MonoBehaviour, IAnalyticsManager
    {
        private IAnalyticsService[] _services;

        private void Awake()
        {
            _services = new IAnalyticsService[]
            {
                new UnityAnalyticsService(),
            };
        }

        public void SendGameStarted()
        {
            SendEvent("Game Started");
        }

        public void SendTransaction(string productId, decimal amount, string currency)
        {
            for (int i = 0; i < _services.Length; i++)
            {
                _services[i].SendTransaction(productId, amount, currency);
            }
            
            Debug.Log($"Sent transaction {productId}");
        }
        
        private void SendEvent(string eventName)
        {
            foreach (IAnalyticsService service in _services)
            {
                service.SendEvent(eventName);
            }
        }
        
        public void SendMainMenuOpenedEvent() =>
            SendEvent("Main Menu Open");

        private void SendEvent(string eventName, Dictionary<string, object> eventData)
        {
            foreach (IAnalyticsService service in _services)
            {
                service.SendEvent(eventName, eventData);
            }
        }
        
    }
}