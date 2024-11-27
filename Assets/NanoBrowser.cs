using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace NanoOS
{
    public class NanoBrowser : MonoBehaviour
    {
        public TMP_InputField searchInputField;
        public Button searchButton;
        public Transform resultsContainer;  // Container to hold search result items
        public GameObject resultItemPrefab; // Prefab for each search result item

        void Start()
        {
            searchButton.onClick.AddListener(OnSearchButtonClicked);
        }

        void OnSearchButtonClicked()
        {
            string query = searchInputField.text;
            StartCoroutine(this.GetComponent<GoogleSearch>().Search(query));
        }

        public void ProcessSearchResults(string json)
        {
            GoogleSearch.GoogleSearchResults results = JsonUtility.FromJson<GoogleSearch.GoogleSearchResults>(json);

            // Clear previous results before adding new ones
            foreach (Transform child in resultsContainer)
            {
                Destroy(child.gameObject);
            }

            // Instantiate result items for each search result
            foreach (var item in results.items)
            {
                GameObject resultItem = Instantiate(resultItemPrefab, resultsContainer);

                // Get the TMP_Text component from the instantiated result item
                TMP_Text resultText = resultItem.GetComponentInChildren<TMP_Text>();

                // Set the text of the TMP_Text component to the title of the search result
                if (resultText != null)
                {
                    resultText.text = item.title;
                }
            }
        }
    }

}
