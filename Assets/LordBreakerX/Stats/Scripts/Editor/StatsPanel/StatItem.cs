using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatItem : StatsListItem<Stat>
    {
        public StatItem(StatsEditorPanel parentPanel, ListView parentView) : base(parentPanel, parentView)
        {
        }

        protected override void OnBindData()
        {
            StatsPanel statsPanel = ParentPanel as StatsPanel;

            StatProfile profile = ParentWindow.CurrentProfilePanel.SelectedItem;

            List<Stat> stats = profile.Stats;

            if (Data.GetId() == "")
            {
                int index = stats.IndexOf(Data);
                Data.SetId($"Stat Profile {index}");
                NameTextField.value = Data.GetId();

                EditorUtility.SetDirty(profile);
                EditorUtility.SetDirty(ParentWindow.Asset);

                NameTextField.style.display = DisplayStyle.Flex;
            }
            else
            {
                NameLabel.style.display = DisplayStyle.Flex;
            }

            NameTextField.value = Data.GetId();
            NameLabel.text = Data.GetId();
        }

        protected override void OnUnbindData()
        {

        }

        protected override void DeleteItem(DropdownMenuAction action)
        {
            StatProfile profile = ParentPanel.ParentWindow.CurrentProfilePanel.SelectedItem;

            if (profile == null) return;

            List<Stat> stats = profile.Stats;

            if (Data != null && stats.Contains(Data))
            {
                stats.Remove(Data);

                ParentView.RefreshItems();
                EditorUtility.SetDirty(ParentWindow.Asset);
                EditorUtility.SetDirty(profile);
            }
        }

        protected override void DuplicateItem(DropdownMenuAction action)
        {
            StatProfile profile = ParentPanel.ParentWindow.CurrentProfilePanel.SelectedItem;

            if (profile == null) return;

            List<Stat> stats = profile.Stats;

            if (Data != null && stats.Contains(Data))
            {
                string itemName = NameLabel.text;
                int nextIndex = stats.IndexOf(Data) + 1;
                stats.Insert(nextIndex, Data);
                ParentView.RefreshItems();

                EditorUtility.SetDirty(profile);
            }
        }

        protected override void RenameItem(DropdownMenuAction action)
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
            StatProfile profile = ParentPanel.ParentWindow.CurrentProfilePanel.SelectedItem;

            if (profile != null)
            {
                int statIndex = profile.Stats.IndexOf(Data);
                profile.Stats[statIndex].SetId(NameTextField.value);

                EditorUtility.SetDirty(profile);
            }

            NameTextField.style.display = DisplayStyle.None;
            NameLabel.style.display = DisplayStyle.Flex;

            ParentWindow.UpdatePanels();

            EditorUtility.SetDirty(ParentWindow.Asset);

            ParentView.RefreshItems();
        }

        protected override void OnCreateContextMenu(ContextualMenuPopulateEvent evt)
        {
            
        }
    }
}
