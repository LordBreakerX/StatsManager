using System;

namespace LordBreakerX.Stats
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class CustomStatModifierAttribute : Attribute
    {
        public string LabelText { get; private set; }

        public CustomStatModifierAttribute(string labelText)
        {
            LabelText = labelText;
        }
    }
}
