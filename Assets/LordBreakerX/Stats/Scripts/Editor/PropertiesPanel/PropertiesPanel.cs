using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class PropertiesPanel : StatsEditorPanel
    {
        private Stat _currentStat;

        private TextField _idField;
        private EnumField _statTypeField;
        private FloatField _statFloatValueField;
        private IntegerField _statIntValueField;

        private ModifiersFoldout _modifiiersFoldout;

        public Stat CurrentStat { get => _currentStat; }

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
                _idField.value = stat.GetId();

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
                _statTypeField.value = StatType.Float;
                _statFloatValueField.value = 0;
                _statIntValueField.value = 0;
                _idField.value = "";
            }

            _modifiiersFoldout.UpdateModifiers();
        }

        protected override void OnExtendHeader(VisualElement header)
        {

        }

        protected override void OnCreatePanelGUI(VisualElement root)
        {
            Foldout generalProperties = CreateGeneralPropertiesFoldout();
            generalProperties.AddToClassList("decorated-foldout");

            VisualElement spacer = new VisualElement();
            spacer.style.minHeight = 15;

            _modifiiersFoldout = new ModifiersFoldout(this);
            root.Add(generalProperties);
            root.Add(spacer);
            root.Add(_modifiiersFoldout);

            ChangeStat(null);
        }

        private Foldout CreateGeneralPropertiesFoldout()
        {
            Foldout rootElement = new Foldout();
            rootElement.text = "Stat";

            _idField = new TextField("Stat ID");
            _idField.RegisterCallback<FocusOutEvent>(EditId);

            _statTypeField = new EnumField("Stat Type", StatType.Float);
            _statTypeField.RegisterValueChangedCallback(OnStatTypeChanged);

            _statFloatValueField = new FloatField("Base Value");
            _statFloatValueField.RegisterValueChangedCallback(OnFloatValueChanged);

            _statIntValueField = new IntegerField("Base Value");
            _statIntValueField.RegisterValueChangedCallback(OnIntValueChanged);
            _statIntValueField.style.display = DisplayStyle.None;

            rootElement.Add(_idField);
            rootElement.Add(_statTypeField);
            rootElement.Add(_statFloatValueField);
            rootElement.Add(_statIntValueField);

            rootElement.style.flexShrink = 0;

            return rootElement;
        }

        private void EditId(FocusOutEvent evt)
        {
            if (_currentStat == null) return;

            _currentStat.SetId(_idField.value);

            ParentWindow.CurrentStatsPanel.StatsListView.RefreshItems();

            EditorUtility.SetDirty(ParentWindow.CurrentStatsPanel.CurrentProfile);
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

    }
}
