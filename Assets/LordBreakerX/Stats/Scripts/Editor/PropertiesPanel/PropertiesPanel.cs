using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class PropertiesPanel : StatsEditorPanel
    {
        private Stat _currentStat;

        private EnumField _statTypeField;
        private FloatField _statFloatValueField;
        private IntegerField _statIntValueField;

        private VisualElement _modifiersContainer;

        private Button _statModifierButton;

        public PropertiesPanel(string labelText, StatsEditorWindow parent) : base(labelText, parent)
        {
        }

        public void ChangeStat(Stat stat)
        {
            _currentStat = stat;

            if (_currentStat != null)
            {
                SetEnabled(true);
                _statTypeField.value = stat.ValueType;
                _statFloatValueField.value = stat.BaseValue;
                _statIntValueField.value = (int)stat.BaseValue;

                switch (stat.ValueType)
                {
                    case StatType.Float:
                        _statFloatValueField.style.display = DisplayStyle.Flex;
                        _statIntValueField.style.display = DisplayStyle.None;
                        break;
                    case StatType.Int:
                        _statFloatValueField.style.display = DisplayStyle.None;
                        _statIntValueField.style.display = DisplayStyle.Flex;
                        break;
                }
            }
            else
            {
                SetEnabled(false);
            }
        }

        protected override void OnExtendHeader(VisualElement header)
        {

        }

        protected override void OnCreatePanelGUI(VisualElement root)
        {
            Foldout generalProperties = CreateGeneralPropertiesFoldout();

            VisualElement spacer = new VisualElement();
            spacer.style.minHeight = 15;

            Foldout statModifiers = CreateStatModifiersFoldout();
            root.Add(generalProperties);
            root.Add(spacer);
            root.Add(statModifiers);

            ChangeStat(null);
        }

        private Foldout CreateGeneralPropertiesFoldout()
        {
            Foldout rootElement = new Foldout();
            rootElement.text = "Stat";

            _statTypeField = new EnumField("Stat Type", StatType.Float);
            _statTypeField.RegisterValueChangedCallback(OnStatTypeChanged);

            _statFloatValueField = new FloatField("Base Value");
            _statFloatValueField.RegisterValueChangedCallback(OnFloatValueChanged);

            _statIntValueField = new IntegerField("Base Value");
            _statIntValueField.RegisterValueChangedCallback(OnIntValueChanged);
            _statIntValueField.style.display = DisplayStyle.None;

            rootElement.Add(_statTypeField);
            rootElement.Add(_statFloatValueField);
            rootElement.Add(_statIntValueField);

            return rootElement;
        }

        private void OnFloatValueChanged(ChangeEvent<float> evt)
        {
            if (_currentStat == null) return;
            if (_currentStat.ValueType != StatType.Float) return;

            _currentStat.BaseValue = evt.newValue;

            EditorUtility.SetDirty(ParentWindow.CurrentStatsPanel.CurrentProfile);
        }

        private void OnIntValueChanged(ChangeEvent<int> evt)
        {
            if (_currentStat == null) return;
            if (_currentStat.ValueType != StatType.Int) return;

            _currentStat.BaseValue = evt.newValue;

            EditorUtility.SetDirty(ParentWindow.CurrentStatsPanel.CurrentProfile);
        }

        private void OnStatTypeChanged(ChangeEvent<Enum> evt)
        {
            if (_currentStat == null) return;

            _statFloatValueField.value = _currentStat.BaseValue;
            _statIntValueField.value = (int)_currentStat.BaseValue;

            _currentStat.ValueType = (StatType)evt.newValue;

            switch (evt.newValue)
            {
                case StatType.Float:
                    _statFloatValueField.style.display = DisplayStyle.Flex;
                    _statIntValueField.style.display = DisplayStyle.None;
                    break;
                case StatType.Int:
                    _statFloatValueField.style.display = DisplayStyle.None;
                    _statIntValueField.style.display = DisplayStyle.Flex;
                    _currentStat.BaseValue = (int)_currentStat.BaseValue;
                    break;
            }

            EditorUtility.SetDirty(ParentWindow.CurrentStatsPanel.CurrentProfile);
        }

        private Foldout CreateStatModifiersFoldout()
        {
            Foldout rootElement = new Foldout();
            rootElement.text = "Stat Modifiers";

            Toggle foldoutToggle = rootElement.Q<Toggle>();
            foldoutToggle.style.flexDirection = FlexDirection.Row;

            VisualElement spacer = new VisualElement();
            spacer.style.flexGrow = 1;

            _statModifierButton = new Button(CreateStatModifier);
            _statModifierButton.text = "+";
            _statModifierButton.AddToClassList("plus-button");

            foldoutToggle.Add(spacer);
            foldoutToggle.Add(_statModifierButton);

            _modifiersContainer = new VisualElement();
            rootElement.Add(_modifiersContainer);

            return rootElement;
        }

        private void CreateStatModifier()
        {
            if (_currentStat == null) return;

            GenericMenu menu = new GenericMenu();

            List<ModifierAttributeResult> modifiers = ModifierAttributeFinder.GetTypesWithAttribute();

            foreach (ModifierAttributeResult result in modifiers)
            {
                if (result.attribute.ModifierType == _currentStat.ValueType)
                {
                    menu.AddItem(new GUIContent(result.attribute.DisplayName), false, () =>
                    {
                        StatModifier modifier = (StatModifier)Activator.CreateInstance(result.modifierType);

                        if (modifier != null)
                        {
                            _currentStat.AddModifier(modifier);
                            EditorUtility.SetDirty(ParentWindow.CurrentStatsPanel.CurrentProfile);
                        }
                    });
                }
            }

            menu.ShowAsContext();
        }

        
    }
}
