using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsEditorPanel : VisualElement
    {
        public StatsEditorWindow ParentWindow { get; private set; }

        public StatsEditorPanel(string labelText, StatsEditorWindow parent)
        {
            ParentWindow = parent;

            // creating the header
            VisualElement header = new VisualElement();
            header.AddToClassList("panel-header");
            Label panelLabel = new Label(labelText);

            header.Add(panelLabel);
            
            OnExtendHeader(header);

            Add(header);

            VisualElement panelArea = new VisualElement();
            panelArea.style.flexGrow = 1;

            OnCreatePanelGUI(panelArea);

            Add(panelArea);
        }

        protected virtual void OnExtendHeader(VisualElement header)
        {

        }

        protected virtual void OnCreatePanelGUI(VisualElement root)
        {

        }

        protected virtual void OnEnablePanel()
        {

        }

        protected virtual void OnDisablePanel()
        {

        }
    }
}
