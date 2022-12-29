using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class PlayerMistakeAnalytics : MonoBehaviour
    {
        public static PlayerMistakeAnalytics Instance;

        private readonly Dictionary<string, int> _mistakes = new();

        public Dictionary<string, int> Mistakes
        {
            get { return _mistakes; }
        }

        [Header("Events")]
        [SerializeField]
        private GameEventStringInt _addMistakeCallback = null;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);

            _addMistakeCallback.AddListener(AddMistakeCallback);
        }

        private void OnDestroy()
        {
            _addMistakeCallback.RemoveListener(AddMistakeCallback);
        }

        private void AddMistakeCallback(string key, int value)
        {
            if (_mistakes.ContainsKey(key))
            {
                _mistakes[key] += value;
                return;
            }

            _mistakes.Add(key, value);
        }
    }
}
