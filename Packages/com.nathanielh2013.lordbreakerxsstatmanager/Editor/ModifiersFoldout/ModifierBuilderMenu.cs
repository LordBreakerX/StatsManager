using LordBreakerX.EditorUtilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using static PlasticGui.WorkspaceWindow.Merge.MergeInProgress;

namespace LordBreakerX.Stats
{
    public class ModifierBuilderMenu : BuilderMenu<StatModifier>
    {
        private StatType _statType;

        public ModifierBuilderMenu(StatType statType, Action<Builder<StatModifier>> onClose) : base(onClose)
        {
            _statType = statType;
        }

        protected override string BuilderTitle => "Stat Modifiers";

        protected override IEnumerable<Builder<StatModifier>> GetBuilders()
        {
            List<Builder<StatModifier>> elements = new List<Builder<StatModifier>>();

            List<ModifierAttributeResult> modifiers = ModifierAttributeFinder.GetTypesWithAttribute();

            foreach (ModifierAttributeResult result in modifiers)
            {
                if (result.attribute.ModifierType ==_statType)
                {
                    Type capturedType = result.modifierType;
                    string modifierDisplayName = result.attribute.DisplayName;
                    string modifierGroupName = result.attribute.GroupName;

                    StatModifier modifier = (StatModifier)Activator.CreateInstance(capturedType);

                    Texture icon = IconUtility.GetTypeIcon(capturedType);

                    Builder<StatModifier> element = new Builder<StatModifier>(icon, modifierGroupName, modifierDisplayName, modifier);
                    elements.Add(element);
                }
            }

            return elements;
        }
    }
}
