using Sirenix.OdinInspector;
using SOGameEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLimiter : MonoBehaviour, IObstacle
{
    #region Variables

    [BoxGroup("Data")]
    [SerializeField]
    private int _violatePoint = 50;

    [BoxGroup("Data")]
    [SerializeField]
    private float _timeBetweenViolate = 50;

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
    private float _curentTimeBetweenViolated = 0f;

    [FoldoutGroup("Debug", Order = 100)]
    [SerializeField, ReadOnly]
    private bool _canViolated = true;

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
            _onChangedMinSpeedLimit.Invoke(value);
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
        CheckCanViolated();
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

    private void CheckCanViolated()
    {
        if (_curentTimeBetweenViolated > 0f)
        {
            _curentTimeBetweenViolated -= Time.deltaTime;
            return;
        }

        _curentTimeBetweenViolated = 0f;
        _canViolated = true;
    }

    private void OnCheckSpeed(float speed)
    {
        CheckMaxSpeed(speed);
        CheckMinSpeed(speed);
    }

    private void CheckMaxSpeed(float speed)
    {
        if (!_isMaxActive) return;

        if (speed > _maxSpeed)
        {
            OnViolate();
        }
    }

    private void CheckMinSpeed(float speed)
    {
        if (!_isMinActive) return;

        if (speed < _minSpeed)
        {
            OnViolate();
        }
    }

    private void SetActiveMaxSpeed(bool active)
    {
        IsMaxActive = active;
    }

    private void SetActiveMinSpeed(bool active)
    {
        IsMinActive = active;
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
        if (!_canViolated) return;

        Debug.Log($"On violate speed limiter!");

        _onViolateSpeedLimit?.Invoke();
        _addPointCallback?.Invoke(-_violatePoint);

        _canViolated = false;
        _curentTimeBetweenViolated = _timeBetweenViolate;
    }

    #endregion
}
