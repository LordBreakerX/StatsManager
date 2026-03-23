using System;

namespace LordBreakerX.Stats
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomStatModifierAttribute : Attribute
    {
        public string DisplayName { get; private set; }

        public StatType ModifierType { get; private set; }

        public CustomStatModifierAttribute(string displayName, StatType modifierType)
        {
            DisplayName = displayName;
            ModifierType = modifierType;
        }
    }
}
