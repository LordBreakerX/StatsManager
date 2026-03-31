using System.Collections.Generic;
using UnityEngine;

namespace LordBreakerX.Stats
{
    /* main script that handles the logic of updating stats
     */
    public static class StatManager
    {
        public struct StatContext
        {
            public readonly StatProfile statProfile;
            public readonly Stat stat;

            public StatContext(StatProfile statProfile, Stat stat)
            {
                this.stat = stat;
                this.statProfile = statProfile;
            }

            public readonly float GetValue()
            {
                return this.stat.GetValue();
            }

            public readonly bool IsStat(string statID)
            {
                return this.stat.GetId() == statID;
            }

            public readonly bool IsStat(string profileId, string statID)
            {
                return this.statProfile.ID == profileId && this.stat.GetId() == statID;
            }
        }

        public delegate void OnStatUpdate(StatContext context);

        private static OnStatUpdate _onStatUpdate;

        public static void RegisterListener(OnStatUpdate listener)
        {
            if (listener != null)
            {
                _onStatUpdate += listener;
            }
        }

        public static void UnregisterListener(OnStatUpdate listener)
        {
            if (listener != null)
            {
                _onStatUpdate -= listener;
            }
        }

        public static void UpdateStats(StatProfile profile)
        {
            foreach (Stat stat in profile.Stats)
            {
                StatContext context = new StatContext(profile, stat);
                _onStatUpdate?.Invoke(context);
            }    
        }

        public static void UpdateStats(StatProfilesAsset profilesAsset, string profileID)
        {
            StatProfile profile = profilesAsset.GetProfile(profileID);

            if (profile != null)
            {
                UpdateStats(profile);
            }
        }
    }
}
