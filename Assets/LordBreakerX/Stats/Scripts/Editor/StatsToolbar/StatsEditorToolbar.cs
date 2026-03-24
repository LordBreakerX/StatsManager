using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsEditorToolbar : Toolbar
    {
        private List<StatProfileTemplate> _options = new List<StatProfileTemplate>();

        public StatProfileTemplate CurrentTemplate { get; private set; }

        public StatsEditorToolbar()
        {
            AddToClassList("stat-toolbar");

            UpdateTemplates();
            PopupField<StatProfileTemplate> popupField = new PopupField<StatProfileTemplate>(_options, 0);

            popupField.formatListItemCallback = FormatPopupItem;
            popupField.formatSelectedValueCallback = FormatPopupItem;
            popupField.RegisterValueChangedCallback(OnTemplateChanged);

            Add(popupField);
        }

        private void OnTemplateChanged(ChangeEvent<StatProfileTemplate> evt)
        {
            CurrentTemplate = evt.newValue;
        }

        private string FormatPopupItem(StatProfileTemplate template)
        {
            if (template == null)
            {
                return "< None >";
            }
            else
            {
                return template.ToString();
            }
        }

        public void UpdateTemplates()
        {
            _options.Clear();
            _options.Add(null);

            string[] guids = AssetDatabase.FindAssets("t:StatProfileTemplate");

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                
                if (!string.IsNullOrEmpty(assetPath))
                {
                    StatProfileTemplate template = AssetDatabase.LoadAssetAtPath<StatProfileTemplate>(assetPath);
                    
                    if (template != null)
                        _options.Add(template);
                }
            }
        }

        public void CreateTemplate(StatProfile profile, string createPath)
        {
            StatProfileTemplate template = ScriptableObject.CreateInstance<StatProfileTemplate>();

            template.SetStats(profile.Stats);

            AssetDatabase.CreateAsset(template, createPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.SetDirty(template);
        }
    }
}
