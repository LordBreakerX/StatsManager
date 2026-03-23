using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public sealed class StatProfileItem : StatsListItem<StatProfile>
    {
        private bool _isRenaming = true;

        public StatProfileItem(StatsEditorWindow parentWindow, ListView parentView) : base(parentWindow, parentView)
        {
        }

        protected sealed override void OnBindData()
        {
            NameTextField.value = Data.name;
            NameLabel.text = Data.name;

            if (_isRenaming)
            {
                NameLabel.style.display = DisplayStyle.None;
                NameTextField.style.display = DisplayStyle.Flex;
            }
            else
            {
                NameLabel.style.display = DisplayStyle.Flex;
                NameTextField.style.display = DisplayStyle.None;
            }
        }

        protected sealed override void OnUnbindData()
        {
            
        }

        protected sealed override void DeleteItem(DropdownMenuAction action)
        {
            List<StatProfile> profiles = ParentWindow.Asset.Profiles;

            if (Data != null && profiles.Contains(Data))
            {
                profiles.Remove(Data);

                AssetDatabase.RemoveObjectFromAsset(Data);
                Object.DestroyImmediate(Data, true);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                ParentView.RefreshItems();
                EditorUtility.SetDirty(ParentWindow.Asset);
            }
        }

        protected sealed override void DuplicateItem(DropdownMenuAction action)
        {
            List<StatProfile> profiles = ParentWindow.Asset.Profiles;

            if (Data != null && profiles.Contains(Data))
            {
                string itemName = NameLabel.text;
                int nextIndex = profiles.IndexOf(Data) + 1;
                profiles.Insert(nextIndex, Data);
                ParentView.RefreshItems();
            }
        }

        protected sealed override void RenameItem(DropdownMenuAction action)
        {
            NameTextField.style.display = DisplayStyle.Flex;
            NameLabel.style.display = DisplayStyle.None;
            NameTextField.Focus();
        }

        public override void RegisterEvents()
        {
            NameTextField.RegisterCallback<BlurEvent>(OnBlur);
        }

        public override void UnregisterEvents()
        {
            NameTextField.UnregisterCallback<BlurEvent>(OnBlur);
        }

        private void OnBlur(BlurEvent evt)
        {
            int stageIndex = ParentWindow.Asset.Profiles.IndexOf(Data);
            ParentWindow.Asset.Profiles[stageIndex].name = NameTextField.value;

            NameTextField.style.display = DisplayStyle.None;
            NameLabel.style.display = DisplayStyle.Flex;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.SetDirty(ParentWindow.Asset);

            ParentView.RefreshItems();

            _isRenaming = false;
        }
    }
}
