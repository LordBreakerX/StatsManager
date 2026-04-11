using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    [CustomEditor(typeof(StatHolder))]
    public class StatHolderEditor : Editor
    {
        private PopupField<StatProfile> _currentProfilePopup;

        private ListView _eventsList;

        public override VisualElement CreateInspectorGUI()
        {
            StatHolder holder = (StatHolder)target;

            VisualElement root = new VisualElement();

            SerializedProperty profilesProperty = serializedObject.FindProperty("_holderStatProfiles");
            PropertyField profilesField = new PropertyField(profilesProperty, "Profiles");
            profilesField.RegisterValueChangeCallback(OnProfileAssetChanged);
            root.Add(profilesField);

            _currentProfilePopup = new PopupField<StatProfile>("Starting Profile");
            _currentProfilePopup.formatListItemCallback = (statProfile) =>
            {
                if (statProfile != null) return statProfile.ID;
                else return "No Profiles";
            };
            _currentProfilePopup.formatSelectedValueCallback = (statProfile) =>
            {
                if (statProfile != null) return statProfile.ID;
                else return "No Profiles";
            };
            _currentProfilePopup.RegisterValueChangedCallback(OnProfileChanged);
            root.Add(_currentProfilePopup);

            VisualElement spacer = new VisualElement();
            spacer.style.flexGrow = 15;
            root.Add(spacer);

            _eventsList = new ListView();
            _eventsList.style.flexGrow = 1;
            _eventsList.itemsSource = holder.StatsEvents;
            _eventsList.makeNoneElement = () => { return new VisualElement(); };
            _eventsList.makeItem = () => { return new VisualElement(); };
            _eventsList.bindItem = OnCreateEventItem;
            _eventsList.reorderable = false;
            _eventsList.selectionType = SelectionType.None;
            _eventsList.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            _eventsList.Rebuild();

            root.Add(_eventsList);

            return root;
        }

        private void OnCreateEventItem(VisualElement element, int index)
        {
            throw new NotImplementedException();
        }

        private void OnProfileChanged(ChangeEvent<StatProfile> evt)
        {
            StatHolder holder = (StatHolder)target;

            holder.StatsEvents = new List<StatHolder.StatProfileEvent>();

            if (evt.newValue != null)
            {
                
            }

            EditorUtility.SetDirty(target);
        }

        private void OnProfileAssetChanged(SerializedPropertyChangeEvent evt)
        {
            if (evt.changedProperty.objectReferenceValue is StatProfilesAsset profilesAsset)
            {
                _currentProfilePopup.choices = profilesAsset.Profiles;
                _currentProfilePopup.index = 0;
                _currentProfilePopup.style.display = DisplayStyle.Flex;
            }
            else
            {
                _currentProfilePopup.choices = new List<StatProfile>();
                _currentProfilePopup.style.display = DisplayStyle.None;
            }

            EditorUtility.SetDirty(target);
        }
    }
}
