using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsStagePanel : StatsEditorPanel
    {
        private Button _createStageButton;

        private ListView _stagesListView;

        public StatsStagePanel(string labelText, StatsEditorWindow parent) : base(labelText, parent)
        {
        }

        protected override void OnExtendHeader(VisualElement header)
        {
            _createStageButton = new Button(CreateStage);
            _createStageButton.text = "+";
            header.Add(_createStageButton);
        }

        private void CreateStage()
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

            root.Add(_stagesListView);

            root.AddManipulator(new ContextualMenuManipulator(CreateContextMenu));
        }

        private void CreateContextMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Add Stat Stage", (action) =>
            {
                CreateStage();
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
            StatProfileItem stageItem = new StatProfileItem(ParentWindow, _stagesListView);
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
