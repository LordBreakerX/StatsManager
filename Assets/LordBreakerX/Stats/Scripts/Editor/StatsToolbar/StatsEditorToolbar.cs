using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsEditorToolbar : Toolbar
    {
        public StatsEditorToolbar()
        {
            VisualElement spacerElement = new VisualElement();
            spacerElement.style.flexGrow = 1;

            ToolbarButton saveAssetButton = new ToolbarButton();
            saveAssetButton.text = "Save Asset";

            ToolbarButton autoSaveButton = new ToolbarButton();
            autoSaveButton.text = "Auto-Save";

            Add(spacerElement);
            Add(saveAssetButton);
            Add(autoSaveButton);

            style.borderBottomWidth = 2;
        }
    }
}
