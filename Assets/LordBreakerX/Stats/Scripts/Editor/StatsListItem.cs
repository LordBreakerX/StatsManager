using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsListItem<TData> : VisualElement
    {
        protected TextField NameTextField { get; private set; }
        protected Label NameLabel { get; private set; }

        protected TData Data { get; private set; }
        protected List<TData> DataSource { get; private set; }

        protected ListView ParentView { get; private set; }

        public StatsListItem(List<TData> dataSource, ListView parentView)
        {
            DataSource = dataSource;
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

        protected virtual void RenameItem(DropdownMenuAction action)
        {
            NameTextField.style.display = DisplayStyle.Flex;
            NameLabel.style.display = DisplayStyle.None;
            NameTextField.Focus();
        }

        protected virtual void DuplicateItem(DropdownMenuAction action)
        {
            if (Data != null && DataSource.Contains(Data))
            {
                string itemName = NameLabel.text;
                int nextIndex = DataSource.IndexOf(Data) + 1;
                DataSource.Insert(nextIndex, Data);
                ParentView.RefreshItems();
            }
        }

        protected virtual void DeleteItem(DropdownMenuAction action)
        {
            if (Data != null && DataSource.Contains(Data))
            {
                DataSource.Remove(Data);
                ParentView.RefreshItems();
            }
        }

        public void BindData(TData data) 
        {
            
        }

        public void UnbindData()
        {

        }

    }
}
