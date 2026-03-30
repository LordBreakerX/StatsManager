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

        private StatProfileTemplate _currentTemplate;

        private StatsElement _statsElement;

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

            window._currentTemplate = ScriptableObject.CreateInstance<StatProfileTemplate>();

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

            window.ShowModalUtility();
        }

        public static void OpenDuplicatingTemplateWindow(Rect startingBounds, StatProfileTemplate template)
        {
            ProfileTemplateEditorWindow window = ScriptableObject.CreateInstance<ProfileTemplateEditorWindow>();
            window.titleContent = new GUIContent("Duplicate Profile Template");
            window._editType = TemplateEditType.Duplicating;

            window.minSize = new Vector2(500, 400);

            window.position = new Rect(
                startingBounds.width,
                startingBounds.height,
                window.minSize.x,
                window.minSize.y
            );

            window._currentTemplate = ScriptableObject.CreateInstance<StatProfileTemplate>();
            window._currentTemplate.SetStats(template.CopyStats());

            window.ShowModalUtility();
        }

        public static void OpenCreateFromProfile(Rect startingBounds, StatProfile profile)
        {
            List<Stat> stats = new List<Stat>();

            foreach (Stat stat in profile.Stats) 
            {
                stats.Add(new Stat(stat));
            }

            ProfileTemplateEditorWindow window = ScriptableObject.CreateInstance<ProfileTemplateEditorWindow>();
            window.titleContent = new GUIContent("Create Profile Template");
            window._editType = TemplateEditType.Adding;

            window.minSize = new Vector2(500, 400);

            window.position = new Rect(
                startingBounds.width,
                startingBounds.height,
                window.minSize.x,
                window.minSize.y
            );

            window._currentTemplate = ScriptableObject.CreateInstance<StatProfileTemplate>();
            window._currentTemplate.SetStats(stats);

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

            _statsElement = ui.Q<StatsElement>();
            _statsElement.Init(_currentTemplate, () => 
            {
                
            });

            _statsElement.SetSource((System.Collections.IList)_currentTemplate.Stats);

            // add UI to the root element
            rootVisualElement.Add(ui);
        }

        private void InitilizeGUI(VisualElement ui)
        {
            TwoPaneSplitView splitView = ui.Q<TwoPaneSplitView>();
            splitView.fixedPaneInitialDimension = 200;

            Button cancelButton = ui.Q<Button>("cancel-button");
            cancelButton.clicked += OnCancel;

            Button saveButton = ui.Q<Button>("save-button");
            saveButton.clicked += OnSave;
        }

        private void OnSave()
        {
            if (_editType != TemplateEditType.Editing)
            {
                string createPath = EditorUtility.SaveFilePanel("Save Profile Template", "Assets", "StatProfile_Template", "asset");

                if (!string.IsNullOrEmpty(createPath))
                {
                    createPath = "Assets" + createPath.Substring(Application.dataPath.Length);

                    AssetDatabase.CreateAsset(_currentTemplate, createPath);

                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }

            EditorUtility.SetDirty(_currentTemplate);
            Close();
        }

        private void OnCancel()
        {
            if (!AssetDatabase.Contains(_currentTemplate) && _currentTemplate != null)
            {
                DestroyImmediate(_currentTemplate, true);
            }

            Close();
        }
    }
}
