using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsEditorWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset _editorUI;

        [SerializeField]
        private StyleSheet _editorStyles;

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
            window.titleContent = new UnityEngine.GUIContent(namePrefix + " Stats Editor");
            window._asset = assetToEdit;
        }

        private void CreateGUI()
        {
            VisualElement ui = _editorUI.Instantiate();
            ui.style.flexGrow = 1;

            ui.styleSheets.Add(_editorStyles);

            ui.schedule.Execute(() =>
            {
                ui.Q<TwoPaneSplitView>("first-split").fixedPaneInitialDimension = 200;
                ui.Q<TwoPaneSplitView>("second-split").fixedPaneInitialDimension = 200;
            });

            rootVisualElement.Add(ui);
        }
    }
}
