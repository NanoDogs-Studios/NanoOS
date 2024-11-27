using NanoOS;
using UnityEngine.Networking;
using UnityEngine;
using System.Collections;

public class GoogleSearch : MonoBehaviour
{
    private readonly string apiKey = "AIzaSyD7zDiCRpMEe86idMsbTSoB1lChyLYEVTk";  // Replace with your API key
    private readonly string searchEngineId = "717e2725a0e9642e4";  // Replace with your CSE ID

    public NanoBrowser browser;  // Reference to the NanoBrowser to call ProcessSearchResults

    void Start()
    {
        browser = GetComponent<NanoBrowser>();
    }

    public IEnumerator Search(string query)
    {
        string url = $"https://www.googleapis.com/customsearch/v1?q={query}&key={apiKey}&cx={searchEngineId}";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(www.downloadHandler.text);
                // Pass the JSON response to the ProcessSearchResults method of NanoBrowser
                browser.ProcessSearchResults(www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
    }

    [System.Serializable]
    public class GoogleSearchResults
    {
        public GoogleSearchItem[] items;
    }

    [System.Serializable]
    public class GoogleSearchItem
    {
        public string title;
        public string link;
    }
}