using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatProfilePanel : StatsEditorPanel
    {
        private Button _createStageButton;

        private ListView _stagesListView;

        public StatProfilePanel(string labelText, StatsEditorWindow parent) : base(labelText, parent)
        {
        }

        protected override void OnExtendHeader(VisualElement header)
        {
            _createStageButton = new Button(CreateStatProfile);
            _createStageButton.text = "+";
            header.Add(_createStageButton);
        }

        private void CreateStatProfile()
        {
            int nextIndex = ParentWindow.Asset.Profiles.Count;

            StatProfile profile = ScriptableObject.CreateInstance<StatProfile>();
            profile.name = $"Stat Profile {nextIndex}";
            
            string parentPath = AssetDatabase.GetAssetPath(ParentWindow.Asset);

            if (!string.IsNullOrEmpty(parentPath))
            {
                AssetDatabase.AddObjectToAsset(profile, parentPath);
                AssetDatabase.SaveAssets();
            }

            ParentWindow.Asset.Profiles.Add(profile);
            _stagesListView.RefreshItems();

            AssetDatabase.Refresh();
            EditorUtility.SetDirty(ParentWindow.Asset);
        }

        protected override void OnCreatePanelGUI(VisualElement root)
        {
            Debug.Log(ParentWindow.Asset);
            _stagesListView = new ListView(ParentWindow.Asset.Profiles);
            _stagesListView.reorderable = true;
            _stagesListView.makeItem = MakeStageItem;
            _stagesListView.destroyItem = DestroyStageItem;
            _stagesListView.bindItem = BindStageItem;
            _stagesListView.style.flexGrow = 1;
            _stagesListView.Rebuild();

            _stagesListView.selectionChanged += OnProfileChanged;

            root.Add(_stagesListView);

            root.AddManipulator(new ContextualMenuManipulator(CreateContextMenu));
        }

        private void OnProfileChanged(System.Collections.Generic.IEnumerable<object> obj)
        {
            if (_stagesListView.selectedItem == null) return;

            if (_stagesListView.selectedItem is StatProfile item)
            {
                ParentWindow.CurrentStatsPanel.ChangeProfile(item);
            }
        }

        private void CreateContextMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Add Stat Stage", (action) =>
            {
                CreateStatProfile();
            });
        }

        private void DestroyStageItem(VisualElement element)
        {
            if (element is StatProfileItem profileItem)
            {
                profileItem.UnregisterEvents();
            }
        }

        private VisualElement MakeStageItem()
        {
            StatProfileItem stageItem = new StatProfileItem(this, _stagesListView);
            stageItem.RegisterEvents();
            return stageItem;
        }

        private void BindStageItem(VisualElement element, int stageIndex)
        {
            if (element is StatProfileItem stageItem)
            {
                StatProfile profile = ParentWindow.Asset.Profiles[stageIndex];
                stageItem.BindData(profile);
            }
        }
    }
}
