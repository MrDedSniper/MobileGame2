using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ui.GameMenu
{
    internal class GameMenuView : MonoBehaviour
    {
        [SerializeField] private Button _buttonBack;

        public void Init(UnityAction back) => _buttonBack.onClick.AddListener(back);

        private void OnDestroy() => _buttonBack.onClick.RemoveAllListeners();
    }
}