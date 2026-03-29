using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
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

        [SerializeField]
        [SerializeReference]
        private List<Stat> _stats = new List<Stat>();

        private Stat _currentStat;

        private StatProfileTemplate _currentTemplate;

        private ListView _statsListView;

        private StatProperties _statFoldout;

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

        public static void OpenEditingTemplateWindow(Rect startingBounds, StatProfileTemplate template)
        {
            ProfileTemplateEditorWindow window = ScriptableObject.CreateInstance<ProfileTemplateEditorWindow>();
            window.titleContent = new GUIContent("Edit Profile Template");
            window._editType = TemplateEditType.Editing;

            window.minSize = new Vector2(500, 400);

            window.position = new Rect(
                startingBounds.width,
                startingBounds.height,
                window.minSize.x,
                window.minSize.y
            );

            window._currentTemplate = template;
            window._stats = template.CopyStats();

            window.ShowModalUtility();
        }

        public static void OpenDuplicatingTemplateWindow(Rect startingBounds, StatProfileTemplate template)
        {
            ProfileTemplateEditorWindow window = ScriptableObject.CreateInstance<ProfileTemplateEditorWindow>();
            window.titleContent = new GUIContent("Edit Profile Template");
            window._editType = TemplateEditType.Duplicating;

            window.minSize = new Vector2(500, 400);

            window.position = new Rect(
                startingBounds.width,
                startingBounds.height,
                window.minSize.x,
                window.minSize.y
            );

            window._currentTemplate = template;
            window._stats = template.CopyStats();

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

            _statFoldout = ui.Q<StatProperties>();
            _statFoldout.Init(() =>
            {
                _statsListView.Rebuild();
            });
            _statFoldout.SetEnabled(false);

            // add UI to the root element
            rootVisualElement.Add(ui);
        }

        private void InitilizeGUI(VisualElement ui)
        {
            TwoPaneSplitView splitView = ui.Q<TwoPaneSplitView>();

            splitView.fixedPaneInitialDimension = 200;

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
        }

        private void OnSave()
        {
            switch(_editType)
            {
                case TemplateEditType.Duplicating:
                case TemplateEditType.Adding:
                    StatProfileTemplate template = ScriptableObject.CreateInstance<StatProfileTemplate>();

                    _statFoldout.UpdateSerilized();
                    template.SetStats(_stats);

                    string createPath = EditorUtility.SaveFilePanel("Save Profile Template", "Assets", "StatProfile_Template", "asset");

                    createPath = "Assets" + createPath.Substring(Application.dataPath.Length);

                    AssetDatabase.CreateAsset(template, createPath);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();

                    EditorUtility.SetDirty(template);
                    break;
                case TemplateEditType.Editing:
                    if (_currentTemplate != null)
                    {
                        _statFoldout.UpdateSerilized();
                        _currentTemplate.SetStats(_stats);

                        EditorUtility.SetDirty(_currentTemplate);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                break;
            }

            Close();
        }

        private void OnStatChanged(IEnumerable<object> obj)
        {
            if (_statsListView.selectedItem is Stat selectedStat)
            {
                _currentStat = selectedStat;

                SerializedObject serializedWindow = new SerializedObject(this);
                SerializedProperty statsProperty = serializedWindow.FindProperty("_stats");
                SerializedProperty statProperty = statsProperty.GetArrayElementAtIndex(_stats.IndexOf(_currentStat));

                _statFoldout.SetStat(_currentStat, statProperty);
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
