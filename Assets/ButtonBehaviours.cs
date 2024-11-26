using System;
using UnityEngine;

public class ButtonBehaviours : MonoBehaviour
{
    public GameObject Window;
    public void Close()
    {
        Destroy(Window);
        Debug.Log("Closed " + Window.name);
    }

    public void Minimize()
    {
        Window.SetActive(false);
        Debug.Log("Minimzed " + Window.name);
    }

    public void Fullscreen()
    {
        throw new NotImplementedException();
    }
}
