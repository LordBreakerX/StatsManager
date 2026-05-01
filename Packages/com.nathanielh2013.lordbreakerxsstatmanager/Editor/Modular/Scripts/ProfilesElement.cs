using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    [UxmlElement]
    public partial class ProfilesElement : VisualElement
    {
        private const string UI_PATH = "Packages/com.nathanielh2013.lordbreakerxsstatmanager/Editor/Modular/UXML/ProfilesUI.uxml";

        private ListView _profilesList;

        private StatsElement _statsElement;

        private Button _addButton;

        private StatProfile _currentProfile;

        private StatProfilesAsset _profilesAsset;

        private Action<StatProfile> _onProfileAdded;

        public ProfilesElement()
        {
            VisualTreeAsset uiTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UI_PATH);
            uiTree.CloneTree(this);

            style.flexGrow = 1;

            _profilesList = this.Q<ListView>("profiles-list");
            _profilesList.makeItem = MakeProfilesItem;
            _profilesList.bindItem = BindProfilesItem;
            _profilesList.reorderable = true;
            _profilesList.selectionChanged += OnProfileChanged;
            _profilesList.Rebuild();

            _statsElement = this.Q<StatsElement>();

            _addButton = this.Q<VisualElement>("profilesPanel").Q<Button>("headerButton");
            _addButton.clicked += CreateProfile;
        }

        private void RegisterProfileAddedCallback(Action<StatProfile> onProfileAdded)
        {
            _onProfileAdded = onProfileAdded;
        }

        private void CreateProfile()
        {
            StatProfile profile = ScriptableObject.CreateInstance<StatProfile>();
            profile.name = $"Profile {_profilesAsset.Profiles.Count}";

            string parentPath = AssetDatabase.GetAssetPath(_profilesAsset);

            if (!string.IsNullOrEmpty(parentPath))
            {
                AssetDatabase.AddObjectToAsset(profile, parentPath);
            }

            _profilesAsset.Profiles.Add(profile);
            _profilesList.Rebuild();

            _onProfileAdded?.Invoke(profile);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.SetDirty(_profilesAsset);
        }

        public void ChangeAsset(StatProfilesAsset profilesAsset)
        {
            _profilesAsset = profilesAsset;

            if (_profilesAsset != null)
            {
                _profilesList.itemsSource = profilesAsset.Profiles;
            }
            else
            {
                _profilesList.itemsSource = new List<StatProfile>();
            }

            _profilesList.Rebuild();
        }

        private void OnProfileChanged(IEnumerable<object> enumerable)
        {
            if (_profilesList.selectedItem is StatProfile profile)
            {
                _currentProfile = profile;
                _statsElement.Init(profile, () => _profilesList.Rebuild());
                _statsElement.SetSource(profile.Stats);
            }
        }

        private VisualElement MakeProfilesItem()
        {
            Label label = new Label();
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.paddingLeft = 10;
            return label;
        }

        private void BindProfilesItem(VisualElement element, int index)
        {
            StatProfile statProfile = (StatProfile)_profilesList.itemsSource[index];

            element.userData = index;

            if (element is Label label)
            {
                label.text = statProfile.ID;
            }
        }
    }
}
