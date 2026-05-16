using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    [CustomEditor(typeof(StatHolder))]
    public class StatHolderEditor : Editor
    {
        private PopupField<StatProfile> _currentProfilePopup;

        public override VisualElement CreateInspectorGUI()
        {
            StatHolder holder = (StatHolder)target;

            VisualElement root = new VisualElement();

            SerializedProperty profilesProperty = serializedObject.FindProperty("_holderStatProfiles");
            PropertyField profilesField = new PropertyField(profilesProperty, "Profiles");

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

            profilesField.RegisterValueChangeCallback(OnProfileAssetChanged);
            _currentProfilePopup.RegisterValueChangedCallback(OnProfileChanged);

            root.Add(profilesField);
            root.Add(_currentProfilePopup);

            return root;
        }

        private void OnProfileChanged(ChangeEvent<StatProfile> evt)
        {
            StatHolder holder = (StatHolder)target;

            holder.StartingProfile = evt.newValue;

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
