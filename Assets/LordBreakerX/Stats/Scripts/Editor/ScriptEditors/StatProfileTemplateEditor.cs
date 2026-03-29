using Codice.Utils;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    [CustomEditor(typeof(StatProfileTemplate))]
    public class StatProfileTemplateEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            StatProfileTemplate template = (StatProfileTemplate)target;

            VisualElement root = new VisualElement();

            Label headerLabel = new Label("Stats In Template");
            headerLabel.style.justifyContent = Justify.Center;
            headerLabel.style.alignItems = Align.Center;
            headerLabel.style.unityTextAlign = TextAnchor.MiddleCenter;

            root.Add(headerLabel);

            VisualElement spacer = new VisualElement();
            spacer.style.minHeight = 15;
            root.Add(spacer);

            ListView view = new ListView((IList)template.Stats);
            view.makeItem = OnMake;
            view.bindItem = OnBind;
            view.Rebuild();

            view.selectionType = SelectionType.None;
            root.Add(view);

            return root;
        }

        private VisualElement OnMake()
        {
            var label = new Label();
            label.style.justifyContent = Justify.Center;
            label.style.alignItems = Align.Center;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            return label;
        }

        private void OnBind(VisualElement element, int arg2)
        {
            StatProfileTemplate template = (StatProfileTemplate)target;
            Label label = element.Q<Label>();

            Stat stat = template.Stats[arg2];

            label.text = stat.GetId() + " <color=orange>(" + stat.ValueType.ToString() + ")</color>";
        }
    }
}
