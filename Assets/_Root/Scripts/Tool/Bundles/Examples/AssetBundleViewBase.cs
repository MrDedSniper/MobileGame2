using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Tool.Bundles.Examples
{
    internal class AssetBundleViewBase : MonoBehaviour
    {
        private const string UrlAssetBundleBackground = "https://drive.google.com/uc?export=download&id=1y2gUPCn4mUzfZ6W8L7K4NeNitS03_Pn0";
        private const string UrlAssetBundleSprites = "https://drive.google.com/uc?export=download&id=1rQzWdcChHhJJBTe4rf1D0Kwi1a43jxWR";
        private const string UrlAssetBundleAudio = "https://drive.google.com/uc?export=download&id=1I7euU6Hv5yrn1ektprUumbGHEikklk3Y";

        [SerializeField] private DataSpriteBundle[] _dataBackgroundBundles;
        [SerializeField] private DataSpriteBundle[] _dataSpriteBundles;
        [SerializeField] private DataAudioBundle[] _dataAudioBundles;

        private AssetBundle _backgroundAssetBundle;
        private AssetBundle _spritesAssetBundle;
        private AssetBundle _audioAssetBundle;

        protected IEnumerator DownloadAndSetBackgroundAssetBundles()
        {
            yield return GetBackgroundAsssetBundle();
            
            if(_backgroundAssetBundle != null)
            {
                SetBackgroundAssets(_backgroundAssetBundle);
            }
            else
            {
                Debug.LogError($"AssetBundle {nameof(_backgroundAssetBundle)} failed to load");
            }
        }

        protected IEnumerator DownloadAndSetSpriteAssetBundles()
        {
            yield return GetSpritesAssetBundle();

            if (_spritesAssetBundle != null)
                SetSpriteAssets(_spritesAssetBundle);
            else
                Debug.LogError($"AssetBundle {nameof(_spritesAssetBundle)} failed to load");
        }
        
        protected IEnumerator DownloadAndSetAudioAssetBundles()
        {
            yield return GetAudioAssetBundle();

            if (_audioAssetBundle != null)
                SetAudioAssets(_audioAssetBundle);
            else
                Debug.LogError($"AssetBundle {nameof(_audioAssetBundle)} failed to load");
        }

        private IEnumerator GetBackgroundAsssetBundle()
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleBackground);
            
            yield return request.SendWebRequest();

            while (!request.isDone)
                yield return null;

            StateRequest(request, out _spritesAssetBundle);
        }

        private IEnumerator GetSpritesAssetBundle()
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleSprites);

            yield return request.SendWebRequest();

            while (!request.isDone)
                yield return null;

            StateRequest(request, out _backgroundAssetBundle);
        }

        private IEnumerator GetAudioAssetBundle()
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(UrlAssetBundleAudio);

            yield return request.SendWebRequest();

            while (!request.isDone)
                yield return null;

            StateRequest(request, out _audioAssetBundle);
        }

        private void StateRequest(UnityWebRequest request, out AssetBundle assetBundle)
        {
            if (request.error == null)
            {
                assetBundle = DownloadHandlerAssetBundle.GetContent(request);
                Debug.Log("Complete");
            }
            else
            {
                assetBundle = null;
                Debug.LogError(request.error);
            }
        }

        private void SetBackgroundAssets(AssetBundle assetBundle)
        {
            foreach (DataSpriteBundle data in _dataBackgroundBundles)
                data.Image.sprite = assetBundle.LoadAsset<Sprite>(data.NameAssetBundle);
        }
        

        private void SetSpriteAssets(AssetBundle assetBundle)
        {
            foreach (DataSpriteBundle data in _dataSpriteBundles)
                data.Image.sprite = assetBundle.LoadAsset<Sprite>(data.NameAssetBundle);
        }

        private void SetAudioAssets(AssetBundle assetBundle)
        {
            foreach (DataAudioBundle data in _dataAudioBundles)
            {
                data.AudioSource.clip = assetBundle.LoadAsset<AudioClip>(data.NameAssetBundle);
                data.AudioSource.Play();
            }
        }
    }
}
