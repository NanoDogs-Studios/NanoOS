using SimpleFileBrowser;
using System.Collections;
using System.IO;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Networking;

public class NanoPlayer : MonoBehaviour
{
    public AudioSource Audio;
    public AudioClip CurrentClip;

    public UISkin FileBrowserSkin;
    string filePath;

    public void PickFile()
    {
        FileBrowser.Skin = FileBrowserSkin;
        FileBrowser.Filter audioFilter = new FileBrowser.Filter("Audio Files", ".mp3", ".wav", ".ogg");
        FileBrowser.SetDefaultFilter("Audio Files");
        FileBrowser.ShowLoadDialog(OnSuccess, OnCancel, SimpleFileBrowser.FileBrowser.PickMode.Files, false);
    }
    void OnSuccess(string[] filePaths)
    {
        // Print paths of the selected files
        for (int i = 0; i < filePaths.Length; i++)
            Debug.Log(filePaths[i]);

        // Get the file path of the first selected file
        filePath = filePaths[0];

        StartCoroutine(LoadSongCoroutine(filePath));
    }

    public void OnCancel()
    {

    }

    IEnumerator LoadSongCoroutine(string path)
    {
        string url = $"file://{path}";
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                CurrentClip = DownloadHandlerAudioClip.GetContent(request);
                Audio.clip = CurrentClip;
                Audio.Play();
            }
            else
            {
                Debug.LogError($"Failed to load song from {url}: {request.error}");
                Invoke("ResetStatusText", 5f);
            }
        }
    }

    public string GetSongName()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("Invalid file path.");
            return null;
        }

        // Use Path.GetFileNameWithoutExtension to extract the file name without its extension
        return Path.GetFileNameWithoutExtension(filePath);
    }
    public string GetArtistNameFromSongTitle()
    {
        string songTitle = GetSongName();
        if (string.IsNullOrEmpty(songTitle))
        {
            return null;
        }

        // Check for the presence of a hyphen ('-')
        int hyphenIndex = songTitle.IndexOf('-');
        if (hyphenIndex > 0) // Ensure the hyphen is not at the start
        {
            // Extract and return the part before the hyphen
            return songTitle.Substring(0, hyphenIndex).Trim();
        }

        // If no hyphen is found, return null
        return null;
    }

    public string GetSongTitleWithoutArtist()
    {
        string songTitle = GetSongName();
        if (string.IsNullOrEmpty(songTitle))
        {
            return null;
        }

        // Check for the presence of a hyphen ('-')
        int hyphenIndex = songTitle.IndexOf('-');
        if (hyphenIndex >= 0 && hyphenIndex < songTitle.Length - 1) // Ensure hyphen is not at the end
        {
            // Extract and return the part after the hyphen
            return songTitle.Substring(hyphenIndex + 1).Trim();
        }

        // If no hyphen is found, return the full song title
        return songTitle;
    }
}
