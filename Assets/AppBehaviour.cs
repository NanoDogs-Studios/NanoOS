using UnityEngine;

namespace NanoOS
{
    public class AppBehaviour : MonoBehaviour
    {
        public AppAsset AppAsset;

        public void OpenWindow()
        {
            if (AppAsset.WindowPrefab == null)
            {
                Debug.LogError("Please set your App's Window Prefab!");
            }
            GameObject window = GameObject.Instantiate(AppAsset.WindowPrefab, Utils.GetOSCanvas().transform.Find("Windows"));
            window.name = AppAsset.name + " (Window)";
            WindowAssociations windowAssociations = window.GetComponent<WindowAssociations>();
            if(windowAssociations != null)
            {
                windowAssociations.associatedApp = AppAsset;
            }
            else
            {
                Debug.LogError("Please add WindowAssociations to your App's Window Prefab!");
            }
        }
        
        public void CloseWindow()
        {

        }
    }

}