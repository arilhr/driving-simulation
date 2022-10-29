using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeController : MonoBehaviour
{
    public const string HORIZONTAL_INPUT = "Horizontal";
    public const string VERTICAL_INPUT = "Vertical";

    [Header("Properties")]
    [SerializeField] private float _motorForce = 1000f;
    [SerializeField] private float _brakeForce = 300f;
    [SerializeField] private float maxSteerAngle = 30f;

    [Header("Wheel Collider")]
    [SerializeField] private WheelCollider _frontWheelCollider = null;
    [SerializeField] private WheelCollider _backWheelCollider = null;

    [Header("Wheel Transform")]
    [SerializeField] private Transform _frontWheelModel = null;
    [SerializeField] private Transform _backWheelModel = null;

    private Vector2 _inputDirection = Vector2.zero;
    private float _currentSteerAngle = 0f;
    private float _currentBrakeForce = 0f;
    private bool _isBraking = false;

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        _inputDirection = new Vector2(Input.GetAxis(HORIZONTAL_INPUT), Input.GetAxis(VERTICAL_INPUT));
        _isBraking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        _backWheelCollider.motorTorque = _inputDirection.y * _motorForce;

        _currentBrakeForce = _isBraking ? _brakeForce : 0f;

        ApplyBrake();
    }

    private void ApplyBrake()
    {
        _frontWheelCollider.brakeTorque = _currentBrakeForce;
        _backWheelCollider.brakeTorque = _currentBrakeForce;
    }

    private void HandleSteering()
    {
        _currentSteerAngle = maxSteerAngle * _inputDirection.x;
        _frontWheelCollider.steerAngle = _currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(_frontWheelCollider, _frontWheelModel);
        UpdateSingleWheel(_backWheelCollider, _backWheelModel);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;

        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}
