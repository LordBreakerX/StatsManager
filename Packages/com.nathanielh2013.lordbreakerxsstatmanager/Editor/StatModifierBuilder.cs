using LordBreakerX.EditorUtilities;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace LordBreakerX.Stats
{
    public class StatModifierBuilder : BuilderPopup<Type>
    {
        private Stat _currentStat;

        public override string Title => "Create Stat Modifier";

        public StatModifierBuilder(Stat currentStat, Action<ElementInfo<Type>> onClosed) : base(onClosed)
        {
            _currentStat = currentStat;
        } 

        protected override List<ElementInfo<Type>> GetItemsSource()
        {
            List<ElementInfo<Type>> elements = new List<ElementInfo<Type>>();

            List<ModifierAttributeResult> modifiers = ModifierAttributeFinder.GetTypesWithAttribute();

            foreach (ModifierAttributeResult result in modifiers)
            {
                if (result.attribute.ModifierType == _currentStat.ValueType)
                {
                    Type capturedType = result.modifierType;
                    string modifierDisplayName = result.attribute.DisplayName;

                    ElementInfo<Type> element = new ElementInfo<Type>(modifierDisplayName, capturedType);
                    elements.Add(element);
                }
            }

            return elements;
        }


    }
}
