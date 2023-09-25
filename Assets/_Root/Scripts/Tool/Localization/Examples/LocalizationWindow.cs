using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

namespace Tool.Localization.Examples
{
    internal abstract class LocalizationWindow : MonoBehaviour
    {
        private const int EnglishIndex = 0;
        private const int SpainIndex = 1;
        private const int RussianIndex = 2;

        [Header("Scene Components")]
        [SerializeField] private Button _englishButton;
        [SerializeField] private Button _spainButton;
        [SerializeField] private Button _russianButton;


        private void Start()
        {
            _englishButton.onClick.AddListener(() => ChangeLanguage(EnglishIndex));
            _spainButton.onClick.AddListener(() => ChangeLanguage(SpainIndex));
            _russianButton.onClick.AddListener(() => ChangeLanguage(RussianIndex));
            OnStarted();
        }

        private void OnDestroy()
        {
            _englishButton.onClick.RemoveAllListeners();
            _spainButton.onClick.RemoveAllListeners();
            _russianButton.onClick.RemoveAllListeners();
            OnDestroyed();
        }


        protected virtual void OnStarted() { }
        protected virtual void OnDestroyed() { }


        private void ChangeLanguage(int index) =>
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
