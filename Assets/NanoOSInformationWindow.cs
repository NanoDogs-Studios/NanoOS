using TMPro;
using UnityEngine;

public class NanoOSInformationWindow : MonoBehaviour
{
    public TMP_Text VerText;
    public TMP_Text UnityVerText;

    private void Start()
    {
        VerText.text = "Nano OS V" + Application.version;
        UnityVerText.text = "Unity V" + Application.unityVersion;
    }
}
