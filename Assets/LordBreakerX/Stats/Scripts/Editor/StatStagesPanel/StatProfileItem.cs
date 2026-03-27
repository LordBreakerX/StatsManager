using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public sealed class StatProfileItem : StatsListItem<StatProfile>
    {
        public StatProfileItem(StatsEditorPanel parentPanel, ListView parentView) : base(parentPanel, parentView)
        {
        }

        protected sealed override void OnBindData()
        {
            List<StatProfile> profiles = ParentWindow.Asset.Profiles;

            if (Data.ID == "")
            {
                int index =  profiles.IndexOf(Data);
                Data.name = $"Stat Profile {index}";
                EditorUtility.SetDirty(Data);
                EditorUtility.SetDirty(ParentWindow.Asset);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                NameTextField.style.display = DisplayStyle.Flex;
            }
            else
            {
                NameLabel.style.display = DisplayStyle.Flex;
            }

            NameTextField.value = Data.name;
            NameLabel.text = Data.ID;
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
        }

        protected override void OnCreateContextMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Create Profile Template", (action) =>
            {
                if (Data != null)
                {
                    string templateName = $"{Data.name}_template";
                    string path = EditorUtility.SaveFilePanel("Save Profile Template", "Assets", templateName, "asset");

                    path = "Assets" + path.Substring(Application.dataPath.Length);

                    if (!string.IsNullOrEmpty(path)) 
                    { 
                        ParentWindow.CurrentToolbar.CreateTemplate(Data, path);
                        ParentWindow.CurrentToolbar.UpdateTemplatesMenu();
                    }
                }
            });
        }
    }
}
