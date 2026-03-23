using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsPanel : StatsEditorPanel
    {
        private Button _createStageButton;

        private StatProfile _currentProfile;

        private ListView _stagesListView;

        public StatsPanel(string labelText, StatsEditorWindow parent) : base(labelText, parent)
        {

        }

        public void ChangeProfile(StatProfile profile)
        {
            _currentProfile = profile;
        } 

        protected override void OnExtendHeader(VisualElement header)
        {
            _createStageButton = new Button(CreateStage);
            _createStageButton.text = "+";
            header.Add(_createStageButton);
        }

        private void CreateStage()
        {
            if (_currentProfile != null)
            {
                //_currentProfile.Stats.Add();
                _stagesListView.RefreshItems();
            }
        }

        protected override void OnCreatePanelGUI(VisualElement root)
        {
            _stagesListView = new ListView(_currentProfile.Stats);
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
            string stageName = _currentProfile.Stats[stageIndex].Id;
            TextField field = element.Q<TextField>();
            field.value = stageName;
        }

        private void OnStageChanged(ChangeEvent<string> evt, int stageIndex)
        {
            //[stageIndex] = evt.newValue;
            _stagesListView.RefreshItems();
        }

        private void UnbindStageItem(VisualElement element, int stageIndex)
        {
            TextField field = element.Q<TextField>();
        }
    }
}
