using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    [UxmlElement]
    public partial class StatsElement : VisualElement
    {
        private const string UI_PATH = "Packages/com.nathanielh2013.lordbreakerxsstatmanager/Editor/Modular/UXML/StatsUI.uxml";

        private ListView _statsView;

        private StatProperties _statProperties;

        private Button _addButton;

        private Stat _currentStat;

        private UnityEngine.Object _statHolder;

        private Action _onChanged;

        private IList _itemSources;

        public StatsElement()
        {
            VisualTreeAsset uiTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UI_PATH);
            uiTree.CloneTree(this);

            style.flexGrow = 1;

            _statsView = this.Q<ListView>("stats-list");
            _statsView.makeItem = MakeStatsItem;
            _statsView.bindItem = BindStatsItem;
            _statsView.reorderable = true;
            _statsView.selectionChanged += OnStatChanged;
            _statsView.Rebuild();

            _statProperties = this.Q<StatProperties>();
            _statProperties.Init(() => { _statsView.Rebuild(); });

            _addButton = this.Q<VisualElement>("statsPanel").Q<Button>("headerButton");
            _addButton.clicked += CreateStat;
        }

        private void OnStatChanged(IEnumerable<object> obj)
        {
            if (_statsView.selectedItem is Stat selectedStat)
            {
                _currentStat = selectedStat;

                SerializedObject serializedHolder = new SerializedObject(_statHolder);

                serializedHolder.Update();

                SerializedProperty statsProperty = serializedHolder.FindProperty("_stats");

                SerializedProperty statProperty = statsProperty.GetArrayElementAtIndex(_itemSources.IndexOf(_currentStat));

                _statProperties.SetStat(_currentStat, statProperty);
                _onChanged.Invoke();
            }
        }

        private void CreateStat()
        {
            Stat stat = new Stat();
            stat.SetId($"Stat {_itemSources.Count}");
            _itemSources.Add(stat);
            _statsView.Rebuild();
        }

        private VisualElement MakeStatsItem()
        {
            Label label = new Label();
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.paddingLeft = 10;
            return label;
        }

        private void BindStatsItem(VisualElement element, int index)
        {
            Stat stat = (Stat)_itemSources[index];

            if (element is Label label)
            {
                label.text = stat.GetId();
            }
        }

        public void Init(UnityEngine.Object statHolder, Action onChanged)
        {
            _statHolder = statHolder;
            _onChanged = onChanged;
        }

        public void SetSource(IList itemSources)
        {
            _itemSources = itemSources;

            if (_itemSources != null)
            {
                _statsView.itemsSource = _itemSources;
                _statsView.Rebuild();

                _addButton.SetEnabled(true);

                //if (_stats.Count > 0)
                //    _statProperties.SetStat(_stats[0], null);
            }
            else
            {
                _addButton.SetEnabled(false);
                //_statProperties.SetStat(null, null);
            }
        }
    }
}
