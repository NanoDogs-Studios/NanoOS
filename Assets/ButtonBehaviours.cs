using System;
using UnityEngine;

namespace NanoOS
{
    public class ButtonBehaviours : MonoBehaviour
    {
        public GameObject Window;
        public void Close()
        {
            Destroy(Window);
            Debug.Log("Closed " + Window.name);
        }

        public void Fullscreen()
        {
            throw new NotImplementedException();
        }

        public void Minimize()
        {
            Window.SetActive(false);
            Debug.Log("Minimzed " + Window.name);
        }
    }


}