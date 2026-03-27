using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LordBreakerX.Stats
{
    public class StatProfilePanel : StatsEditorListPanel<StatProfile>
    {

        public override string HeaderText => "Stat Profiles";

        public override string AddElementTitle => "Add Stat Profile";

        public StatProfilePanel(StatsEditorWindow parent) : base(parent)
        {
            
        }

        public override void UpdatePanel()
        {
            if (ParentWindow.Asset.Profiles.Count == 0 || !ParentWindow.Asset.Profiles.Contains(SelectedItem)) 
            {
                CurrentListView.selectedIndex = -1;
                SelectedItem = null;
            }

            CurrentListView.itemsSource = GetItemsSource();
            CurrentListView.Rebuild();
        }

        protected override StatsListItem<StatProfile> MakeElementItem()
        {
            StatProfileItem stageItem = new StatProfileItem(this, CurrentListView);
            stageItem.RegisterEvents();
            return stageItem;
        }

        protected override List<StatProfile> GetItemsSource()
        {
            return ParentWindow.Asset.Profiles;
        }

        protected override void CreateElement()
        {
            StatProfile profile = ScriptableObject.CreateInstance<StatProfile>();
            profile.name = "";

            string parentPath = AssetDatabase.GetAssetPath(ParentWindow.Asset);

            if (!string.IsNullOrEmpty(parentPath))
            {
                AssetDatabase.AddObjectToAsset(profile, parentPath);
            }

            ParentWindow.Asset.Profiles.Add(profile);
            CurrentListView.Rebuild();

            if (ParentWindow.CurrentToolbar.CurrentTemplate != null)
            {
                IReadOnlyList<Stat> stats = ParentWindow.CurrentToolbar.CurrentTemplate.CopyStats();

                profile.Stats.AddRange(stats);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(ParentWindow.Asset);
        }
    }
}
