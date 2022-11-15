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

    [BoxGroup("Minimum Limit")]
    [SerializeField]
    private bool _isMinActive = false;

    [BoxGroup("Minimum Limit")]
    [SerializeField]
    private float _minSpeed = 30f;

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

    #region Mono

    private void Awake()
    {
        _onCheckSpeed?.AddListener(OnCheckSpeed);
    }

    private void Update()
    {
        CheckCanViolated();
    }

    private void OnDestroy()
    {
        _onCheckSpeed?.RemoveListener(OnCheckSpeed);
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
