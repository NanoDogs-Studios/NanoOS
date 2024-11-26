using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

namespace NanoOS
{
    public class OSManager : MonoBehaviour
    {
        public bool VerboseLogging = true;
        public List<AppAsset> apps = new List<AppAsset>();

        public Transform appsGrid;
        public GameObject AppTemplate;

        public Transform taskbar;
        public GameObject appTaskbarTemplate;

        private Dictionary<WindowAssociations, GameObject> trackedWindows = new Dictionary<WindowAssociations, GameObject>();

        private void Start()
        {
            foreach (var app in apps)
            {
                GameObject appGO = Instantiate(AppTemplate, appsGrid);
                appGO.name = app.AppName;
                appGO.SetActive(true);
                appGO.GetComponent<App>().AppAsset = app;

            }
        }

        private void Update()
        {
            // Get all windows under "Windows," including inactive ones
            var windows = new HashSet<WindowAssociations>(transform.Find("Windows").GetComponentsInChildren<WindowAssociations>(true));

            // Remove taskbar entries for windows that no longer exist
            var toRemove = new List<WindowAssociations>();
            foreach (var entry in trackedWindows)
            {
                if (!windows.Contains(entry.Key)) // Check if the window still exists
                {
                    Destroy(entry.Value); // Destroy the taskbar object
                    toRemove.Add(entry.Key); // Mark for removal from the dictionary
                }
            }
            foreach (var window in toRemove)
            {
                trackedWindows.Remove(window);
            }

            // Add taskbar entries for new windows
            foreach (var window in windows)
            {
                if (!trackedWindows.ContainsKey(window))
                {
                    if (VerboseLogging)
                        Debug.Log("Windows Found: " + window.name);

                    // Add the window to the taskbar
                    GameObject windowTaskbar = GameObject.Instantiate(appTaskbarTemplate, taskbar);
                    windowTaskbar.name = window.associatedApp.AppName;
                    windowTaskbar.GetComponent<RawImage>().texture = window.associatedApp.Icon;

                    // Add the onClick listener to enable the window
                    windowTaskbar.GetComponent<Button>().onClick.AddListener(() => window.gameObject.SetActive(true));

                    // Track the window and its taskbar entry
                    trackedWindows.Add(window, windowTaskbar);
                }
            }
        }
    }


    /// <summary>
    /// Various Utilities for NanoOS.
    /// </summary>
    public class Utils : MonoBehaviour
    {
        /// <summary>
        /// Gets the OS Canvas via Name, if that fails, then it uses tag.
        /// </summary>
        /// <returns></returns>
        public static GameObject GetOSCanvas()
        {
            GameObject canvas = GameObject.Find("OS Canvas");
            if(canvas == null)
            {
                canvas = GameObject.FindGameObjectWithTag("OS Canvas");
                if(canvas == null)
                {
                    Debug.LogError("Could not find OS Canvas in Scene, verify it is present in the currently loaded scene. Returning null.");
                    return null;
                }
                else
                {
                    return canvas;
                }
            }
            else
            {
                return canvas;
            }
        }

        public static GameObject GetDesktop()
        {
            GameObject desktop = GetOSCanvas().transform.Find("Desktop").gameObject;
            if (desktop == null)
            {
                Debug.LogError("Could not find Desktop in OS Canvas, verify it is present in the OS Canvas. Returning null.");
                return null;
            }
            else
            {
                return desktop;
            }
        }

        public static Texture2D TextureField(string name, Texture2D texture)
        {
            GUILayout.BeginVertical();
            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.UpperCenter;
            style.fixedWidth = 70;
            GUILayout.Label(name, style);
            var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
            GUILayout.EndVertical();
            return result;
        }
    }

}

