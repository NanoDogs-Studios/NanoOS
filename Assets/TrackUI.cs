using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrackUI : MonoBehaviour
{
    public TMP_Text trackNameText;
    //public TMP_Text artistText;

    public AudioSource source1;

    public void SetTrackInfo(string trackName, AudioSource source, AudioClip clip)
    {
        source1 = source;
        trackNameText.text = trackName;
    }

    public void Play()
    {
        source1.Play();
    }
}
