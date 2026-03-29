using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    [UxmlElement]
    public partial class StatProperties : VisualElement
    {
        private const string UI_PATH = "Assets/LordBreakerX/Stats/Scripts/Editor/TemplateWindow/StatPropertiesUI.uxml";

        private TextField _idField;
        private EnumField _statTypeField;
        private FloatField _floatValueField;

        private Stat _currentStat;

        private ListView _modifiersView;

        private Action _onChanged;

        private SerializedProperty _statProperty;

        public StatProperties()
        {
            VisualTreeAsset uiTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UI_PATH);
            uiTree.CloneTree(this);

            _idField = this.Q<TextField>("stat-id");
            _idField.RegisterValueChangedCallback(OnIdChanged);

            _statTypeField = this.Q<EnumField>("stat-type");
            _statTypeField.Init(StatType.Float);
            _statTypeField.RegisterValueChangedCallback(OnStatTypeChanged);

            _floatValueField = this.Q<FloatField>("stat-base-value");
            _floatValueField.RegisterValueChangedCallback(OnValueChanged);

            Toggle modifiersToggle = this.Q<Foldout>("modifiers-foldout").Q<Toggle>();

            VisualElement spacer = new VisualElement();
            spacer.style.flexGrow = 1;
            modifiersToggle.Add(spacer);

            Button addModifierButton = new Button(AddModifier);
            addModifierButton.text = "+";
            addModifierButton.AddToClassList("plus-button");
            modifiersToggle.Add(addModifierButton);

            _modifiersView = this.Q<ListView>("stat-modifiers-list");
            _modifiersView.makeNoneElement = () => new VisualElement();
            _modifiersView.makeItem = () => { return new VisualElement(); };
            _modifiersView.bindItem = BindModifier;
            _modifiersView.unbindItem = UnbindModifier;
            _modifiersView.virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight;
            _modifiersView.selectionType = SelectionType.None;
            _modifiersView.Rebuild();
        }

        // Stat events

        private void OnIdChanged(ChangeEvent<string> evt)
        {
            if (_currentStat != null)
            {
                _currentStat.SetId(evt.newValue);
                _onChanged?.Invoke();
            }
        }

        private void OnStatTypeChanged(ChangeEvent<Enum> evt)
        {
            if (_currentStat != null)
            {
                _currentStat.ValueType = (StatType)evt.newValue;
                _onChanged?.Invoke();
            }
        }

        private void OnValueChanged(ChangeEvent<float> evt)
        {
            if (_currentStat != null)
            {
                _currentStat.BaseValue = evt.newValue;
                _onChanged?.Invoke();
            }
        }

        // modifier events

        private void BindModifier(VisualElement element, int index)
        {
            element.Clear();

            if (_currentStat == null || _statProperty == null) return;

            StatModifier currentModifier = _currentStat.Modifiers[index];

            Debug.Log($"modifiers: {_currentStat.Modifiers.Count}");

            Type modifierType = currentModifier.GetType();

            _statProperty.serializedObject.Update();

            SerializedProperty modifiersListProperty = _statProperty.FindPropertyRelative("_modifiers");

            SerializedProperty modifierProperty = modifiersListProperty.GetArrayElementAtIndex(index);

            string modifieirName = modifierType.Name;

            CustomStatModifierAttribute attribute = modifierType.GetCustomAttribute<CustomStatModifierAttribute>();

            if (attribute != null)
            {
                modifieirName = attribute.DisplayName;
            }

            PropertyField field = new PropertyField(modifierProperty, modifieirName);
            field.RegisterCallback<ContextualMenuPopulateEvent>(evt =>
            {
                evt.menu.MenuItems().Clear();

                evt.menu.AppendAction("Delete Modifier", (_) =>
                {
                    _currentStat.RemoveModifier(index);

                    UpdateSerilized();

                    _modifiersView.Rebuild();
                    _onChanged?.Invoke();
                });
            });

            element.Add(field);

            element.Bind(_statProperty.serializedObject);

            Toggle toggle = field.Q<Toggle>();

            VisualElement spacer = new VisualElement();
            spacer.style.flexGrow = 1;
            toggle.Add(spacer);

            Button shiftUp = new Button(() =>
            {
                if (index > 0)
                {
                    _currentStat.RemoveModifier(index);
                    _currentStat.InsertModifier(currentModifier, index - 1);

                    UpdateSerilized();

                    _modifiersView.Rebuild();
                    _onChanged?.Invoke();
                }
            });
            shiftUp.text = "▲";
            shiftUp.AddToClassList("shift-button");

            toggle.Add(shiftUp);

            Button shiftDown = new Button(() =>
            {
                if (_currentStat.ModifierCount - 1 > index)
                {
                    _currentStat.RemoveModifier(index);
                    _currentStat.InsertModifier(currentModifier, index + 1);

                    UpdateSerilized();

                    _modifiersView.Rebuild();
                    _onChanged?.Invoke();
                }
            });
            shiftDown.text = "▼";
            shiftDown.AddToClassList("shift-button");

            toggle.Add(shiftDown);

            Button removeButton = new Button(() =>
            {
                _currentStat.RemoveModifier(index);

                UpdateSerilized();

                _modifiersView.Rebuild();
                _onChanged?.Invoke();
            });
            removeButton.text = "-";
            removeButton.AddToClassList("plus-button");

            toggle.Add(removeButton);
        }

        private void UnbindModifier(VisualElement element, int index)
        {
            element.Clear();
            element.Unbind();
        }

        private void AddModifier()
        {
            if (_currentStat == null) return;

            GenericMenu menu = new GenericMenu();

            List<ModifierAttributeResult> modifiers = ModifierAttributeFinder.GetTypesWithAttribute();

            foreach (ModifierAttributeResult result in modifiers)
            {
                if (result.attribute.ModifierType == _currentStat.ValueType)
                {
                    var capturedType = result.modifierType;
                    var capturedName = result.attribute.DisplayName;

                    menu.AddItem(new GUIContent(capturedName), false, () =>
                    {
                        var modifier = (StatModifier)Activator.CreateInstance(capturedType);

                        if (modifier != null)
                        {
                            _currentStat.AddModifier(modifier);

                            UpdateSerilized();


                            _modifiersView.Rebuild();
                            _onChanged?.Invoke();

                        }
                    });
                }
            }

            menu.ShowAsContext();
        }

        // other methods

        public void SetStat(Stat stat, SerializedProperty property)
        {
            _currentStat = stat;
            _statProperty = property;

            if (_currentStat != null && _statProperty != null)
            {
                _idField.value = _currentStat.GetId();
                _floatValueField.value = _currentStat.BaseValue;
                _statTypeField.value = _currentStat.ValueType;
                _modifiersView.itemsSource = (IList)_currentStat.Modifiers;

                UpdateSerilized();

                _modifiersView.Rebuild();
                SetEnabled(true);
            }
            else
            {
                SetEnabled(false);
            }
        }

        public void Init(Action onChanged)
        {
            _onChanged = onChanged;
        }

        public void UpdateSerilized()
        {
            if (_statProperty != null)
            {
                _statProperty.serializedObject.Update();
                _statProperty.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}
