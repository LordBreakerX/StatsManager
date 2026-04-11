using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LordBreakerX.Stats
{
    public class StatHolder : MonoBehaviour, IStatHandler
    {
        [Serializable]
        public struct StatProfileEvent
        {
            public string statID;
            public UnityEvent<StatContext> _onChangedEvent;
        }

        [SerializeField]
        private StatProfilesAsset _holderStatProfiles;

        [SerializeField]
        private StatProfile _currentProfile;

        [SerializeField]
        private List<StatProfileEvent> _statsEvents = new();

        private Dictionary<string, UnityEvent<StatContext>> _eventRegistry = new Dictionary<string, UnityEvent<StatContext>>();

        public List<StatProfileEvent> StatsEvents { get => _statsEvents; set => _statsEvents = value; }
        private void Awake()
        {
            foreach (StatProfileEvent profileEvent in _statsEvents) 
            {
                if (!_eventRegistry.ContainsKey(profileEvent.statID))
                {
                    _eventRegistry.Add(profileEvent.statID, profileEvent._onChangedEvent);
                }
            }
        }

        public void OnStatUpdate(StatContext context)
        {
            if (context.statProfile != _currentProfile) return;

            string statID = context.stat.GetId();

            if (_eventRegistry.ContainsKey(statID))
            {
                var statEvent = _eventRegistry[statID];

                if (statEvent != null)
                    statEvent.Invoke(context);
            }
        }
    }
}
