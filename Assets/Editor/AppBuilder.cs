using UnityEngine;
using NanoOS;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEditor.VersionControl;

namespace NanoOS.Editor
{
    public class AppBuilder : EditorWindow
    {
        string AppNameInput = "App Name";
        string AppVerInput = "App Version";
        string AppDescInput = "App Description";
        Texture2D appIcon = null;

        [MenuItem("NanoOS/App Builder")]
        public static void ShowExample()
        {
            AppBuilder wnd = GetWindow<AppBuilder>();
            wnd.titleContent = new GUIContent("App Builder");
        }

        private void OnGUI()
        {
            AppNameInput = EditorGUILayout.TextField("App Name", AppNameInput);
            AppVerInput = EditorGUILayout.TextField("App Version", AppVerInput);
            AppDescInput = EditorGUILayout.TextField("App Description", AppDescInput);
            appIcon = NanoOS.Utils.TextureField("App Icon", appIcon);

            if (GUILayout.Button("Create App"))
            {
                AppAsset newAppA = ScriptableObject.CreateInstance<AppAsset>();
                newAppA.AppName = AppNameInput;
                newAppA.AppVersion = AppVerInput;
                newAppA.AppDescription = AppDescInput;
                newAppA.Icon = appIcon;

                // add the function 


                AssetDatabase.CreateAsset(newAppA, "Assets/" + newAppA.AppName + ".asset");
                AssetDatabase.SaveAssets();

                EditorUtility.FocusProjectWindow();

                Selection.activeObject = newAppA;

            }
        }
    }
}
