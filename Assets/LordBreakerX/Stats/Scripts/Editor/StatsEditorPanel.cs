using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsEditorPanel : VisualElement
    {
        private VisualElement _headerContainer;

        private VisualElement _panelContainer;

        public StatsEditorWindow ParentWindow { get; private set; }

        public virtual string HeaderText { get { return GetType().Name; } }

        public StatsEditorPanel(StatsEditorWindow parent)
        {
            ParentWindow = parent;

            // creating the header
            _headerContainer = new VisualElement();
            _headerContainer.AddToClassList("panel-header");
            Label panelLabel = new Label(HeaderText);

            _headerContainer.Add(panelLabel);

            Add(_headerContainer);

            _panelContainer = new VisualElement();
            _panelContainer.style.flexGrow = 1;

            Add(_panelContainer);

        }

        public void AddToHeader(VisualElement elementToAdd)
        {
            _headerContainer.Add(elementToAdd);
        }

        public void AddToPanel(VisualElement elementToAdd)
        {
            _panelContainer.Add(elementToAdd);
        }

        public void AddManipulatorToPanel(IManipulator manipulator)
        {
            _panelContainer.AddManipulator(manipulator);
        }

        public virtual void UpdatePanel()
        {

        }

    }
}
