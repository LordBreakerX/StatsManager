using System;

namespace LordBreakerX.Stats
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomStatModifierAttribute : Attribute
    {
        public string GroupName { get; private set; }

        public string DisplayName { get; private set; }

        public StatType ModifierType { get; private set; }

        public CustomStatModifierAttribute(string groupName, string displayName, StatType modifierType)
        {
            GroupName = groupName;
            DisplayName = displayName;
            ModifierType = modifierType;
        }

        public CustomStatModifierAttribute(string displayName, StatType modifierType) : this("Custom Stat Modifiers", displayName, modifierType)
        {

        }
    }
}
