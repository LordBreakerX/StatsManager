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

            Add(spacerElement);

            style.borderBottomWidth = 2;
        }
    }
}
