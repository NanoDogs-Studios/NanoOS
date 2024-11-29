using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class NanoPlayer : MonoBehaviour
{
    public GameObject trackPrefab; // Prefab for UI track display
    public Transform trackList; // Parent container for track UI elements

    public AudioSource Audio;
    public List<AudioClip> clips = new List<AudioClip>();

    public UISkin FileBrowserSkin;
    private string filePath;

    public void PickFile()
    {
        FileBrowser.Skin = FileBrowserSkin;
        FileBrowser.Filter audioFilter = new FileBrowser.Filter("Audio Files", ".mp3", ".wav", ".ogg");
        FileBrowser.SetDefaultFilter("Audio Files");
        FileBrowser.ShowLoadDialog(OnSuccess, OnCancel, SimpleFileBrowser.FileBrowser.PickMode.Files, false);
    }

    void OnSuccess(string[] filePaths)
    {
        foreach (string path in filePaths)
        {
            Debug.Log($"Loading file: {path}");
            StartCoroutine(LoadSongCoroutine(path));
        }
    }

    public void OnCancel()
    {
        Debug.Log("File selection canceled.");
    }

    IEnumerator LoadSongCoroutine(string path)
    {
        string url = $"file://{path}";
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                clips.Add(clip); // Add the clip to the list
                AddTrackToUI(clip, path); // Add the clip to the UI
            }
            else
            {
                Debug.LogError($"Failed to load song from {url}: {request.error}");
            }
        }
    }

    private void AddTrackToUI(AudioClip clip, string path)
    {
        // Instantiate the track UI prefab
        GameObject trackUI = Instantiate(trackPrefab, trackList);

        // Update the track UI with song info
        TrackUI trackUIComponent = trackUI.GetComponent<TrackUI>();
        if (trackUIComponent != null)
        {
            trackUIComponent.SetTrackInfo(GetSongNameFromPath(path), Audio, clip);
        }

        // Optionally, add a button to play the track
        trackUI.GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            PlayClip(clip);
        });
    }

    private void PlayClip(AudioClip clip)
    {
        Audio.clip = clip;
        Audio.Play();
    }

    private string GetSongNameFromPath(string path)
    {
        return Path.GetFileNameWithoutExtension(path);
    }

    public string GetArtistNameFromSongTitle()
    {
        string songTitle = GetSongName();
        if (string.IsNullOrEmpty(songTitle))
        {
            return null;
        }

        int hyphenIndex = songTitle.IndexOf('-');
        if (hyphenIndex > 0)
        {
            return songTitle.Substring(0, hyphenIndex).Trim();
        }

        return null;
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

    public string GetSongTitleWithoutArtist()
    {
        string songTitle = GetSongName();
        if (string.IsNullOrEmpty(songTitle))
        {
            return null;
        }

        int hyphenIndex = songTitle.IndexOf('-');
        if (hyphenIndex >= 0 && hyphenIndex < songTitle.Length - 1)
        {
            return songTitle.Substring(hyphenIndex + 1).Trim();
        }

        return songTitle;
    }
}
