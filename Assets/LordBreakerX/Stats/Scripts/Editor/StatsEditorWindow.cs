using UnityEditor;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsEditorWindow : EditorWindow
    {
        // the title of the editor window
        private const string WINDOW_TITLE = " Stats Editor";

        // the text of the header of the stages panel
        private const string STAGE_PANEL_HEADER = "Stat Profiles";

        // the text of the header of the stats panel
        private const string STATS_PANEL_HEADER = "Stats";

        // the text of the header of the stat properties panel
        private const string PROPERTIES_PANEL_HEADER = "Stat Properties";

        // the path to the main stylesheet of the editor
        private const string EDITOR_STYLE = "Assets/LordBreakerX/Stats/StyleSheets/StatEditor.uss";

        private const string EDITOR_SAVE_PATH = "LordBreakerX_StatsEditorWindow_Path";

        private StatsEditorToolbar _toolbar;

        private StatProfilesAsset _asset;

        public StatProfilesAsset Asset { get { return _asset; } }

        public static void OpenWindow(string namePrefix, StatProfilesAsset assetToEdit)
        {
            System.Type sceneViewType = typeof(SceneView);
            StatsEditorWindow window = GetWindow<StatsEditorWindow>(namePrefix + WINDOW_TITLE, sceneViewType);
            window._asset = assetToEdit;
        }

        private void OnEnable()
        {
            string path = EditorPrefs.GetString(EDITOR_SAVE_PATH, null);
            if (!string.IsNullOrEmpty(path) && Asset == null)
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

            _toolbar = new StatsEditorToolbar();

            VisualElement stagePanel = new StatsStagePanel(STAGE_PANEL_HEADER, this);
            VisualElement statsPanel = new StatsPanel(STATS_PANEL_HEADER, this);
            VisualElement propertiesPanel = new StatsEditorPanel(PROPERTIES_PANEL_HEADER, this);

            VisualElement splitView = CreateSplitView(stagePanel, statsPanel, propertiesPanel);

            root.Add(_toolbar);
            root.Add(splitView);
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
    }
}
