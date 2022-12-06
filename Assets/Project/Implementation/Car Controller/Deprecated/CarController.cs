
using UnityEngine;

namespace DrivingSimulation
{
    public class CarController : MonoBehaviour
    {
        public CarInput _input = null;

        [Header("Properties")]
        [SerializeField] private float _motorForce = 1f;
        [SerializeField] private float _brakeForce = 1f;
        [SerializeField] private float maxSteerAngle = 70f;

        [Header("Wheel Collider")]
        [SerializeField] private WheelCollider _frontLeftWheelCollider = null;
        [SerializeField] private WheelCollider _frontRightWheelCollider = null;
        [SerializeField] private WheelCollider _rearLeftWheelCollider = null;
        [SerializeField] private WheelCollider _rearRightWheelCollider = null;

        [Header("Wheel Transform")]
        [SerializeField] private Transform _frontLeftWheelTransform = null;
        [SerializeField] private Transform _frontRightWheelTransform = null;
        [SerializeField] private Transform _rearLeftWheelTransform = null;
        [SerializeField] private Transform _rearRightWheelTransform = null;

        private Vector2 _inputDirection = Vector2.zero;
        private float _currentSteerAngle = 0f;
        private float _currentBrakeForce = 0f;
        private bool _isBraking = false;

        private void Awake()
        {
            _input = new CarInput();
            _input.Controller.Brake.started += (ctx) => { _isBraking = true; };
            _input.Controller.Brake.canceled += (ctx) => { _isBraking = false; };

            _input.Enable();
        }

        private void OnDestroy()
        {
            _input.Controller.Brake.Dispose();
            _input.Disable();
        }

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
            _inputDirection = new Vector2(_input.Controller.Turn.ReadValue<float>(), _input.Controller.Run.ReadValue<float>());
        }

        private void HandleMotor()
        {
            _rearLeftWheelCollider.motorTorque = _inputDirection.y * _motorForce;
            _rearRightWheelCollider.motorTorque = _inputDirection.y * _motorForce;
            
            _currentBrakeForce = _isBraking ? _brakeForce : 0f;
            ApplyBrake();
        }

        private void ApplyBrake()
        {
            _frontRightWheelCollider.brakeTorque = _currentBrakeForce;
            _frontLeftWheelCollider.brakeTorque = _currentBrakeForce;
            _rearLeftWheelCollider.brakeTorque = _currentBrakeForce;
            _rearRightWheelCollider.brakeTorque = _currentBrakeForce;
        }

        private void HandleSteering()
        {
            _currentSteerAngle = maxSteerAngle * _inputDirection.x;
            _frontLeftWheelCollider.steerAngle = _currentSteerAngle;
            _frontRightWheelCollider.steerAngle = _currentSteerAngle;
        }

        private void UpdateWheels()
        {
            UpdateSingleWheel(_frontLeftWheelCollider, _frontLeftWheelTransform);
            UpdateSingleWheel(_frontRightWheelCollider, _frontRightWheelTransform);
            UpdateSingleWheel(_rearRightWheelCollider, _rearRightWheelTransform);
            UpdateSingleWheel(_rearLeftWheelCollider, _rearLeftWheelTransform);
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
}
