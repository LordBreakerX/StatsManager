using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class ProfileTemplateEditorWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset _uiTreeAsset;

        [SerializeField]
        private StyleSheet _uiStyles;

        private TemplateEditType _editType;

        private List<Stat> _stats = new List<Stat>();

        private Stat _currentStat;

        private StatProfileTemplate _currentTemplate;

        private ListView _statsListView;

        private TextField _statIdField;

        private EnumField _statTypeField;

        private FloatField _statFloatValueField;

        public static void OpenAddingTemplateWindow(Rect startingBounds)
        {
            ProfileTemplateEditorWindow window = ScriptableObject.CreateInstance<ProfileTemplateEditorWindow>();
            window.titleContent = new GUIContent("Add Profile Template");
            window._editType = TemplateEditType.Adding;

            window.minSize = new Vector2(500, 400);

            window.position = new Rect(
                startingBounds.width,
                startingBounds.height,
                window.minSize.x, 
                window.minSize.y
            );

            window.ShowModalUtility();
        }

        public static void OpenEditingTemplateWindow(StatProfileTemplate template)
        {
            ProfileTemplateEditorWindow window = ScriptableObject.CreateInstance<ProfileTemplateEditorWindow>();
            window.titleContent = new GUIContent("Edit Profile Template");
            window._editType = TemplateEditType.Editing;
            window._currentTemplate = template;
            window.ShowModalUtility();
        }

        public static void OpenDuplicatingTemplateWindow(StatProfileTemplate template)
        {
            ProfileTemplateEditorWindow window = ScriptableObject.CreateInstance<ProfileTemplateEditorWindow>();
            window.titleContent = new GUIContent("Edit Profile Template");
            window._editType = TemplateEditType.Duplicating;
            window._currentTemplate = template;
            window.ShowModalUtility();
        }

        private void CreateGUI()
        {
            // create the UI
            VisualElement ui = _uiTreeAsset.Instantiate();
            ui.style.flexGrow = 1;

            // add the styles
            ui.styleSheets.Add(_uiStyles);

            InitilizeGUI(ui);

            // add UI to the root element
            rootVisualElement.Add(ui);
        }

        private void InitilizeGUI(VisualElement ui)
        {
            TwoPaneSplitView splitView = ui.Q<TwoPaneSplitView>();

            splitView.fixedPaneInitialDimension = 200;

            _statTypeField = ui.Q<EnumField>();
            _statTypeField.Init(StatType.Float);
            _statTypeField.RegisterValueChangedCallback(OnStatTypeChanged);

            Foldout modifiersFoldout = ui.Q<Foldout>("modifiers-foldout");
            Toggle toggle = modifiersFoldout.Q<Toggle>();

            VisualElement spacer = new VisualElement();
            spacer.style.flexGrow = 1;
            toggle.Add(spacer);

            Button addModifierButton = new Button();
            addModifierButton.text = "+";
            addModifierButton.AddToClassList("plus-button");
            toggle.Add(addModifierButton);

            _statsListView = ui.Q<ListView>("stats-list");
            _statsListView.itemsSource = _stats;
            _statsListView.makeItem = MakeStatsItem;
            _statsListView.bindItem = BindStatsItem;
            _statsListView.reorderable = true;
            _statsListView.selectionChanged += OnStatChanged;
            _statsListView.Rebuild();

            Button cancelButton = ui.Q<Button>("cancel-button");
            cancelButton.clicked += OnCancel;

            Button saveButton = ui.Q<Button>("save-button");
            saveButton.clicked += OnSave;

            Button addStatButton = ui.Q<VisualElement>("statsPanel").Q<Button>("headerButton");
            addStatButton.clicked += AddStat;

            _statIdField = ui.Q<TextField>("stat-id");
            _statIdField.RegisterValueChangedCallback(OnStatIdChanged);

            _statFloatValueField = ui.Q<FloatField>("stat-base-value");
            _statFloatValueField.RegisterValueChangedCallback(OnStatValueChanged);
        }

        private void OnSave()
        {
            switch(_editType)
            {
                case TemplateEditType.Duplicating:
                case TemplateEditType.Adding:
                    StatProfileTemplate template = ScriptableObject.CreateInstance<StatProfileTemplate>();

                    template.SetStats(_stats);

                    string createPath = EditorUtility.SaveFilePanel("Save Profile Template", "Assets", "StatProfile_Template", "asset");

                    createPath = "Assets" + createPath.Substring(Application.dataPath.Length);

                    AssetDatabase.CreateAsset(template, createPath);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    EditorUtility.SetDirty(template);
                    break;
                case TemplateEditType.Editing:
                    break;
            }

            Close();
        }

        private void OnStatValueChanged(ChangeEvent<float> evt)
        {
            if (_currentStat != null)
            {
                _currentStat.BaseValue = evt.newValue;
                _statsListView.Rebuild();
            }
        }

        private void OnStatTypeChanged(ChangeEvent<Enum> evt)
        {
            if (_currentStat != null)
            {
                _currentStat.ValueType = (StatType)evt.newValue;
                _statsListView.Rebuild();
            }
        }

        private void OnStatIdChanged(ChangeEvent<string> evt)
        {
            if (_currentStat != null)
            {
                _currentStat.SetId(evt.newValue);
                _statsListView.Rebuild();
            }
        }

        private void OnStatChanged(IEnumerable<object> obj)
        {
            if (_statsListView.selectedItem is Stat selectedStat)
            {
                _currentStat = selectedStat;

                _statIdField.value = selectedStat.GetId();
                _statFloatValueField.value = selectedStat.BaseValue;
                _statTypeField.value = selectedStat.ValueType;
            }
        }

        private void AddStat()
        {
            Stat stat = new Stat();
            stat.SetId($"Stat {_stats.Count}");
            _stats.Add(stat);
            _statsListView.Rebuild();
        }

        private void OnCancel()
        {
            Close();
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
            Stat stat = _stats[index];

            if (element is Label label)
            {
                label.text = stat.GetId();
            }
        }
    }
}
