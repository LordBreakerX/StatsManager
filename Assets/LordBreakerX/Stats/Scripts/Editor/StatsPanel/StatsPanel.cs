using System.Collections.Generic;
using UnityEditor;

namespace LordBreakerX.Stats
{
    public class StatsPanel : StatsEditorListPanel<Stat>
    {
        public override string HeaderText => "Stats";

        public override string AddElementTitle => "Add Stat";

        public StatsPanel(StatsEditorWindow parent) : base(parent)
        {
            
        }

        public override void UpdatePanel()
        {
            CurrentListView.itemsSource = GetItemsSource();
            CurrentListView.Rebuild();

            if (ParentWindow.CurrentProfilePanel.SelectedItem != null)
            {
                CreateButton.SetEnabled(true);
            }
            else
            {
                CreateButton.SetEnabled(false);
            }
        }

        protected override List<Stat> GetItemsSource()
        {
            if (ParentWindow.CurrentProfilePanel.SelectedItem != null)
            {
                return ParentWindow.CurrentProfilePanel.SelectedItem.Stats;
            }
            else
            {
                return new List<Stat>();
            }
        } 

        protected override void CreateElement()
        {
            StatProfile selectedProfile = ParentWindow.CurrentProfilePanel.SelectedItem;

            if (selectedProfile != null)
            {
                Stat stat = new Stat();
                stat.SetId("");

                selectedProfile.Stats.Add(stat);
                CurrentListView.RefreshItems();

                EditorUtility.SetDirty(ParentWindow.Asset);
                EditorUtility.SetDirty(selectedProfile);
            }
        }

        protected override StatsListItem<Stat> MakeElementItem()
        {
            StatItem statItem = new StatItem(this, CurrentListView);
            statItem.RegisterEvents();
            return statItem;
        }
    }
}
