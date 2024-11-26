using UnityEngine;
using UnityEngine.Events;

namespace NanoOS
{
    [CreateAssetMenu(fileName = "New App Asset", menuName = "NanoOS/App Asset", order = 0)]
    public class AppAsset : ScriptableObject
    {
        public bool UseAssetNameAsAppName;
        public string AppName;
        public string AppVersion;
        public string AppDescription;
        public Texture2D Icon;

        public GameObject WindowPrefab;

        // Add a UnityEvent to allow custom behavior
        public UnityEvent OnOpenEvent;

        private void Awake()
        {
            if (UseAssetNameAsAppName)
                AppName = this.name;
        }

        // Trigger the event when the app is opened
        public void OnOpen()
        {
            Debug.Log(AppName + " opened.");
            OnOpenEvent?.Invoke(); // Invoke the custom event

            AppBehaviour behaviour = GameObject.Find("App Behaviour").GetComponent<AppBehaviour>();
            behaviour.AppAsset = this;
            behaviour.OpenWindow();

        }
    }

}