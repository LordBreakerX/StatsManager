using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsEditorWindow : EditorWindow
    {
        // the title of the editor window
        public const string WINDOW_TITLE = " Stats Editor";

        // the path to the main stylesheet of the editor
        public const string EDITOR_STYLE = "Assets/LordBreakerX/Stats/StyleSheets/StatEditor.uss";

        public const string EDITOR_SAVE_PATH = "LordBreakerX_StatsEditorWindow_Path";

        private StatProfilesAsset _asset;

        public StatProfilesAsset Asset { get { return _asset; } }

        public StatProfilePanel CurrentProfilePanel { get; private set; }
        public StatsPanel CurrentStatsPanel { get; private set; }

        public PropertiesPanel CurrentPropertiesPanel { get; private set; }

        public StatsEditorToolbar CurrentToolbar { get; private set; }  

        public static void OpenWindow(string namePrefix, StatProfilesAsset assetToEdit)
        {
            System.Type sceneViewType = typeof(SceneView);
            StatsEditorWindow window = GetWindow<StatsEditorWindow>(sceneViewType);
            window.titleContent = new UnityEngine.GUIContent(namePrefix + WINDOW_TITLE);
            window._asset = assetToEdit;

            string path = AssetDatabase.GetAssetPath(window._asset);
            EditorPrefs.SetString(EDITOR_SAVE_PATH, path);
        }

        private void OnEnable()
        {
            string path = EditorPrefs.GetString(EDITOR_SAVE_PATH, null);
            if (!string.IsNullOrEmpty(path))
            {
                _asset = AssetDatabase.LoadAssetAtPath<StatProfilesAsset>(path);
            }
        }

        private void OnDisable()
        {
            string path = AssetDatabase.GetAssetPath(_asset);
            EditorPrefs.SetString(EDITOR_SAVE_PATH, path);
        }

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            AddStyleSheets(root);

            CurrentToolbar = new StatsEditorToolbar();

            CurrentProfilePanel = new StatProfilePanel(this);
            CurrentStatsPanel = new StatsPanel(this);
            CurrentPropertiesPanel = new PropertiesPanel(this);

            VisualElement splitView = CreateSplitView(CurrentProfilePanel, CurrentStatsPanel, CurrentPropertiesPanel);

            root.Add(CurrentToolbar);
            root.Add(splitView);

            UpdatePanels();
        }

        private void AddStyleSheets(VisualElement root)
        {
            StyleSheet editorStyles = AssetDatabase.LoadAssetAtPath<StyleSheet>(EDITOR_STYLE);
            root.styleSheets.Add(editorStyles);
        }

        private VisualElement CreateSplitView(VisualElement stage, VisualElement stats, VisualElement properties)
        {
            var mainSplit = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            var secondSplit = new TwoPaneSplitView(1, 300, TwoPaneSplitViewOrientation.Horizontal);

            secondSplit.Add(stats);
            secondSplit.Add(properties);

            mainSplit.Add(stage);
            mainSplit.Add(secondSplit);

            mainSplit.style.flexGrow = 1;

            return mainSplit;
        }

        public void UpdatePanels()
        {
            if (CurrentProfilePanel != null)
                CurrentProfilePanel.UpdatePanel();

            if (CurrentStatsPanel != null)
                CurrentStatsPanel.UpdatePanel();

            if (CurrentPropertiesPanel != null)
                CurrentPropertiesPanel.UpdatePanel();
        }
    }
}
