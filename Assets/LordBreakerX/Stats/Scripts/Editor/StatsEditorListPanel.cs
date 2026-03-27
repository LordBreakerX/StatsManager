using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public abstract class StatsEditorListPanel<TElement> : StatsEditorPanel where TElement : class
    {
        private readonly Button _createButton;

        public ListView CurrentListView { get; private set; }

        public abstract string AddElementTitle { get; }

        public TElement SelectedItem { get; set; }

        public Button CreateButton { get => _createButton; }

        public StatsEditorListPanel(StatsEditorWindow parent) : base(parent)
        {
            IList itemsSource = GetItemsSource();

            // extend header with a add element button
            VisualElement spacer = new VisualElement();
            spacer.style.flexGrow = 1;

            AddToHeader(spacer);

            _createButton = new Button(CreateElement);
            _createButton.text = "+";
            _createButton.AddToClassList("plus-button");
            AddToHeader(_createButton);

            // extend the panel with a list view
            CurrentListView = new ListView(itemsSource);
            CurrentListView.reorderable = true;
            CurrentListView.makeItem = MakeElementItem;
            CurrentListView.destroyItem = DestroyStageItem;
            CurrentListView.bindItem = BindStageItem;
            CurrentListView.style.flexGrow = 1;

            CurrentListView.selectionChanged += OnSelectionChanged;
            CurrentListView.Rebuild();

            AddToPanel(CurrentListView);

            AddManipulatorToPanel(new ContextualMenuManipulator(CreateContextMenu));
        }

        protected void OnSelectionChanged(IEnumerable<object> enumerable)
        {
            if (CurrentListView.selectedItem == null) return;

            if (CurrentListView.selectedItem is TElement selectedElement)
            {
                SelectedItem = selectedElement;
                ParentWindow.UpdatePanels();
            }
        }

        private void BindStageItem(VisualElement element, int index)
        {
            if (element is StatsListItem<TElement> listItem)
            {
                List<TElement> source = GetItemsSource();
                listItem.BindData(source[index]);
            }
        }

        private void DestroyStageItem(VisualElement element)
        {
            if (element is StatsListItem<TElement> listItem)
            {
                listItem.UnregisterEvents();
            }
        }

        protected abstract StatsListItem<TElement> MakeElementItem();

        private void CreateContextMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction(AddElementTitle, (action) =>
            {
                CreateElement();
            });
        }

        protected abstract List<TElement> GetItemsSource();

        protected abstract void CreateElement();
    }
}
