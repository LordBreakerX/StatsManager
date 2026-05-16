using System.Collections.Generic;
using UnityEngine;

namespace LordBreakerX.Stats
{
    public class StatHolder : MonoBehaviour
    {
        [SerializeField]
        private StatProfilesAsset _holderStatProfiles;

        [SerializeField]
        private StatProfile _startingProfile;

        private Dictionary<string, Stat> _stats = new Dictionary<string, Stat>();

        public StatProfile StartingProfile { get => _startingProfile; set => _startingProfile = value;  }

        private void Awake()
        {
            SetProfile(_startingProfile);
        }

        public void SetProfile(StatProfile profile)
        {
            _stats = profile.GetStatsTable();
        }

        public float GetFloat(string statID)
        {
            if (_stats.ContainsKey(statID))
            {
                return _stats[statID].GetValue();
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"The stat \"{statID}\" does not exist!");
#endif
                return 0;
            }
        }

        public int GetInt(string statID)
        {
            return (int)GetFloat(statID);
        }
    }
}
