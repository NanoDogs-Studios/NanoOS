using NanoOS;
using SimpleFileBrowser;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static SimpleFileBrowser.FileBrowser;

public class NanoOSSettings : MonoBehaviour
{
    public RawImage preview;

    public UISkin FileBrowserSkin;
    string filePath;

    public void ChangeWallpaper(Texture2D texture)
    {
        // update preview
        preview.texture = texture;
        NanoOS.Utils.GetOSCanvas().GetComponent<OSManager>().currentWallpaper = texture;
        Debug.Log("Set wallpaper to: " + texture.name);
    }
    
    public void FilePromptChangeWallpaper()
    {
        FileBrowser.Skin = FileBrowserSkin;
        FileBrowser.Filter audioFilter = new FileBrowser.Filter("Picture Files", ".png", ".jpg", ".jpeg");
        FileBrowser.SetDefaultFilter("Picture Files");
        FileBrowser.ShowLoadDialog(OnSuccess, OnCancel, SimpleFileBrowser.FileBrowser.PickMode.Files, false);
    }

    void OnSuccess(string[] filePaths)
    {
        // Print paths of the selected files
        for (int i = 0; i < filePaths.Length; i++)
            Debug.Log(filePaths[i]);

        // Get the file path of the first selected file
        filePath = filePaths[0];

        StartCoroutine(LoadTextureCoroutine(filePath));
    }

    public void OnCancel()
    {

    }

    IEnumerator LoadTextureCoroutine(string path)
    {
        string url = $"file://{path}";
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                ChangeWallpaper(DownloadHandlerTexture.GetContent(request));
            }
            else
            {
                Debug.LogError($"Failed to load texture from {url}: {request.error}");
            }
        }
    }

}
