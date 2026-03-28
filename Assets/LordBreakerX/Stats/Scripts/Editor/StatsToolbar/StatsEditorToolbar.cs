using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.Stats
{
    public class StatsEditorToolbar : Toolbar
    {
        private const string NO_TEMPLATE_NAME = "None";

        private const string ADD_TEMPLATE_NAME = "Add Profile Template";

        private const string EDIT_TEMPLATE_NAME = "Edit Profile Template";

        private const string DUPLICATE_TEMPLATE_NAME = "Duplicate Profile Template";

        private const string REMOVE_TEMPLATE_NAME = "Remove Profile Template";

        private ToolbarMenu _templateMenu;

        public StatProfileTemplate CurrentTemplate { get; private set; }

        public StatsEditorToolbar()
        {
            AddToClassList("stat-toolbar");

            _templateMenu = new ToolbarMenu();
            _templateMenu.text = NO_TEMPLATE_NAME;

            Add(_templateMenu);

            UpdateTemplatesMenu();
        }

        private void OnTemplateChanged(ChangeEvent<StatProfileTemplate> evt)
        {
            CurrentTemplate = evt.newValue;
        }

        public void UpdateTemplatesMenu()
        {
            _templateMenu.menu.ClearItems();

            _templateMenu.menu.AppendAction(NO_TEMPLATE_NAME, OnSelectTemplate, GetSelectableStatus);

            AddTemplatesToMenu();

            _templateMenu.menu.AppendSeparator();

            _templateMenu.menu.AppendAction(ADD_TEMPLATE_NAME, OnAddTemplate);
            _templateMenu.menu.AppendAction(EDIT_TEMPLATE_NAME, OnEditTemplate, UpdateTemplateActionStatus);
            _templateMenu.menu.AppendAction(DUPLICATE_TEMPLATE_NAME, OnDuplicateTemplate, UpdateTemplateActionStatus);
            _templateMenu.menu.AppendAction(REMOVE_TEMPLATE_NAME, OnRemoveTemplate, UpdateTemplateActionStatus);

            if (CurrentTemplate != null)
            {
                _templateMenu.text = $"Template: {CurrentTemplate.name}";
            }
            else
            {
                _templateMenu.text = $"Template: {NO_TEMPLATE_NAME}";
            }
        }

        private void OnRemoveTemplate(DropdownMenuAction action)
        {
           if (CurrentTemplate != null)
           {
                string assetPath = AssetDatabase.GetAssetPath(CurrentTemplate);

                if (AssetDatabase.DeleteAsset(assetPath))
                {
                    AssetDatabase.SaveAssets();
                }

                CurrentTemplate = null;
                UpdateTemplatesMenu();
            }
        }

        private void OnDuplicateTemplate(DropdownMenuAction action)
        {
            throw new NotImplementedException();
        }

        private void OnEditTemplate(DropdownMenuAction action)
        {
            throw new NotImplementedException();
        }

        private void OnAddTemplate(DropdownMenuAction action)
        {
            ProfileTemplateEditorWindow.OpenAddingTemplateWindow(_templateMenu.worldBound);
            UpdateTemplatesMenu();
        }

        private DropdownMenuAction.Status UpdateTemplateActionStatus(DropdownMenuAction action)
        {
            if (CurrentTemplate == null)
                return DropdownMenuAction.Status.Disabled;
            else
                return DropdownMenuAction.Status.Normal;
        }

        private void AddTemplatesToMenu()
        {
            string[] guids = AssetDatabase.FindAssets("t:StatProfileTemplate");

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                if (!string.IsNullOrEmpty(assetPath))
                {
                    StatProfileTemplate template = AssetDatabase.LoadAssetAtPath<StatProfileTemplate>(assetPath);

                    if (template != null)
                    {
                        _templateMenu.menu.AppendAction(template.name, OnSelectTemplate, GetSelectableStatus, template);
                    }
                }
            }
        }

        private void OnSelectTemplate(DropdownMenuAction action)
        {
            if (action.userData is StatProfileTemplate template) 
            {
                CurrentTemplate = template;
                _templateMenu.text = $"Template: {CurrentTemplate.name}";
            }
            else if (action.userData == null)
            {
                CurrentTemplate = null;
                _templateMenu.text = $"Template: {NO_TEMPLATE_NAME}";
            }
        }

        private DropdownMenuAction.Status GetSelectableStatus(DropdownMenuAction action)
        {
            StatProfileTemplate template = (StatProfileTemplate)action.userData;

            if (template == CurrentTemplate)
            {
                return DropdownMenuAction.Status.Checked;
            }
            else if (CurrentTemplate == null && action.userData == null)
            {
                return DropdownMenuAction.Status.Checked;
            }
            else
            {
                return DropdownMenuAction.Status.Normal;
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
