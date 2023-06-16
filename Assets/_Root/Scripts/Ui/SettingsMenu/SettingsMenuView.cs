using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ui
{
    public class SettingsMenuView : MonoBehaviour
    {
        [SerializeField] private Button _buttonMenuBack;

        private UnityAction _settingsMenu;
        
        public void Init(UnityAction settingsMenu)
        {
            _settingsMenu = settingsMenu;
            
            _buttonMenuBack.onClick.AddListener(settingsMenu);
        }

        public void OnDestroy()
        {
            _buttonMenuBack.onClick.RemoveAllListeners();
        }
            
    }
}
