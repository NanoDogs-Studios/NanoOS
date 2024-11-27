using EZCameraShake;
using UnityEngine;

public class BPMManager : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip previousClip;
    private float timeBetweenBeats;
    private float nextBeatTime;

    // Camera shake base parameters
    [Header("Camera Shake Settings")]
    public float baseShakeMagnitude = 2f; // Base magnitude
    public float baseShakeRoughness = 2f; // Base roughness
    public float maxShakeMultiplier = 3f; // Max multiplier for high BPM
    public float shakeFadeIn = 0.1f; // Fade-in time
    public float shakeFadeOut = 0.2f; // Fade-out time
    public float dividen = 100f;

    private float currentShakeMagnitude;
    private float currentShakeRoughness;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component is missing!");
            enabled = false;
        }
    }

    private void Update()
    {
        // Detect if the clip has changed
        if (audioSource.clip != previousClip)
        {
            OnClipChanged();
        }

        // Handle camera shake based on beats
        if (audioSource.isPlaying && timeBetweenBeats > 0 && Time.time >= nextBeatTime)
        {
            CameraShaker.Instance.ShakeOnce(currentShakeMagnitude, currentShakeRoughness, shakeFadeIn, shakeFadeOut);
            nextBeatTime = Time.time + timeBetweenBeats;
        }
    }

    private void OnClipChanged()
    {
        previousClip = audioSource.clip;

        if (audioSource.clip != null)
        {
            // Calculate BPM once when the clip changes
            int bpm = UniBpmAnalyzer.AnalyzeBpm(audioSource.clip);

            if (bpm > 0)
            {
                timeBetweenBeats = 60f / bpm; // Time in seconds between each beat
                nextBeatTime = Time.time; // Reset beat timing

                // Scale shake magnitude and roughness based on BPM
                float bpmMultiplier = Mathf.Clamp((float)bpm / dividen, 1f, maxShakeMultiplier); // Scale between 1x and max multiplier
                currentShakeMagnitude = baseShakeMagnitude * bpmMultiplier;
                currentShakeRoughness = baseShakeRoughness * bpmMultiplier;

                Debug.Log($"New clip detected. BPM: {bpm}, Shake Magnitude: {currentShakeMagnitude}, Shake Roughness: {currentShakeRoughness}");
            }
            else
            {
                Debug.LogWarning("Failed to analyze BPM for the new clip.");
                timeBetweenBeats = 0; // Disable beat-based functionality
            }
        }
        else
        {
            timeBetweenBeats = 0; // No clip means no beats
        }
    }
}
