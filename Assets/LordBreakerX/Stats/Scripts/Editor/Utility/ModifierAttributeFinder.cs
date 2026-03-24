using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LordBreakerX.Stats
{
    public class ModifierAttributeResult
    {
        public Type modifierType;
        public CustomStatModifierAttribute attribute;
    }

    public static class ModifierAttributeFinder
    {
        public static List<ModifierAttributeResult> GetTypesWithAttribute()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly =>
                {
                    try { return assembly.GetTypes(); }
                    catch (ReflectionTypeLoadException e)
                    {
                        return e.Types.Where(t => t != null);
                    }
                })
                .Select(type => new
                {
                    Type = type,
                    Attribute = type.GetCustomAttribute<CustomStatModifierAttribute>()
                })
                .Where(x => x.Attribute != null)
                .Select(x => new ModifierAttributeResult
                {
                    modifierType = x.Type,
                    attribute = x.Attribute
                })
                .ToList();
        }
    }
}
