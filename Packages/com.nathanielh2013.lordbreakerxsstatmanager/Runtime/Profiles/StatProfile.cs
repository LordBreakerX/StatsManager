using System;
using System.Collections.Generic;
using UnityEngine;

namespace LordBreakerX.Stats
{
    public class StatProfile : ScriptableObject
    {
        [SerializeField]
        private List<Stat> _stats = new List<Stat>();

        public string ID { get { return name; } }

        public List<Stat> Stats { get { return _stats; } }

        public Dictionary<string, Stat> GetStatsTable()
        {
            Dictionary<string, Stat> statTable = new Dictionary<string, Stat>();

            foreach (Stat stat in Stats) 
            {
                if (!statTable.ContainsKey(stat.GetId()))
                {
                    statTable.Add(stat.GetId(), stat);
                }
#if UNITY_EDITOR
                else
                {
                    Debug.LogWarning($"\"{ID}\" Stat Profile already has the stat \"{stat.GetId()}\"!");
                }
#endif
            }

            return statTable;
        }
    }
}
