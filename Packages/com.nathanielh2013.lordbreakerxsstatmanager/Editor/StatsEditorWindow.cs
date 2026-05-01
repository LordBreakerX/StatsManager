using UnityEditor;

namespace LordBreakerX.Stats
{
    public class StatsEditorWindow : EditorWindow
    {
        // the title of the editor window
        public const string WINDOW_TITLE = " Stats Editor";

        public const string EDITOR_SAVE_PATH = "LordBreakerX_StatsEditorWindow_Path";

        private StatProfilesAsset _asset;

        private ProfilesElement _profiles;

        private StatsEditorToolbar _toolbar;

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

            if (_profiles != null) _profiles.ChangeAsset(_asset);
        }

        private void OnDisable()
        {
            string path = AssetDatabase.GetAssetPath(_asset);
            EditorPrefs.SetString(EDITOR_SAVE_PATH, path);
        }

        private void Update()
        {
            if (_asset == null)
            {
                Close();
            }
        }

        private void CreateGUI()
        {
            _toolbar = new StatsEditorToolbar();

            _profiles = new ProfilesElement();
            _profiles.ChangeAsset(_asset);

            rootVisualElement.Add(_toolbar);
            rootVisualElement.Add(_profiles);
        }
    }
}
