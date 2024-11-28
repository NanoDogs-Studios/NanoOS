using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OSPanel : MonoBehaviour
{
    public GameObject OptionTemplate;
    public Transform optionList;

    /// <summary>
    /// Adds an option to the OS Menu. (example: AddOption("My Name", MyFunction);)
    /// </summary>
    /// <param name="Name">The name of the Option.</param>
    /// <param name="clickAction">What will happen when the option is clicked.</param>
    public void AddOption(string Name, UnityAction clickAction)
    {
        GameObject option = GameObject.Instantiate(OptionTemplate);
        option.name = Name;
        option.GetComponentInChildren<TMP_Text>().text = Name;
        option.transform.SetParent(optionList.transform, false);

        option.GetComponent<Button>().onClick.AddListener(clickAction);
    }
}
