using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    [UxmlElement]
    public partial class ProfilesElement : VisualElement
    {
        private const string UI_PATH = "";

        private ListView _profilesList;

        private StatsElement _statsElement;

        private Button _addButton;

        private StatProfile _currentProfile;

        private UnityEngine.Object _statHolder;

        private Action _onChanged;

        private IList _itemSources;

        public ProfilesElement()
        {
            VisualTreeAsset uiTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UI_PATH);
            uiTree.CloneTree(this);

            style.flexGrow = 1;

            _profilesList = this.Q<ListView>("stats-list");
            _profilesList.makeItem = MakeStatsItem;
            _profilesList.bindItem = BindStatsItem;
            _profilesList.reorderable = true;
            _profilesList.selectionChanged += OnStatChanged;
            _profilesList.Rebuild();

            _statsElement = this.Q<StatsElement>();

        }

        private void OnStatChanged(IEnumerable<object> enumerable)
        {
            throw new NotImplementedException();
        }

        private void BindStatsItem(VisualElement element, int arg2)
        {
            throw new NotImplementedException();
        }

        private VisualElement MakeStatsItem()
        {
            throw new NotImplementedException();
        }
    }
}
