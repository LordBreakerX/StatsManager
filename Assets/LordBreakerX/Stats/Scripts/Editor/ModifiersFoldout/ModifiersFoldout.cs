using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class ModifiersFoldout : Foldout
    {
        private ListView _modifiersListView;
        private Button _statModifierButton;

        private PropertiesPanel _parentPanel;

        public ModifiersFoldout(PropertiesPanel panel)
        {
            AddToClassList("decorated-foldout");

            _parentPanel = panel;
            text = "Stat Modifiers";

            Toggle foldoutToggle = this.Q<Toggle>();
            foldoutToggle.style.flexDirection = FlexDirection.Row;

            VisualElement spacer = new VisualElement();
            spacer.style.flexGrow = 1;

            _statModifierButton = new Button(CreateStatModifier);
            _statModifierButton.text = "+";
            _statModifierButton.AddToClassList("plus-button");

            foldoutToggle.Add(spacer);
            foldoutToggle.Add(_statModifierButton);

            _modifiersListView = new ListView(new List<StatModifier>());
            
            Add(_modifiersListView);
        }

        private void CreateStatModifier()
        {
            Stat stat = _parentPanel.CurrentStat;

            if (stat == null) return;

            GenericMenu menu = new GenericMenu();

            List<ModifierAttributeResult> modifiers = ModifierAttributeFinder.GetTypesWithAttribute();

            foreach (ModifierAttributeResult result in modifiers)
            {
                if (result.attribute.ModifierType == stat.ValueType)
                {
                    menu.AddItem(new GUIContent(result.attribute.DisplayName), false, () =>
                    {
                        StatModifier modifier = (StatModifier)Activator.CreateInstance(result.modifierType);

                        if (modifier != null)
                        {
                            stat.AddModifier(modifier);
                            EditorUtility.SetDirty(_parentPanel.ParentWindow.CurrentStatsPanel.CurrentProfile);
                            UpdateModifiers();
                        }
                    });
                }
            }

            menu.ShowAsContext();
        }

        public void UpdateModifiers()
        {
            _modifiersListView.Rebuild();

            //_modifiersListView.Rebuild();

            //StatProfile profile = ParentWindow.CurrentStatsPanel.CurrentProfile;

            //if (_currentStat == null || profile == null) return;

            //SerializedObject serializedProfile = new SerializedObject(profile);
            //SerializedProperty statsProperty = serializedProfile.FindProperty("_stats");

            //int statIndex = profile.Stats.IndexOf(_currentStat);

            //SerializedProperty currentStatProperty = statsProperty.GetArrayElementAtIndex(statIndex);
            //SerializedProperty modifiersProperty = currentStatProperty.FindPropertyRelative("_modifiers");

            //for (int i = 0; i < modifiersProperty.arraySize; i++)
            //{
            //    SerializedProperty currentModifierProperty = modifiersProperty.GetArrayElementAtIndex(i);

            //    _modifiersContainer.Bind(serializedProfile);
            //}
        }

        private static void ShowDerivedFields(VisualElement element, SerializedProperty property, Type targetType, Type baseType)
        {
            List<FieldInfo> derivedFields = new List<FieldInfo>();

            Debug.Log(targetType);

            while (targetType != baseType)
            {
                FieldInfo[] fields = targetType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                derivedFields.AddRange(fields);
                targetType = targetType.BaseType;
            }

            foreach (FieldInfo field in derivedFields)
            {
                SerializedProperty fieldProperty = property.FindPropertyRelative(field.Name);

                if (property != null)
                {
                    PropertyField propertyField = new PropertyField(fieldProperty);
                    propertyField.BindProperty(fieldProperty);
                    element.Add(propertyField);
                }
            }
        }
    }
}
