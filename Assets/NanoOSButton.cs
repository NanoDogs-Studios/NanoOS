using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class NanoOSButton : Button
{
    [Tooltip("The text to display in the tooltip.")]
    public string tooltipText;

    private GameObject tooltipObject;

    protected override void Start()
    {
        base.Start();
        CreateTooltip();
    }

    private void CreateTooltip()
    {
        // Create the tooltip GameObject
        tooltipObject = new GameObject("Tooltip");
        tooltipObject.transform.SetParent(transform.root); // Add to the root canvas
        tooltipObject.transform.localScale = Vector3.one; // Ensure correct scale
        tooltipObject.transform.position = Vector3.zero;
        tooltipObject.SetActive(false);

        // Add components to the tooltip
        var canvasRenderer = tooltipObject.AddComponent<CanvasRenderer>();
        var image = tooltipObject.AddComponent<Image>();
        image.color = new Color(0, 0, 0, 0.8f); // Semi-transparent black background

        var textObject = new GameObject("TooltipText");
        textObject.transform.SetParent(tooltipObject.transform);
        textObject.transform.localScale = Vector3.one; // Ensure correct scale for text
        textObject.transform.position = Vector3.zero;
        var text = textObject.AddComponent<TextMeshProUGUI>();
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 12; // Smaller font size
        text.text = tooltipText;

        // Position and size the tooltip
        var tooltipRect = tooltipObject.AddComponent<RectTransform>();
        tooltipRect.sizeDelta = new Vector2(120, 40); // Smaller tooltip size
        tooltipRect.pivot = new Vector2(0.5f, 0);

        var textRect = textObject.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(110, 30); // Smaller text area
        textRect.pivot = new Vector2(0.5f, 0.5f);
        textRect.anchoredPosition = Vector2.zero;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        ShowTooltip(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        HideTooltip();
    }

    private void ShowTooltip(PointerEventData eventData)
    {
        if (tooltipObject == null) return;

        tooltipObject.SetActive(true);
        var tooltipRect = tooltipObject.GetComponent<RectTransform>();
        tooltipRect.position = eventData.position; // Position tooltip near mouse
    }

    private void HideTooltip()
    {
        if (tooltipObject == null) return;
        tooltipObject.SetActive(false);
    }
}