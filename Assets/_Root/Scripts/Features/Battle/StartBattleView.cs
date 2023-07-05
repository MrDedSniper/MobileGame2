using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Features.Battle
{
    internal class StartBattleView : MonoBehaviour
    {
        [SerializeField] private Button _startBattleButton;

        public void Init(UnityAction startFight) =>
            _startBattleButton.onClick.AddListener(startFight);

        private void OnDestroy() =>
            _startBattleButton.onClick.RemoveAllListeners();
    }
}
