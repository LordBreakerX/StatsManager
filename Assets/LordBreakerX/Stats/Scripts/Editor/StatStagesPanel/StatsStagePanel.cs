using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsStagePanel : StatsEditorPanel
    {
        private Button _createStageButton;

        private List<string> _stages = new List<string>();

        private ListView _stagesListView;

        public StatsStagePanel(string labelText) : base(labelText)
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
            int nextIndex = _stages.Count;
            _stages.Add($"stage_{nextIndex}");
            _stagesListView.RefreshItems();
        }

        protected override void OnCreatePanelGUI(VisualElement root)
        {
            _stagesListView = new ListView(_stages);
            _stagesListView.reorderable = true;
            _stagesListView.makeItem = MakeStageItem;
            _stagesListView.destroyItem = DestroyStageItem;
            _stagesListView.bindItem = BindStageItem;
            _stagesListView.style.flexGrow = 1;
            _stagesListView.allowAdd = true;
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
            TextField field = element.Q<TextField>();
            field.UnregisterCallback<BlurEvent>(OnBlurStageField);
        }

        private VisualElement MakeStageItem()
        {
            StatsListItem<string> stageItem = new StatsListItem<string>(_stages, _stagesListView);
            return stageItem;
        }

        private void BindStageItem(VisualElement element, int stageIndex)
        {
            if (element is StatsListItem<string> stageItem)
            {
                string data = _stages[stageIndex];
                stageItem.BindData(data);
            }

            //string stageName = _stages[stageIndex];
            //TextField stageField = element.Q<TextField>();
            //Label stageLabel = element.Q<Label>();

            //stageField.value = stageName;
            //stageField.userData = stageIndex;
            //stageLabel.text = stageName;
        }

        private void OnBlurStageField(BlurEvent evt)
        {
            TextField stageField = evt.target as TextField;
            VisualElement rootElement = stageField.parent;
            Label stageLabel = rootElement.Q<Label>();

            int stageIndex = (int)stageField.userData;
            _stages[stageIndex] = stageField.value;

            stageField.style.display = DisplayStyle.None;
            stageLabel.style.display = DisplayStyle.Flex;

            _stagesListView.RefreshItems();
        }
    }
}
