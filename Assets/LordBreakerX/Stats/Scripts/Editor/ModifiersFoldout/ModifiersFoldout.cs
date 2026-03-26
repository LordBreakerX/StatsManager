using System;
using System.Collections;
using System.Collections.Generic;
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
            _modifiersListView.makeNoneElement = () => new VisualElement();
            _modifiersListView.makeItem = () => new VisualElement();
            _modifiersListView.bindItem = BindModifierItem;
            _modifiersListView.unbindItem = UnbindModifierItem;
            _modifiersListView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            _modifiersListView.selectionType = SelectionType.None;

            Add(_modifiersListView);
        }

        private void BindModifierItem(VisualElement element, int index)
        {
            element.Clear();

            StatProfile profile = _parentPanel.ParentWindow.CurrentStatsPanel.CurrentProfile;

            if (_parentPanel.CurrentStat == null || profile == null) return;

            StatModifier currentModifier = _parentPanel.CurrentStat.Modifiers[index];

            Type modifierType = currentModifier.GetType();


            SerializedObject serializedProfile = new SerializedObject(profile);
            SerializedProperty statsProperty = serializedProfile.FindProperty("_stats");

            int statIndex = profile.Stats.IndexOf(_parentPanel.CurrentStat);

            SerializedProperty currentStatProperty = statsProperty.GetArrayElementAtIndex(statIndex);
            SerializedProperty modifiersProperty = currentStatProperty.FindPropertyRelative("_modifiers");

            SerializedProperty currentModifierProperty = modifiersProperty.GetArrayElementAtIndex(index);

            PropertyField field = new PropertyField(currentModifierProperty, modifierType.Name);
            field.RegisterCallback<ContextualMenuPopulateEvent>(evt =>
            {
                evt.menu.MenuItems().Clear();
            });

            element.Add(field);

            element.Bind(serializedProfile);
        }

        private void RemoveStatModifier(int modifierIndex)
        {
            if (_parentPanel.CurrentStat == null) return;
            if (_parentPanel.ParentWindow.CurrentStatsPanel.CurrentProfile == null) return;

            _parentPanel.CurrentStat.RemoveModifier(modifierIndex);

            _modifiersListView.Rebuild();

            EditorUtility.SetDirty(_parentPanel.ParentWindow.CurrentStatsPanel.CurrentProfile);
        }

        private void UnbindModifierItem(VisualElement element, int index)
        {
            element.Clear();
            element.Unbind();
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
                    var capturedType = result.modifierType;
                    var capturedName = result.attribute.DisplayName;

                    menu.AddItem(new GUIContent(capturedName), false, () =>
                    {
                        var modifier = (StatModifier)Activator.CreateInstance(capturedType);

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
            StatProfile profile = _parentPanel.ParentWindow.CurrentStatsPanel.CurrentProfile;

            if (_parentPanel.CurrentStat != null && profile != null)
            {
                IList modifiers = (IList)_parentPanel.CurrentStat.Modifiers;

                _modifiersListView.itemsSource = modifiers;
            }
            else
            {
                _modifiersListView.itemsSource = new List<StatModifier>();
            }

                _modifiersListView.Rebuild();
        }
    }
}
