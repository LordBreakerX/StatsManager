using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsPanel : StatsEditorPanel
    {
        private Button _createStatButton;

        private StatProfile _currentProfile;

        private ListView _statsListView;

        public StatProfile CurrentProfile { get { return _currentProfile; } }

        public ListView StatsListView { get { return _statsListView; } }

        public StatsPanel(string labelText, StatsEditorWindow parent) : base(labelText, parent)
        {

        }

        public void ChangeProfile(StatProfile profile)
        {
            _currentProfile = profile;
            
            if (_currentProfile != null)
            {
                _statsListView.itemsSource = _currentProfile.Stats;
                _statsListView.RefreshItems();
                _createStatButton.SetEnabled(true);
            }
            else
            {
                _statsListView.itemsSource = new List<StatProfile>();
                _statsListView.RefreshItems();
                _createStatButton.SetEnabled(false);
            }
        } 

        protected override void OnExtendHeader(VisualElement header)
        {
            _createStatButton = new Button(CreateStat);
            _createStatButton.text = "+";
            header.Add(_createStatButton);
        }

        private void CreateStat()
        {
            if (_currentProfile == null) return;

            Stat stat = new Stat();
            stat.SetId("");

            _currentProfile.Stats.Add(stat);
            _statsListView.RefreshItems();

            EditorUtility.SetDirty(ParentWindow.Asset);
            EditorUtility.SetDirty(CurrentProfile);
        }

        protected override void OnCreatePanelGUI(VisualElement root)
        {
            _statsListView = new ListView(new List<Stat>());
            _statsListView.makeItem = MakeStatItem;
            _statsListView.destroyItem = DestroyStatItem;
            _statsListView.bindItem = BindStageItem;
            _statsListView.RefreshItems();

            _statsListView.selectionChanged += OnStatChanged;

            _createStatButton.SetEnabled(false);

            root.Add(_statsListView);
        }

        private void OnStatChanged(IEnumerable<object> enumerable)
        {
            if (_statsListView.selectedItem == null && _currentProfile != null) return;

            if (_statsListView.selectedItem is Stat stat)
            {
                ParentWindow.CurrentPropertiesPanel.ChangeStat(stat);
            }
        }

        private VisualElement MakeStatItem()
        {
            StatItem statItem = new StatItem(this, _statsListView);
            statItem.RegisterEvents();
            return statItem;
        }

        private void DestroyStatItem(VisualElement element)
        {
            if (element is StatItem statItem)
            {
                statItem.UnregisterEvents();
            }
        }

        private void BindStageItem(VisualElement element, int statIndex)
        {
            if (CurrentProfile == null) return;

            if (element is StatItem statItem)
            {
                Stat stat = CurrentProfile.Stats[statIndex];
                statItem.BindData(stat);
            }
        }
    }
}
