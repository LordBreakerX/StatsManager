using UnityEditor;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsEditorWindow : EditorWindow
    {
        // the title of the editor window
        private const string WINDOW_TITLE = "Stats Editor";

        // the text of the header of the stages panel
        private const string STAGE_PANEL_HEADER = "Stat Stages";

        // the text of the header of the stats panel
        private const string STATS_PANEL_HEADER = "Stats";

        // the text of the header of the stat properties panel
        private const string PROPERTIES_PANEL_HEADER = "Stat Properties";

        // the path to the main stylesheet of the editor
        private const string EDITOR_STYLE = "Assets/LordBreakerX/Stats/StyleSheets/StatEditor.uss";

        private StatsEditorToolbar _toolbar;

        [MenuItem("Window/Stats/Editor")]
        public static void OpenWindow()
        {
            System.Type sceneViewType = typeof(SceneView);
            StatsEditorWindow window = GetWindow<StatsEditorWindow>(WINDOW_TITLE, sceneViewType);
        }

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            AddStyleSheets(root);

            _toolbar = new StatsEditorToolbar();

            VisualElement stagePanel = new StatsStagePanel(STAGE_PANEL_HEADER);
            VisualElement statsPanel = new StatsPanel(STATS_PANEL_HEADER);
            VisualElement propertiesPanel = new StatsEditorPanel(PROPERTIES_PANEL_HEADER);

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
