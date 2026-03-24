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

            if (statsPanel.CurrentProfile == null) return;

            List<Stat> stats = statsPanel.CurrentProfile.Stats;

            if (Data.GetId() == "")
            {
                int index = stats.IndexOf(Data);
                Data.SetId($"Stat Profile {index}");
                NameTextField.value = Data.GetId();
                EditorUtility.SetDirty(statsPanel.CurrentProfile);
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
            StatsPanel statsPanel = ParentPanel as StatsPanel;

            if (statsPanel.CurrentProfile == null) return;

            List<Stat> stats = statsPanel.CurrentProfile.Stats;

            if (Data != null && stats.Contains(Data))
            {
                stats.Remove(Data);

                ParentView.RefreshItems();
                EditorUtility.SetDirty(ParentWindow.Asset);
            }
        }

        protected override void DuplicateItem(DropdownMenuAction action)
        {
            StatsPanel statsPanel = ParentPanel as StatsPanel;

            if (statsPanel.CurrentProfile == null) return;

            List<Stat> stats = statsPanel.CurrentProfile.Stats;

            if (Data != null && stats.Contains(Data))
            {
                string itemName = NameLabel.text;
                int nextIndex = stats.IndexOf(Data) + 1;
                stats.Insert(nextIndex, Data);
                ParentView.RefreshItems();
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
            StatsPanel statsPanel = ParentPanel as StatsPanel;

            if (statsPanel.CurrentProfile != null)
            {
                int statIndex = statsPanel.CurrentProfile.Stats.IndexOf(Data);
                statsPanel.CurrentProfile.Stats[statIndex].SetId(NameTextField.value);
            }

            NameTextField.style.display = DisplayStyle.None;
            NameLabel.style.display = DisplayStyle.Flex;

            EditorUtility.SetDirty(ParentWindow.Asset);

            ParentView.RefreshItems();
        }

        protected override void OnCreateContextMenu(ContextualMenuPopulateEvent evt)
        {
            throw new System.NotImplementedException();
        }
    }
}
