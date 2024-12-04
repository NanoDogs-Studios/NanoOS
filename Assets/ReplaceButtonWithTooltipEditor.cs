using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ReplaceButtonWithTooltipEditor : EditorWindow
{
    [MenuItem("NanoOS/Replace Buttons with NanoOS Buttons")]
    public static void ReplaceButtons()
    {
        // Find all GameObjects in the scene
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);

        foreach (Button oldButton in buttons)
        {
            GameObject buttonGameObject = oldButton.gameObject;

            // Store old settings
            var oldOnClick = oldButton.onClick;
            var navigation = oldButton.navigation;
            var transition = oldButton.transition;
            var colors = oldButton.colors;
            var spriteState = oldButton.spriteState;
            var animationTriggers = oldButton.animationTriggers;

            // Replace the Button with NanoOSButton
            oldButton.enabled = false;
            NanoOSButton newButton = buttonGameObject.AddComponent<NanoOSButton>();

            // Restore settings
            newButton.onClick = oldOnClick;
            newButton.navigation = navigation;
            newButton.transition = transition;
            newButton.colors = colors;
            newButton.spriteState = spriteState;
            newButton.animationTriggers = animationTriggers;

            // Add a default tooltip text (can be customized later)
            newButton.tooltipText = "Default Tooltip Text";
        }

        Debug.Log($"Replaced {buttons.Length} Button(s) with TooltipButton(s).");
    }
}