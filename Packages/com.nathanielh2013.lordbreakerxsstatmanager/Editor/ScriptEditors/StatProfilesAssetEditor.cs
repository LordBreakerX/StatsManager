using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    [CustomEditor(typeof(StatProfilesAsset))]
    public class StatProfilesAssetEditor : Editor
    {
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);

            if (obj is StatProfilesAsset asset)
            {
                StatsEditorWindow.OpenWindow(asset.name, asset);
                return true;
            } 

            return false;
        }


        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            Button button = new Button(() => {
                StatProfilesAsset asset = (StatProfilesAsset)target;
                StatsEditorWindow.OpenWindow(asset.name, asset);
            });

            button.text = "Open in Stats Editor";
            button.style.minHeight = 40;
            button.style.unityFontStyleAndWeight = FontStyle.Bold;

            root.Add(button);

            return root;
        }
    }
}
