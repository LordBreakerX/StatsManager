using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public abstract class StatsListItem<TData> : VisualElement
    {
        protected TextField NameTextField { get; private set; }
        protected Label NameLabel { get; private set; }

        protected TData Data { get; private set; }

        protected StatsEditorWindow ParentWindow { get; private set; }
        protected ListView ParentView { get; private set; }

        public StatsListItem(StatsEditorWindow parentWindow, ListView parentView)
        {
            ParentWindow = parentWindow;
            ParentView = parentView;

            style.justifyContent = Justify.Center;

            NameTextField = new TextField();
            NameTextField.style.display = DisplayStyle.Flex;

            NameLabel = new Label();
            NameLabel.style.paddingLeft = 5;
            NameLabel.style.paddingRight = 5;
            NameLabel.style.display = DisplayStyle.None;

            Add(NameTextField);
            Add(NameLabel);

            this.AddManipulator(new ContextualMenuManipulator((evt) =>
            {
                evt.menu.AppendAction("Rename", RenameItem);
                evt.menu.AppendAction("Duplicate", DuplicateItem);
                evt.menu.AppendAction("Delete", DeleteItem);

                evt.menu.AppendSeparator();
            }));
        }

        protected abstract void DeleteItem(DropdownMenuAction action);

        protected abstract void DuplicateItem(DropdownMenuAction action);

        protected abstract void RenameItem(DropdownMenuAction action);

        public void BindData(TData data)
        {
            Data = data;
            OnBindData();
        }

        protected abstract void OnBindData();

        public void UnbindData()
        {
            OnUnbindData();
            Data = default;
        }

        protected abstract void OnUnbindData();

        public abstract void RegisterEvents();

        public abstract void UnregisterEvents();
    }
}
