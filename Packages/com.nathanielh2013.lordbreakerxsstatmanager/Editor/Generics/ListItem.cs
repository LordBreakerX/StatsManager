using UnityEditor;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public abstract class ListItem<TData> : VisualElement
    {
        private const string UI_PATH = "Packages/com.nathanielh2013.lordbreakerxsstatmanager/Editor/Generics/ListItem.uxml";

        private TextField _idTextField;
        private Label _idNameLabel;

        private TData _data;

        protected TData Data { get => _data; }

        public ListItem()
        {
            VisualTreeAsset uiTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UI_PATH);
            uiTree.CloneTree(this);

            //style.justifyContent = Justify.Center;

            //_idTextField = new TextField();
            //_idTextField.style.display = DisplayStyle.None;

            //_idNameLabel = new Label();
            //_idNameLabel.style.paddingLeft = 5;
            //_idNameLabel.style.paddingRight = 5;
            //_idNameLabel.style.display = DisplayStyle.None;

            //Add(_idTextField);
            //Add(_idNameLabel);

            this.AddManipulator(new ContextualMenuManipulator((evt) =>
            {
                evt.menu.AppendAction("Rename", RenameItem);
                evt.menu.AppendAction("Duplicate", DuplicateItem);
                evt.menu.AppendAction("Delete", DeleteItem);
                evt.menu.AppendSeparator();
            }));
        }

        public virtual void SetData(TData data, string id)
        {
            _data = data;
            _idTextField.value = id;
            _idNameLabel.text = id;
        }
        
        protected abstract void DeleteItem(DropdownMenuAction action);

        protected abstract void DuplicateItem(DropdownMenuAction action);

        protected abstract void RenameItem(DropdownMenuAction action);
    }
}
