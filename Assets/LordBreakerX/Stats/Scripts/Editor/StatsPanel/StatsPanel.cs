using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsPanel : StatsEditorPanel
    {
        private Button _createStageButton;

        private List<string> _stages = new List<string>();

        private ListView _stagesListView;

        public StatsPanel(string labelText) : base(labelText)
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
            _stages.Add("");
            _stagesListView.RefreshItems();
        }

        protected override void OnCreatePanelGUI(VisualElement root)
        {
            _stagesListView = new ListView(_stages);
            _stagesListView.makeItem = MakeStageItem;
            _stagesListView.bindItem = BindStageItem;
            _stagesListView.unbindItem = UnbindStageItem;
            _stagesListView.RefreshItems();

            root.Add(_stagesListView);
        }

        private VisualElement MakeStageItem()
        {
            return new TextField();
        }

        private void BindStageItem(VisualElement element, int stageIndex)
        {
            string stageName = _stages[stageIndex];
            TextField field = element.Q<TextField>();
            field.value = stageName;
        }

        private void OnStageChanged(ChangeEvent<string> evt, int stageIndex)
        {
            _stages[stageIndex] = evt.newValue;
            _stagesListView.RefreshItems();
        }

        private void UnbindStageItem(VisualElement element, int stageIndex)
        {
            TextField field = element.Q<TextField>();
        }
    }
}
