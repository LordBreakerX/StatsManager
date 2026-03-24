using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    [CustomEditor(typeof(StatProfilesAsset))]
    public class StatProfilesAssetEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();

            Button button = new Button(() => {
                StatProfilesAsset asset = (StatProfilesAsset)target;
                StatsEditorWindow.OpenWindow(asset.name, asset);
            });

            button.text = "Open Stats Editor";
            button.style.fontSize = 25;
            button.style.unityFontStyleAndWeight = FontStyle.Bold;

            root.Add(button);

            return root;
        }
    }
}
