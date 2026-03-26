using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatProfilePanel : StatsEditorPanel
    {
        private Button _createStageButton;

        private ListView _stagesListView;

        public ListView ProfilesView { get { return _stagesListView; } }

        public StatProfilePanel(string labelText, StatsEditorWindow parent) : base(labelText, parent)
        {

        }

        public void Reset()
        {
            ProfilesView.itemsSource = ParentWindow.Asset.Profiles;
            ProfilesView.Rebuild();
        }

        protected override void OnExtendHeader(VisualElement header)
        {
            VisualElement spacer = new VisualElement();
            spacer.style.flexGrow = 1;

            header.Add(spacer); 

            _createStageButton = new Button(CreateStatProfile);
            _createStageButton.text = "+";
            _createStageButton.AddToClassList("plus-button");
            header.Add(_createStageButton);
        }

        private void CreateStatProfile()
        {
            StatProfile profile = ScriptableObject.CreateInstance<StatProfile>();
            profile.name = "";
            
            string parentPath = AssetDatabase.GetAssetPath(ParentWindow.Asset);

            if (!string.IsNullOrEmpty(parentPath))
            {
                AssetDatabase.AddObjectToAsset(profile, parentPath);
            }

            ParentWindow.Asset.Profiles.Add(profile);
            _stagesListView.Rebuild();
            
            if (ParentWindow.CurrentToolbar.CurrentTemplate != null)
            {
                IReadOnlyList<Stat> stats = ParentWindow.CurrentToolbar.CurrentTemplate.CopyStats();

                profile.Stats.AddRange(stats);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(ParentWindow.Asset);
        }

        protected override void OnCreatePanelGUI(VisualElement root)
        {
            _stagesListView = new ListView(ParentWindow.Asset.Profiles);
            _stagesListView.reorderable = true;
            _stagesListView.makeItem = MakeStageItem;
            _stagesListView.destroyItem = DestroyStageItem;
            _stagesListView.bindItem = BindStageItem;
            _stagesListView.style.flexGrow = 1;

            _stagesListView.selectionChanged += OnProfileChanged;
            _stagesListView.Rebuild();

            root.Add(_stagesListView);

            root.AddManipulator(new ContextualMenuManipulator(CreateContextMenu));
        }

        private void OnProfileChanged(System.Collections.Generic.IEnumerable<object> obj)
        {
            if (_stagesListView.selectedItem == null) return;

            if (_stagesListView.selectedItem is StatProfile item)
            {
                ParentWindow.CurrentStatsPanel.ChangeProfile(item);
                ParentWindow.CurrentPropertiesPanel.ChangeStat(null);
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
