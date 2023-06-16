using UnityEngine;

namespace Tool
{
    public class DontDestroyOnLoadObjects : MonoBehaviour
    {
        private void Awake()
        {
            if(enabled) DontDestroyOnLoad(gameObject);
        }
    }
}