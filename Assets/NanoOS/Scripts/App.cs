using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace NanoOS
{
    public class App : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public AppAsset AppAsset;
        public TMP_Text AppNameText;

        public Vector2 gridSize = new Vector2(50f, 50f); // Set grid cell size (e.g., 50x50)
        private RectTransform rectTransform;
        private Canvas canvas;
        private CanvasGroup canvasGroup;
        private Vector2 lastMousePosition; // To track mouse movement delta
        private RectTransform canvasRectTransform;

        // This list will store references to other draggable objects
        public static List<App> allDraggableApps = new List<App>();

        private void Start()
        {
            this.gameObject.GetComponent<RawImage>().texture = AppAsset.Icon;
            AppNameText.text = AppAsset.AppName;

            // adding a delegate with parameters
            this.gameObject.GetComponent<Button>().onClick.AddListener(delegate { AppAsset.OnOpen(); });

            rectTransform = GetComponent<RectTransform>();
            canvas = Utils.GetOSCanvas().GetComponent<Canvas>();
            canvasRectTransform = canvas.GetComponent<RectTransform>(); // Reference to canvas' RectTransform
            canvasGroup = GetComponent<CanvasGroup>();

            // Register this object to the list of draggable objects
            if (!allDraggableApps.Contains(this))
            {
                allDraggableApps.Add(this);
            }

            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Make the object draggable and ignore raycasts temporarily
            canvasGroup.alpha = 0.8f;
            canvasGroup.blocksRaycasts = false;

            // Store the initial mouse position
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out lastMousePosition);
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Calculate the mouse delta
            Vector2 mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out mousePosition);

            // Calculate delta and apply it to the object's position
            Vector2 delta = mousePosition - lastMousePosition;
            rectTransform.anchoredPosition += delta;
            lastMousePosition = mousePosition; // Update the last mouse position

            // Clamp the position to prevent the UI object from going off the screen
            ClampToScreenBounds();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Snap to grid when drag ends
            Vector2 newPos = SnapToGrid(rectTransform.anchoredPosition);

            // Check if the new position causes an overlap with other draggable objects
            if (!IsPositionOccupied(newPos))
            {
                rectTransform.anchoredPosition = newPos;
            }
            else
            {
                // Adjust the position if overlap occurs (or you can handle this differently)
                rectTransform.anchoredPosition = AdjustPositionToAvoidOverlap(newPos);
            }

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        private Vector2 SnapToGrid(Vector2 position)
        {
            float snappedX = Mathf.Round(position.x / gridSize.x) * gridSize.x;
            float snappedY = Mathf.Round(position.y / gridSize.y) * gridSize.y;
            return new Vector2(snappedX, snappedY);
        }

        private void ClampToScreenBounds()
        {
            // Get the min and max x/y positions that the object can have to stay within the screen
            Vector2 min = canvasRectTransform.rect.min;
            Vector2 max = canvasRectTransform.rect.max;

            // Clamp the position to stay within the canvas bounds
            rectTransform.anchoredPosition = new Vector2(
                Mathf.Clamp(rectTransform.anchoredPosition.x, min.x, max.x),
                Mathf.Clamp(rectTransform.anchoredPosition.y, min.y, max.y)
            );
        }

        private bool IsPositionOccupied(Vector2 position)
        {
            // Check if this position collides with any other object in the allDraggableApps list
            foreach (var app in allDraggableApps)
            {
                if (app != this)
                {
                    // Assuming each app's RectTransform position is snapped to the grid as well
                    if (Vector2.Distance(app.rectTransform.anchoredPosition, position) < gridSize.x * 0.8f)
                    {
                        return true; // Overlap detected
                    }
                }
            }
            return false;
        }

        private Vector2 AdjustPositionToAvoidOverlap(Vector2 position)
        {
            // Adjust the position by finding the nearest available spot
            // Here you can implement a custom logic to adjust position based on your requirements
            foreach (var app in allDraggableApps)
            {
                if (app != this)
                {
                    // Look for the closest empty grid position
                    Vector2 directionToMove = position - app.rectTransform.anchoredPosition;
                    if (directionToMove.magnitude < gridSize.x * 0.8f)
                    {
                        // You can adjust the position by moving away from the overlap
                        position += directionToMove.normalized * gridSize.x; // Move to the next available grid cell
                    }
                }
            }
            return position;
        }
    }
}
