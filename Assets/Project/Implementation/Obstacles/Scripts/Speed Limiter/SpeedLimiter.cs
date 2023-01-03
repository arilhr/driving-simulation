using Sirenix.OdinInspector;
using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{

    public class SpeedLimiter : MonoBehaviour, IObstacle
    {
        #region Variables

        [BoxGroup("Data")]
        [SerializeField]
        private int _violatePoint = 10;

        [BoxGroup("Data")]
        [SerializeField]
        private float _timeBetweenViolate = 3f;

        [BoxGroup("Maximum Limit")]
        [SerializeField]
        private bool _isMaxActive = false;

        [BoxGroup("Maximum Limit")]
        [SerializeField]
        private float _maxSpeed = 80f;

        [BoxGroup("Maximum Limit")]
        [SerializeField]
        private GameEventBool _setActiveMaxLimitCallback = null;

        [BoxGroup("Maximum Limit")]
        [SerializeField]
        private GameEventBool _onActiveMaxLimiter = null;

        [BoxGroup("Maximum Limit")]
        [SerializeField]
        private GameEventFloat _changedMaxSpeedCallback = null;

        [BoxGroup("Maximum Limit")]
        [SerializeField]
        private GameEventFloat _onChangedMaxSpeedLimit = null;

        [BoxGroup("Minimum Limit")]
        [SerializeField]
        private bool _isMinActive = false;

        [BoxGroup("Minimum Limit")]
        [SerializeField]
        private float _minSpeed = 30f;

        [BoxGroup("Minimum Limit")]
        [SerializeField]
        private GameEventBool _setActiveMinLimitCallback = null;

        [BoxGroup("Minimum Limit")]
        [SerializeField]
        private GameEventBool _onActiveMinLimiter = null;

        [BoxGroup("Minimum Limit")]
        [SerializeField]
        private GameEventFloat _changedMinSpeedCallback = null;

        [BoxGroup("Minimum Limit")]
        [SerializeField]
        private GameEventFloat _onChangedMinSpeedLimit = null;

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventFloat _onCheckSpeed = null;

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventNoParam _onViolateSpeedLimit = null;

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventInt _addPointCallback = null;

        [FoldoutGroup("Debug", Order = 100)]
        [SerializeField, ReadOnly]
        private float _currentTimeBeetwenViolateMaxLimit = 0f;

        [FoldoutGroup("Debug", Order = 100)]
        [SerializeField, ReadOnly]
        private float _currentTimeBeetwenViolateMinLimit = 0f;

        [FoldoutGroup("Debug", Order = 100)]
        [SerializeField, ReadOnly]
        private bool _isMaxViolated = false;

        [FoldoutGroup("Debug", Order = 100)]
        [SerializeField, ReadOnly]
        private bool _isMinViolated = false;

        private const string VIOLATE_MAX_SPEED_MESSAGE = "Violate the maximum speed limit!";
        private const string VIOLATE_MIN_SPEED_MESSAGE = "Violate the minimum speed limit!";

        #endregion

        #region Properties

        public bool IsMaxActive
        {
            get { return _isMaxActive; }
            private set
            {
                _isMaxActive = value;
                _onActiveMaxLimiter.Invoke(value);
            }
        }

        public float MaxSpeed
        {
            get { return _maxSpeed; }
            private set
            {
                _maxSpeed = value;
                _onChangedMaxSpeedLimit.Invoke(value);
            }
        }

        public bool IsMinActive
        {
            get { return _isMinActive; }
            private set
            {
                _isMinActive = value;
                _onActiveMinLimiter.Invoke(value);
            }
        }

        public float MinSpeed
        {
            get { return _minSpeed; }
            private set
            {
                _minSpeed = value;
                _onChangedMinSpeedLimit.Invoke(_minSpeed);
            }
        }

        #endregion

        #region Mono

        private void Awake()
        {
            _onCheckSpeed.AddListener(OnCheckSpeed);
            _setActiveMaxLimitCallback.AddListener(SetActiveMaxSpeed);
            _setActiveMinLimitCallback.AddListener(SetActiveMinSpeed);
            _changedMaxSpeedCallback.AddListener(SetMaxSpeed);
            _changedMinSpeedCallback.AddListener(SetMinSpeed);
        }

        private void Update()
        {
            CheckMaxSpeedLimitViolatedTime();
            CheckMinSpeedLimitViolatedTime();
        }

        private void OnDestroy()
        {
            _onCheckSpeed.RemoveListener(OnCheckSpeed);
            _setActiveMaxLimitCallback.RemoveListener(SetActiveMaxSpeed);
            _setActiveMinLimitCallback.RemoveListener(SetActiveMinSpeed);
            _changedMaxSpeedCallback.RemoveListener(SetMaxSpeed);
            _changedMinSpeedCallback.RemoveListener(SetMinSpeed);
        }

        #endregion

        #region Methods

        private void CheckMaxSpeedLimitViolatedTime()
        {
            if (!IsMaxActive) return;

            if (!_isMaxViolated)
            {
                _currentTimeBeetwenViolateMaxLimit = _timeBetweenViolate;
                return;
            }

            if (_currentTimeBeetwenViolateMaxLimit > 0f)
            {
                _currentTimeBeetwenViolateMaxLimit -= Time.deltaTime;
                return;
            }

            // Violate max speed
            if (GlobalEvents.Instance != null)
            {
                GlobalEvents.Instance.SetNotificationCallback.Invoke(VIOLATE_MAX_SPEED_MESSAGE, (int)NotificationType.Danger);
                GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);

                GlobalEvents.Instance.AddMistakeCallback.Invoke(VIOLATE_MAX_SPEED_MESSAGE, 1);
            }

            OnViolate();

            _currentTimeBeetwenViolateMaxLimit = _timeBetweenViolate;
        }

        private void CheckMinSpeedLimitViolatedTime()
        {
            if (!IsMinActive) return;

            if (!_isMinViolated)
            {
                _currentTimeBeetwenViolateMinLimit = _timeBetweenViolate;
                return;
            }

            if (_currentTimeBeetwenViolateMinLimit > 0f)
            {
                _currentTimeBeetwenViolateMinLimit -= Time.deltaTime;
                return;
            }

            // Violate max speed
            if (GlobalEvents.Instance != null)
            {
                GlobalEvents.Instance.SetNotificationCallback.Invoke(VIOLATE_MIN_SPEED_MESSAGE, (int)NotificationType.Danger);
                GlobalEvents.Instance.StartNoticationCallback.Invoke(1f, 3f, 1f);

                GlobalEvents.Instance.AddMistakeCallback.Invoke(VIOLATE_MIN_SPEED_MESSAGE, 1);
            }

            OnViolate();

            _currentTimeBeetwenViolateMinLimit = _timeBetweenViolate;
        }

        private void OnCheckSpeed(float speed)
        {
            CheckMaxSpeed(speed);
            CheckMinSpeed(speed);
        }

        private void CheckMaxSpeed(float speed)
        {
            if (!_isMaxActive) return;

            _isMaxViolated = speed > MaxSpeed;
        }

        private void CheckMinSpeed(float speed)
        {
            if (!_isMinActive) return;

            _isMinViolated = speed < MinSpeed;
        }

        private void SetActiveMaxSpeed(bool active)
        {
            IsMaxActive = active;

            if (active)
            {
                _currentTimeBeetwenViolateMaxLimit = _timeBetweenViolate;
            }
        }

        private void SetActiveMinSpeed(bool active)
        {
            IsMinActive = active;

            if (active)
            {
                _currentTimeBeetwenViolateMaxLimit = _timeBetweenViolate;
            }
        }

        private void SetMaxSpeed(float speed)
        {
            MaxSpeed = speed;
        }

        private void SetMinSpeed(float speed)
        {
            MinSpeed = speed;
        }

        #endregion

        #region IObstacle

        public void OnViolate()
        {
            _onViolateSpeedLimit.Invoke();
            _addPointCallback.Invoke(-_violatePoint);
        }

        #endregion
    }
}
