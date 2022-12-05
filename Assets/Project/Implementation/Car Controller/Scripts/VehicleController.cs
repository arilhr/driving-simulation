using Sirenix.OdinInspector;
using SOGameEvents;
using System;
using System.Collections;
using UnityEngine;

namespace DrivingSimulation
{
    #region Wheel

    [Serializable]
    public class Wheel
    {
        [Tooltip("In this variable you must associate the mesh of the wheel of this class.")]
        public Transform wheelMesh;
        [Tooltip("In this variable you must associate the collider of the wheel of this class.")]
        public WheelCollider wheelCollider;
        [Tooltip("If this variable is true, this wheel will receive engine torque.")]
        public bool wheelDrive = true;
        [Tooltip("If this variable is true, this wheel will receive handbrake force.")]
        public bool wheelHandBrake = true;
        [Range(-2.0f, 2.0f)]
        [Tooltip("In this variable you can set the horizontal offset of the sliding mark of this wheel.")]
        public float skidMarkShift = 0.0f;
        [Tooltip("If this variable is true, the wheel associated with this index will receive rotation defined by the flywheel.")]
        public bool wheelTurn = false;
        
        [HideInInspector] public Vector3 wheelWorldPosition;
        [HideInInspector] public Mesh rendSKDmarks;
        [HideInInspector] public bool generateSkidBool;
        [HideInInspector] public float wheelColliderRPM;
        [HideInInspector] public float forwardSkid;
        [HideInInspector] public float sidewaysSkid;
    }

    [Serializable]
    public class Wheels
    {
        [Space(10)]
        [Tooltip("The front right wheel collider must be associated with this variable")]
        public Wheel rightFrontWheel;
        [Tooltip("The front left wheel collider must be associated with this variable")]
        public Wheel leftFrontWheel;
        [Tooltip("The rear right wheel collider should be associated with this variable")]
        public Wheel rightRearWheel;
        [Tooltip("The rear left wheel collider should be associated with this variable")]
        public Wheel leftRearWheel;
    }

    #endregion

    #region Aerodynamic

    [Serializable]
    public class AerodynamicAdjustment
    {
        [Tooltip("If this variable is true, the script will simulate a force down on the vehicle, leaving jumps more realistic.")]
        public bool extraGravity = true;
        [Range(0.0f, 10.0f)]
        [Tooltip("This variable defines how much force will be added to the vehicle suspension to avoid rotations. This makes the vehicle more rigid and harder to knock over.")]
        public float feelingHeavy = 1.0f;
        [Range(0.00f, 1.0f)]
        [Tooltip("This variable defines the amount of force that will be simulated in the vehicle while it is tilted, the steeper it is, the lower the force applied. Values too high make the vehicle too tight and prevent it from slipping.")]
        public float downForceAngleFactor = 0.2f;
        [Range(0, 1.0f)]
        [Tooltip("This variable defines how much force will be simulated in the vehicle while on flat terrain. Values too high cause the suspension to reach the spring limit.")]
        public float verticalDownForce = 0.8f;
        [Range(0, 3)]
        [Tooltip("This variable defines a minimum force value that will be simulated. The value corresponds to the mass of the vehicle times the value of this variable.")]
        public int minDownForceValue = 2;
    }

    #endregion

    #region Vehicle Adjusment

    [Serializable]
    public class VehicleAdjustment
    {
        [Tooltip("If this variable is true, the vehicle will start with the engine running. But this only applies if the player starts inside this vehicle.")]
        public bool startOn = true;
        [Range(500, 2000000)]
        [Tooltip("In this variable you must define the mass that the vehicle will have. Common vehicles usually have a mass around 1500")]
        public int vehicleMass = 2000;
        [Tooltip("In this variable there are some variables that allow to improve the control of the vehicle.")]
        public StabilizeTurnsClassFree improveControl;
        [Tooltip("In this class you can adjust some forces that the vehicle receives, such as gravity simulation.")]
        public AerodynamicAdjustment _aerodynamics;
        [Space(10)]
        [Tooltip("In this variable an empty object affiliated to the vehicle should be associated with the center position of the vehicle, perhaps displaced slightly downward, with the intention of representing the center of mass of the vehicle.")]
        public Transform centerOfMass;
        [Tooltip("The steering wheel of the vehicle")]
        public GameObject volant;
        [HideInInspector]
        public AnimationCurve angle_x_Velocity = new AnimationCurve(new Keyframe(0, 1), new Keyframe(500, 0.8f));
    }

    [Serializable]
    public class StabilizeTurnsClassFree
    {
        [Range(0.0f, 1.2f)]
        [Tooltip("How much the code will stabilize the vehicle's skidding.")]
        public float tireSlipsFactor = 0.85f;
        [Range(0.1f, 2.0f)]
        [Tooltip("This variable defines how much lateral force the vehicle will receive when the steering wheel is rotated. This helps the vehicle to rotate more realistically.")]
        public float helpToTurn = 0.35f;
        [Range(0.1f, 1.0f)]
        [Tooltip("This variable defines how fast the vehicle will straighten automatically. This occurs naturally in a vehicle when it exits a curve.")]
        public float helpToStraightenOut = 0.1f;
        [Range(0.1f, 5.0f)]
        [Tooltip("This variable defines how much downforce the vehicle will receive. This helps to simulate a more realistic gravity, but should be set up carefully so as not to make some surreal situations.")]
        public float downForce = 2.0f;
    }

    #endregion

    #region Vehicle Torque

    [Serializable]
    public class TorqueAdjustment
    {
        [Range(20, 420)]
        [Tooltip("This variable sets the maximum speed that the vehicle can achieve. It must be configured on the KMh unit")]
        public int maxVelocityKMh = 250;
        [Range(0.5f, 2000.0f)]
        [Tooltip("This variable defines the torque that the motor of the vehicle will have.")]
        public float engineTorque = 3;
        [Range(2, 12)]
        [Tooltip("This variable defines the number of gears that the vehicle will have.")]
        public int numberOfGears = 6;
        [Range(0.5f, 2.0f)]
        [Tooltip("This variable defines the speed range of each gear. The higher the range, the faster the vehicle goes, however, the torque is relatively lower.")]
        public float speedOfGear = 1.5f;
        [Range(0.5f, 2.0f)]
        [Tooltip("In this variable, you can manually adjust the torque that each gear has. But it is not advisable to change these values.")]
        public float[] manualAdjustmentOfTorques = new float[12] { 1.1f, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

        [HideInInspector]
        public AnimationCurve[] gears = new AnimationCurve[12]{
            new AnimationCurve(new Keyframe(0, 1.5f),new Keyframe(10, 2.0f),new Keyframe(30, 0)),
            new AnimationCurve(new Keyframe(0, 0.2f),new Keyframe(30, 1),new Keyframe(45, 0)),
            new AnimationCurve(new Keyframe(0, 0.2f),new Keyframe(45, 1),new Keyframe(60, 0)),
            new AnimationCurve(new Keyframe(15, 0.0f),new Keyframe(60, 1),new Keyframe(75, 0)),
            new AnimationCurve(new Keyframe(30, 0.0f),new Keyframe(75, 1),new Keyframe(90, 0)),
            new AnimationCurve(new Keyframe(45, 0.0f),new Keyframe(90, 1),new Keyframe(105, 0)),
            new AnimationCurve(new Keyframe(60, 0.0f),new Keyframe(105, 1),new Keyframe(120, 0)),
            new AnimationCurve(new Keyframe(75, 0.0f),new Keyframe(120, 1),new Keyframe(135, 0)),
            new AnimationCurve(new Keyframe(90, 0.0f),new Keyframe(135, 1),new Keyframe(150, 0)),
            new AnimationCurve(new Keyframe(105, 0.0f),new Keyframe(150, 1),new Keyframe(165, 0)),
            new AnimationCurve(new Keyframe(120, 0.0f),new Keyframe(165, 1),new Keyframe(180, 0)),
            new AnimationCurve(new Keyframe(135, 0.0f),new Keyframe(180, 1),new Keyframe(195, 0)),
        };

        [HideInInspector] public int[] minVelocityGears = new int[12] { 0, 15, 30, 45, 60, 75, 90, 105, 120, 135, 150, 165 };
        [HideInInspector] public int[] idealVelocityGears = new int[12] { 10, 30, 45, 60, 75, 90, 105, 120, 135, 150, 165, 180 };
        [HideInInspector] public int[] maxVelocityGears = new int[12] { 30, 45, 60, 75, 90, 105, 120, 135, 150, 165, 180, 195 };
    }

    #endregion

    #region Vehicle Skid

    [Serializable]
    public class VehicleSkidMarks
    {
        [Range(0.1f, 6.0f)]
        [Tooltip("This variable defines the width of the vehicle's skid trace.")]
        public float standardBrandWidth = 0.3f;
        [Range(1.0f, 10.0f)]
        [Tooltip("This variable sets the sensitivity of the vehicle to start generating traces of skidding. The more sensitive, the easier to generate the traces.")]
        public float sensibility = 2.0f;
        [Tooltip("This variable sets the default color of the skid marks.")]
        public Color standardColor = new Color(0.15f, 0.15f, 0.15f, 0);
    }

    #endregion

    [RequireComponent(typeof(Rigidbody))]
    public class VehicleController : MonoBehaviour
    {
        #region Wheel Data

        [BoxGroup("Wheel Data")]
        [SerializeField, Range(5,15000)]
        private int _wheelMass = 100;

        [Space(10f)]
        [BoxGroup("Wheel Data")]
        [SerializeField]
        private Wheels _wheels;

        #endregion

        #region Data

        [BoxGroup("Vehicle Torque")]
        [Tooltip("In this class you can configure the vehicle torque, number of gears and their respective torques.")]
        [SerializeField]
        private TorqueAdjustment _vehicleTorque;

        [BoxGroup("Vehicle Adjusment")]
        [Tooltip("In this class you can adjust various settings that allow changing the way the vehicle is controlled, as well as the initial state of some variables, such as the engine and the brake.")]
        [SerializeField]
        private VehicleAdjustment _vehicleSettings;

        [BoxGroup("Vehicle Skid Marks")]
        [Tooltip("In this class, you can adjust all preferences in relation to vehicle skid marks, such as color, width, among other options.")]
        [SerializeField]
        private VehicleSkidMarks _skidMarks;

        [BoxGroup("Events")]
        [SerializeField]
        private GameEventFloat _onUpdateSpeed = null;

        [BoxGroup("Read Only", Order = 100f), ReadOnly]
        public float KMh;

        [BoxGroup("Read Only", Order = 100f), ReadOnly]
        public int CurrentGear;

        [BoxGroup("Read Only", Order = 100f), ReadOnly]
        public bool disableVehicle = false;

        [BoxGroup("Read Only", Order = 100f), ReadOnly]
        public bool handBrakeTrue;

        [BoxGroup("Read Only", Order = 100f), ReadOnly]
        public bool isInsideTheCar;

        #endregion

        #region Private Data

        private bool _changinGearsAuto;
        private bool _theEngineIsRunning;
        private bool _enableEngineSound;
        private bool _youCanCall;
        private bool _brakingAuto;
        private bool _colliding;
        private bool _isBraking;

        private float _brakeVerticalInput;
        private float _handBrake_Input;
        private float _totalFootBrake;
        private float _totalHandBrake;
        private float _absBrakeInput;
        private float _absSpeedFactor;

        private float _torqueM;
        private float _rpmTempTorque;
        private float _clampInputTorque;
        private float _adjustTorque;

        private bool _wheelFrontRightIsGrounded;
        private bool _wheelFrontLeftIsGrounded;
        private bool _wheelRearRightIsGrounded;
        private bool _wheelRearLeftIsGrounded;

        private int _groundedWheels;
        private float _sumRPM;
        private float _mediumRPM;
        private float _angle1Ref;
        private float _angle2Volant;
        private float _maxAngleVolant;
        private float _volantStartRotation;
        private float _minPitchAud;
        private float _leftDifferential;
        private float _rightDifferential;
        private float _timeAutoGear;
        private float _reverseForce;
        private float _engineInput;
        private float _angleRefVolant;
        private float _pitchAUD = 1;
        private float _speedLerpSound = 1;
        private float _engineSoundFactor;
        private float _vehicleScale;

        private Vector2 _tireSL;
        private Vector2 _tireFO;

        private Vector3 _lateralForcePointTemp;
        private Vector3 _forwardForceTemp;
        private Vector3 _lateralForceTemp;
        private float _distanceXForceTemp;

        private float _sensImpactFR;
	    private float _sensImpactFL;
	    private float _sensImpactRR;
	    private float _sensImpactRL;
	    private float _additionalCurrentGravity;
	    private float _currentBrakeValue;
	    private float _forceEngineBrake;

        private float _leftFrontForce;
        private float _rightFrontForce;
        private float _leftRearForce;
        private float _rightRearForce;
        private float _roolForce1;
        private float _roolForce2;

        private float _lastRightForwardPositionY;
        private float _lastLeftForwardPositionY;
        private float _lastRightRearPositionY;
        private float _lastLeftRearPositionY;

        private float _currentDownForceVehicle;
        private float _downForceTempLerp;
        private float _downForceUpdateRef;
        private float _gravityValueFixedUpdate;
        private float _downForceValueFixedUpdate;
        private float _inclinationFactorForcesDown;

        private Vector2 _tireSlipTireSlips;
        private Vector2 _tireForceTireSlips;
        private Vector2 _localRigForceTireSlips;
        private Vector2 _localVelocityWheelTireSlips;
        private Vector2 _localSurfaceForceDTireSlips;
        private Vector2 _rawTireForceTireSlips;
        private Vector2 _tempLocalVelocityVector2;
        private Vector3 _tempWheelVelocityVector3;
        private Vector3 _velocityLocalWheelTemp;
        private Vector2 _surfaceLocalForce;
        private Vector3 _surfaceLocalForceTemp;
        private Vector3 _wheelSpeedLocalSurface;
        private Vector3 _downForceUPTemp;
        private float _normalTemp;
        private float _forceFactorTempLocalSurface;
        private float _downForceTireSlips;
        private float _estimatedSprungMass;
        private float _angularWheelVelocityTireSlips;
        private float _wheelMaxBrakeSlip;
        private float _minSlipYTireSlips;
        private float _maxFyTireSlips;

        private bool _isGroundedExtraW;
        private Vector3 _axisFromRotate;
        private Vector3 _torqueForceAirRotation;

        private Vector3 _vectorMeshPos1;
        private Vector3 _vectorMeshPos2;
        private Vector3 _vectorMeshPos3;
        private Vector3 _vectorMeshPos4;
        private Vector3 _vectorMeshPosTemp;
        private Quaternion _quatMesh1;
        private Quaternion _quatMesh2;
        private Quaternion _quatMesh3;
        private Quaternion _quatMesh4;
        private Quaternion _quatMeshTemp;

        private float _tempAlphaSkidMarks;
        private WheelHit _tempWheelHit;

        private float _verticalInput = 0f;
        private float _horizontalInput = 0f;
        private bool _brakeInput = false;

        #endregion

        #region References

        private Rigidbody _rigidbody;
        private CarInput _carInput;

        #endregion

        #region Mono

        private void Awake()
        {
            _carInput = new CarInput();

            _rigidbody = GetComponent<Rigidbody>();
            _theEngineIsRunning = true;
        }

        private void OnEnable()
        {
            _carInput.Enable();
        }

        private void Start()
        {
            SetValues();
        }

        void Update()
        {
            UpdateInput();

            GetWheelsIsGrounded();

            KMh = _rigidbody.velocity.magnitude * 3.6f;
            _onUpdateSpeed.Invoke(KMh);

            _inclinationFactorForcesDown = Mathf.Clamp(Mathf.Abs(Vector3.Dot(Vector3.up, transform.up)), _vehicleSettings._aerodynamics.downForceAngleFactor, 1.0f);

            if (_wheelFrontRightIsGrounded || _wheelFrontLeftIsGrounded || _wheelRearRightIsGrounded || _wheelRearLeftIsGrounded)
            {
                _downForceTempLerp = (_rigidbody.mass * _vehicleSettings._aerodynamics.minDownForceValue + (_vehicleSettings._aerodynamics.verticalDownForce * Mathf.Abs(KMh * 3.0f) * (_rigidbody.mass / 125.0f))) * _inclinationFactorForcesDown;
                _downForceUpdateRef = Mathf.Lerp(_downForceUpdateRef, _downForceTempLerp, Time.deltaTime * 2.5f);
            }
            else
            {
                _downForceTempLerp = _rigidbody.mass * _vehicleSettings._aerodynamics.minDownForceValue * _inclinationFactorForcesDown;
                _downForceUpdateRef = Mathf.Lerp(_downForceUpdateRef, _downForceTempLerp, Time.deltaTime * 2.5f);
            }

            _rigidbody.drag = Mathf.Clamp((KMh / _vehicleTorque.maxVelocityKMh) * 0.075f, 0.001f, 0.075f);

            ChangeGearInput();
            DiscoverAverageRpm();
            UpdateWheelMeshes();
            AutomaticGears();
        }

        private void FixedUpdate()
        {
            ApplyTorque();
            Brakes();
            Volant();
            StabilizeWheelRPM();
            StabilizeVehicleRollForces();
            StabilizeAirRotation();
            StabilizeAngularRotation();

            if (_vehicleSettings._aerodynamics.extraGravity)
            {
                _gravityValueFixedUpdate = 0;
                if (_wheelFrontRightIsGrounded && _wheelFrontLeftIsGrounded && _wheelRearLeftIsGrounded && _wheelRearRightIsGrounded)
                {
                    _gravityValueFixedUpdate = 4.0f * _rigidbody.mass * Mathf.Clamp((KMh / _vehicleTorque.maxVelocityKMh), 0.05f, 1.0f);
                }
                else
                {
                    _gravityValueFixedUpdate = 4.0f * _rigidbody.mass * 3.0f;
                }
                _additionalCurrentGravity = Mathf.Lerp(_additionalCurrentGravity, _gravityValueFixedUpdate, Time.deltaTime);
                _rigidbody.AddForce(Vector3.down * _additionalCurrentGravity);
            }

            _downForceValueFixedUpdate = _vehicleSettings.improveControl.downForce * (((KMh / 10.0f) + 0.3f) / 2.5f);
            _currentDownForceVehicle = Mathf.Clamp(Mathf.Lerp(_currentDownForceVehicle, _downForceValueFixedUpdate, Time.deltaTime * 2.0f), 0.1f, 4.0f);

            _rigidbody.AddForce(-transform.up * _downForceUpdateRef);

            SetWheelForces(_wheels.rightFrontWheel.wheelCollider);
            SetWheelForces(_wheels.leftFrontWheel.wheelCollider);
            SetWheelForces(_wheels.rightRearWheel.wheelCollider);
            SetWheelForces(_wheels.leftRearWheel.wheelCollider);

            if (_wheelFrontRightIsGrounded && _wheelFrontLeftIsGrounded && _wheelRearLeftIsGrounded && _wheelRearRightIsGrounded)
            {
                _absSpeedFactor = Mathf.Clamp(KMh, 70, 150);
                if (CurrentGear > 0 && _mediumRPM > 0)
                {
                    _absBrakeInput = Mathf.Abs(Mathf.Clamp(_verticalInput, -1.0f, 0.0f));
                }
                else if (CurrentGear <= 0 && _mediumRPM < 0)
                {
                    _absBrakeInput = Mathf.Abs(Mathf.Clamp(_verticalInput, 0.0f, 1.0f)) * -1;
                }
                else
                {
                    _absBrakeInput = 0.0f;
                }
                if (_isBraking && Mathf.Abs(KMh) > 1.2f)
                {
                    _rigidbody.AddForce(-transform.forward * _absSpeedFactor * _rigidbody.mass * 0.125f * _absBrakeInput);
                }
            }
        }

        void OnCollisionStay()
        {
            _colliding = true;
        }

        void OnCollisionExit()
        {
            _colliding = false;
        }

        private void OnDisable()
        {
            _carInput.Disable();
        }

        #endregion

        #region Methods

        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F) { angle += 360F; }
            if (angle > 360F) { angle -= 360F; }
            return Mathf.Clamp(angle, min, max);
        }

        void SetValues()
        {
            _vehicleScale = transform.lossyScale.y;
            _forceEngineBrake = 0.75f * _vehicleSettings.vehicleMass;

            _currentDownForceVehicle = _vehicleSettings.improveControl.downForce;

            handBrakeTrue = false;

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = true;
            _rigidbody.mass = _vehicleSettings.vehicleMass;
            _rigidbody.drag = 0.0f;
            _rigidbody.angularDrag = 0.05f;
            _rigidbody.maxAngularVelocity = 14.0f;
            _rigidbody.maxDepenetrationVelocity = 8.0f;
            _additionalCurrentGravity = 4.0f * _rigidbody.mass;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            WheelCollider WheelColliders = GetComponentInChildren<WheelCollider>();
            WheelColliders.ConfigureVehicleSubsteps(1000.0f, 20, 20);

            if (_vehicleSettings.centerOfMass)
            {
                _rigidbody.centerOfMass = transform.InverseTransformPoint(_vehicleSettings.centerOfMass.position);
            }
            else
            {
                _rigidbody.centerOfMass = Vector3.zero;
            }
            if (_vehicleSettings.volant)
            {
                _volantStartRotation = _vehicleSettings.volant.transform.localEulerAngles.z;
            }

            _speedLerpSound = 5;

            _lastRightForwardPositionY = _wheels.rightFrontWheel.wheelMesh.transform.localPosition.y;
            _lastLeftForwardPositionY = _wheels.leftFrontWheel.wheelMesh.transform.localPosition.y;
            _lastRightRearPositionY = _wheels.rightRearWheel.wheelMesh.transform.localPosition.y;
            _lastLeftRearPositionY = _wheels.leftRearWheel.wheelMesh.transform.localPosition.y;

            _sensImpactFR = 0.075f * (2.65f * _wheels.rightFrontWheel.wheelCollider.radius);
            _sensImpactFL = 0.075f * (2.65f * _wheels.leftFrontWheel.wheelCollider.radius);
            _sensImpactRR = 0.075f * (2.65f * _wheels.rightRearWheel.wheelCollider.radius);
            _sensImpactRL = 0.075f * (2.65f * _wheels.leftRearWheel.wheelCollider.radius);
        }

        void DiscoverAverageRpm()
        {
            _groundedWheels = 0;
            _sumRPM = 0;
            _wheels.rightFrontWheel.wheelColliderRPM = _wheels.rightFrontWheel.wheelCollider.rpm;

            // Checking grounded wheel, and if grounded add RPM
            if (_wheelFrontRightIsGrounded)
            {
                _groundedWheels++;
                _sumRPM += _wheels.rightFrontWheel.wheelColliderRPM;
            }

            _wheels.leftFrontWheel.wheelColliderRPM = _wheels.leftFrontWheel.wheelCollider.rpm;
            if (_wheelFrontLeftIsGrounded)
            {
                _groundedWheels++;
                _sumRPM += _wheels.leftFrontWheel.wheelColliderRPM;
            }

            _wheels.rightRearWheel.wheelColliderRPM = _wheels.rightRearWheel.wheelCollider.rpm;
            if (_wheelRearRightIsGrounded)
            {
                _groundedWheels++;
                _sumRPM += _wheels.rightRearWheel.wheelColliderRPM;
            }

            _wheels.leftRearWheel.wheelColliderRPM = _wheels.leftRearWheel.wheelCollider.rpm;
            if (_wheelRearLeftIsGrounded)
            {
                _groundedWheels++;
                _sumRPM += _wheels.leftRearWheel.wheelColliderRPM;
            }
            _mediumRPM = _sumRPM / _groundedWheels;
            if (Mathf.Abs(_mediumRPM) < 0.01f)
            {
                _mediumRPM = 0.0f;
            }
        }

        void UpdateInput()
        {
            _verticalInput = _carInput.Controller.Run.ReadValue<float>();
            _horizontalInput = _carInput.Controller.Turn.ReadValue<float>();

            _brakeInput = _carInput.Controller.Brake.IsPressed();
        }

        #region Wheel Manager

        void UpdateWheelMeshes()
        {
            _wheels.rightFrontWheel.wheelCollider.GetWorldPose(out _vectorMeshPos1, out _quatMesh1);
            _wheels.rightFrontWheel.wheelWorldPosition = _wheels.rightFrontWheel.wheelMesh.position = _vectorMeshPos1;
            _wheels.rightFrontWheel.wheelMesh.rotation = _quatMesh1;

            _wheels.leftFrontWheel.wheelCollider.GetWorldPose(out _vectorMeshPos2, out _quatMesh2);
            _wheels.leftFrontWheel.wheelWorldPosition = _wheels.leftFrontWheel.wheelMesh.position = _vectorMeshPos2;
            _wheels.leftFrontWheel.wheelMesh.rotation = _quatMesh2;

            _wheels.rightRearWheel.wheelCollider.GetWorldPose(out _vectorMeshPos3, out _quatMesh3);
            _wheels.rightRearWheel.wheelWorldPosition = _wheels.rightRearWheel.wheelMesh.position = _vectorMeshPos3;
            _wheels.rightRearWheel.wheelMesh.rotation = _quatMesh3;

            _wheels.leftRearWheel.wheelCollider.GetWorldPose(out _vectorMeshPos4, out _quatMesh4);
            _wheels.leftRearWheel.wheelWorldPosition = _wheels.leftRearWheel.wheelMesh.position = _vectorMeshPos4;
            _wheels.leftRearWheel.wheelMesh.rotation = _quatMesh4;
        }

        void GetWheelsIsGrounded()
        {
            _wheelFrontRightIsGrounded = _wheels.rightFrontWheel.wheelCollider.isGrounded;
            _wheelFrontLeftIsGrounded = _wheels.leftFrontWheel.wheelCollider.isGrounded;
            _wheelRearRightIsGrounded = _wheels.rightRearWheel.wheelCollider.isGrounded;
            _wheelRearLeftIsGrounded = _wheels.leftRearWheel.wheelCollider.isGrounded;
        }

        void SetWheelForces(WheelCollider wheelCollider)
        {
            wheelCollider.GetGroundHit(out _tempWheelHit);

            if (wheelCollider.isGrounded)
            {
                TireSlips(wheelCollider, _tempWheelHit);
                _distanceXForceTemp = _rigidbody.centerOfMass.y - transform.InverseTransformPoint(wheelCollider.transform.position).y + wheelCollider.radius + (1.0f - wheelCollider.suspensionSpring.targetPosition) * wheelCollider.suspensionDistance;
                _lateralForcePointTemp = _tempWheelHit.point + wheelCollider.transform.up * _vehicleSettings.improveControl.helpToStraightenOut * _distanceXForceTemp;
                _forwardForceTemp = _tempWheelHit.forwardDir * (_tireFO.y) * 3.0f;
                _lateralForceTemp = _tempWheelHit.sidewaysDir * (_tireFO.x);
                if (Mathf.Abs(_horizontalInput) > 0.1f && wheelCollider.steerAngle != 0.0f && Mathf.Sign(wheelCollider.steerAngle) != Mathf.Sign(_tireSL.x))
                {
                    _lateralForcePointTemp += _tempWheelHit.forwardDir * _vehicleSettings.improveControl.helpToTurn;
                }
                _rigidbody.AddForceAtPosition(_forwardForceTemp, _tempWheelHit.point);
                _rigidbody.AddForceAtPosition(_lateralForceTemp, _lateralForcePointTemp);
            }
        }

        public Vector2 WheelLocalVelocity(WheelHit wheelHit)
        {
            _tempLocalVelocityVector2 = new Vector2(0, 0);
            _tempWheelVelocityVector3 = _rigidbody.GetPointVelocity(wheelHit.point);
            _velocityLocalWheelTemp = _tempWheelVelocityVector3 - Vector3.Project(_tempWheelVelocityVector3, wheelHit.normal);
            _tempLocalVelocityVector2.y = Vector3.Dot(wheelHit.forwardDir, _velocityLocalWheelTemp);
            _tempLocalVelocityVector2.x = Vector3.Dot(wheelHit.sidewaysDir, _velocityLocalWheelTemp);

            return _tempLocalVelocityVector2;
        }

        public float AngularVelocity(Vector2 localVelocityVector, WheelCollider wheelCollider)
        {
            wheelCollider.GetGroundHit(out _tempWheelHit);
            return (localVelocityVector.y + (_tempWheelHit.sidewaysSlip * ((Mathf.Abs(_verticalInput) + Mathf.Abs(_horizontalInput)) / 2.0f) * (-2.0f))) / wheelCollider.radius;
        }

        public Vector2 LocalSurfaceForce(WheelHit wheelHit)
        {
            _wheelSpeedLocalSurface = _rigidbody.GetPointVelocity(wheelHit.point);
            _forceFactorTempLocalSurface = Mathf.InverseLerp(1.0f, 0.25f, (_wheelSpeedLocalSurface - Vector3.Project(_wheelSpeedLocalSurface, wheelHit.normal)).sqrMagnitude);
            if (_forceFactorTempLocalSurface > 0.0f)
            {
                _normalTemp = Vector3.Dot(Vector3.up, wheelHit.normal);
                if (_normalTemp > 0.000001f)
                {
                    _downForceUPTemp = Vector3.up * wheelHit.force / _normalTemp;
                    _surfaceLocalForceTemp = _downForceUPTemp - Vector3.Project(_downForceUPTemp, wheelHit.normal);
                }
                else
                {
                    _surfaceLocalForceTemp = Vector3.up * 1000000.0f;
                }

                _surfaceLocalForce.y = Vector3.Dot(wheelHit.forwardDir, _surfaceLocalForceTemp);
                _surfaceLocalForce.x = Vector3.Dot(wheelHit.sidewaysDir, _surfaceLocalForceTemp);
                _surfaceLocalForce *= _forceFactorTempLocalSurface;
            }
            else
            {
                _surfaceLocalForce = Vector2.zero;
            }

            return _surfaceLocalForce;
        }

        public void TireSlips(WheelCollider wheelCollider, WheelHit wheelHit)
        {
            _localVelocityWheelTireSlips = WheelLocalVelocity(wheelHit);
            _localSurfaceForceDTireSlips = LocalSurfaceForce(wheelHit);

            if (KMh > _vehicleTorque.maxVelocityKMh)
            {
                _reverseForce = -5 * _rigidbody.velocity.magnitude;
            }
            else
            {
                _reverseForce = 0;
            }
            
            _angularWheelVelocityTireSlips = AngularVelocity(_localVelocityWheelTireSlips, wheelCollider);
            if (wheelCollider.isGrounded)
            {
                _estimatedSprungMass = Mathf.Clamp(wheelHit.force / -Physics.gravity.y, 0.0f, wheelCollider.sprungMass) * 0.5f;
                _localRigForceTireSlips = (-_estimatedSprungMass * _localVelocityWheelTireSlips / Time.deltaTime) + _localSurfaceForceDTireSlips;
                _tireSlipTireSlips.x = _localVelocityWheelTireSlips.x;
                _tireSlipTireSlips.y = _localVelocityWheelTireSlips.y - _angularWheelVelocityTireSlips * wheelCollider.radius;
                _downForceTireSlips = (_currentDownForceVehicle * _vehicleSettings.vehicleMass);

                if (wheelCollider.brakeTorque > 10)
                {
                    _wheelMaxBrakeSlip = Mathf.Max(Mathf.Abs(_localVelocityWheelTireSlips.y * 0.2f), 0.3f);
                    _minSlipYTireSlips = Mathf.Clamp(Mathf.Abs(_reverseForce * _tireSlipTireSlips.x) / _downForceTireSlips, 0.0f, _wheelMaxBrakeSlip);
                }
                else
                {
                    _minSlipYTireSlips = Mathf.Min(Mathf.Abs(_reverseForce * _tireSlipTireSlips.x) / _downForceTireSlips, Mathf.Clamp((_verticalInput * 2.5f), -2.5f, 1.0f));

                    if (_reverseForce != 0.0f && _minSlipYTireSlips < 0.1f) _minSlipYTireSlips = 0.1f;
                }

                if (Mathf.Abs(_tireSlipTireSlips.y) < _minSlipYTireSlips) 
                    _tireSlipTireSlips.y = _minSlipYTireSlips * Mathf.Sign(_tireSlipTireSlips.y);

                _rawTireForceTireSlips = -_downForceTireSlips * _tireSlipTireSlips.normalized;
                _rawTireForceTireSlips.x = Mathf.Abs(_rawTireForceTireSlips.x);
                _rawTireForceTireSlips.y = Mathf.Abs(_rawTireForceTireSlips.y);

                _tireForceTireSlips.x = Mathf.Clamp(_localRigForceTireSlips.x, -_rawTireForceTireSlips.x, +_rawTireForceTireSlips.x);
                if (wheelCollider.brakeTorque > 10)
                {
                    _maxFyTireSlips = Mathf.Min(_rawTireForceTireSlips.y, _reverseForce);
                    _tireForceTireSlips.y = Mathf.Clamp(_localRigForceTireSlips.y, -_maxFyTireSlips, +_maxFyTireSlips);
                }
                else
                {
                    _tireForceTireSlips.y = Mathf.Clamp(_reverseForce, -_rawTireForceTireSlips.y, +_rawTireForceTireSlips.y);
                }
            }
            else
            {
                _tireSlipTireSlips = Vector2.zero;
                _tireForceTireSlips = Vector2.zero;
            }

            _tireSL = _tireSlipTireSlips * _vehicleSettings.improveControl.tireSlipsFactor;
            _tireFO = _tireForceTireSlips * _vehicleSettings.improveControl.tireSlipsFactor;
        }

        #endregion

        #region Stabilizer

        void StabilizeAngularRotation()
        {
            if (Mathf.Abs(_horizontalInput) < 0.9f)
            {
                _rigidbody.angularVelocity = Vector3.Lerp(_rigidbody.angularVelocity, new Vector3(_rigidbody.angularVelocity.x, 0, _rigidbody.angularVelocity.z), Time.deltaTime * 2);
            }
        }

        void StabilizeAirRotation()
        {
            if (!_colliding)
            {
                _isGroundedExtraW = false;
                if (!_wheelFrontRightIsGrounded && !_wheelFrontLeftIsGrounded && !_wheelRearRightIsGrounded && !_wheelRearLeftIsGrounded&& !_isGroundedExtraW)
                {
                    _axisFromRotate = Vector3.Cross(transform.up, Vector3.up);
                    _torqueForceAirRotation = _axisFromRotate.normalized * _axisFromRotate.magnitude * 2.0f;
                    _torqueForceAirRotation -= _rigidbody.angularVelocity;
                    _rigidbody.AddTorque(_torqueForceAirRotation * _rigidbody.mass * 0.02f, ForceMode.Impulse);
                    if (Mathf.Abs(_horizontalInput) > 0.1f)
                    {
                        _rigidbody.AddTorque(transform.forward * -_horizontalInput * _vehicleSettings.vehicleMass * 0.6f);
                    }
                    if (Mathf.Abs(_verticalInput) > 0.1f)
                    {
                        _rigidbody.AddTorque(transform.right * _verticalInput * _vehicleSettings.vehicleMass * 0.44f);
                    }
                }
            }
        }

        void StabilizeWheelRPM()
        {
            if (CurrentGear > 0)
            {
                if (KMh > (_vehicleTorque.maxVelocityGears[CurrentGear - 1] * _vehicleTorque.speedOfGear) && Mathf.Abs(_verticalInput) < 0.5f)
                {
                    if (_wheels.rightFrontWheel.wheelDrive)
                    {
                        _wheels.rightFrontWheel.wheelCollider.brakeTorque = _forceEngineBrake;
                    }
                    if (_wheels.leftFrontWheel.wheelDrive)
                    {
                        _wheels.leftFrontWheel.wheelCollider.brakeTorque = _forceEngineBrake;
                    }
                    if (_wheels.rightRearWheel.wheelDrive)
                    {
                        _wheels.rightRearWheel.wheelCollider.brakeTorque = _forceEngineBrake;
                    }
                    if (_wheels.leftRearWheel.wheelDrive)
                    {
                        _wheels.leftRearWheel.wheelCollider.brakeTorque = _forceEngineBrake;
                    }
                }
            }
            else if (CurrentGear == -1)
            {
                if (KMh > (_vehicleTorque.maxVelocityGears[0] * _vehicleTorque.speedOfGear) && Mathf.Abs(_verticalInput) < 0.5f)
                {
                    if (_wheels.rightFrontWheel.wheelDrive)
                    {
                        _wheels.rightFrontWheel.wheelCollider.brakeTorque = _forceEngineBrake / 5.0f;
                    }
                    if (_wheels.leftFrontWheel.wheelDrive)
                    {
                        _wheels.leftFrontWheel.wheelCollider.brakeTorque = _forceEngineBrake / 5.0f;
                    }
                    if (_wheels.rightRearWheel.wheelDrive)
                    {
                        _wheels.rightRearWheel.wheelCollider.brakeTorque = _forceEngineBrake / 5.0f;
                    }
                    if (_wheels.leftRearWheel.wheelDrive)
                    {
                        _wheels.leftRearWheel.wheelCollider.brakeTorque = _forceEngineBrake / 5.0f;
                    }
                }
            }
        }

        void StabilizeVehicleRollForces()
        {
            _leftFrontForce = 1.0f;
            _rightFrontForce = 1.0f;
            _leftRearForce = 1.0f;
            _rightRearForce = 1.0f;
            
            bool isGround1 = _wheels.leftRearWheel.wheelCollider.GetGroundHit(out _tempWheelHit);
            if (isGround1)
            {
                _leftRearForce = (-_wheels.leftRearWheel.wheelCollider.transform.InverseTransformPoint(_tempWheelHit.point).y - _wheels.leftRearWheel.wheelCollider.radius) / _wheels.leftRearWheel.wheelCollider.suspensionDistance;
            }
            bool isGround2 = _wheels.rightRearWheel.wheelCollider.GetGroundHit(out _tempWheelHit);
            if (isGround2)
            {
                _rightRearForce = (-_wheels.rightRearWheel.wheelCollider.transform.InverseTransformPoint(_tempWheelHit.point).y - _wheels.rightRearWheel.wheelCollider.radius) / _wheels.rightRearWheel.wheelCollider.suspensionDistance;
            }
            
            bool isGround3 = _wheels.leftFrontWheel.wheelCollider.GetGroundHit(out _tempWheelHit);
            if (isGround3)
            {
                _leftFrontForce = (-_wheels.leftFrontWheel.wheelCollider.transform.InverseTransformPoint(_tempWheelHit.point).y - _wheels.leftFrontWheel.wheelCollider.radius) / _wheels.leftFrontWheel.wheelCollider.suspensionDistance;
            }
            bool isGround4 = _wheels.rightFrontWheel.wheelCollider.GetGroundHit(out _tempWheelHit);
            if (isGround4)
            {
                _rightFrontForce = (-_wheels.rightFrontWheel.wheelCollider.transform.InverseTransformPoint(_tempWheelHit.point).y - _wheels.rightFrontWheel.wheelCollider.radius) / _wheels.rightFrontWheel.wheelCollider.suspensionDistance;
            }

            _roolForce1 = (_leftRearForce - _rightRearForce) * _vehicleSettings._aerodynamics.feelingHeavy * _vehicleSettings.vehicleMass * _inclinationFactorForcesDown;
            _roolForce2 = (_leftFrontForce - _rightFrontForce) * _vehicleSettings._aerodynamics.feelingHeavy * _vehicleSettings.vehicleMass * _inclinationFactorForcesDown;
            
            if (isGround1)
            {
                _rigidbody.AddForceAtPosition(_wheels.leftRearWheel.wheelCollider.transform.up * -_roolForce1, _wheels.leftRearWheel.wheelCollider.transform.position);
            }
            if (isGround2)
            {
                _rigidbody.AddForceAtPosition(_wheels.rightRearWheel.wheelCollider.transform.up * _roolForce1, _wheels.rightRearWheel.wheelCollider.transform.position);
            }
            
            if (isGround3)
            {
                _rigidbody.AddForceAtPosition(_wheels.leftFrontWheel.wheelCollider.transform.up * -_roolForce2, _wheels.leftFrontWheel.wheelCollider.transform.position);
            }
            if (isGround4)
            {
                _rigidbody.AddForceAtPosition(_wheels.rightFrontWheel.wheelCollider.transform.up * _roolForce2, _wheels.rightFrontWheel.wheelCollider.transform.position);
            }
        }

        #endregion

        #region Gear Manager

        void ChangeGearInput()
        {
            if (!_changinGearsAuto)
            {
                _engineInput = Mathf.Clamp01(_verticalInput);
            }
            else
            {
                _engineInput = 0;
            }
        }

        void AutomaticGears()
        {
            if (CurrentGear == 0)
            {
                if (_mediumRPM < 5 && _mediumRPM >= 0)
                {
                    CurrentGear = 1;
                }
                if (_mediumRPM > -5 && _mediumRPM < 0)
                {
                    CurrentGear = -1;
                }
            }
            if (_mediumRPM < -0.1f && Mathf.Abs(_verticalInput) < 0.1f)
            {
                CurrentGear = -1;
            }
            if (Mathf.Abs(_verticalInput) < 0.1f && _mediumRPM >= 0 && CurrentGear < 2)
            {
                CurrentGear = 1;
            }

            if ((Mathf.Abs(Mathf.Clamp(_verticalInput, -1f, 0f))) > 0.2f)
            {
                if ((KMh < 5) || _mediumRPM < -2)
                {
                    CurrentGear = -1;
                }
            }
            if ((Mathf.Abs(Mathf.Clamp(_verticalInput, 0f, 1f))) > 0.2f)
            {
                if ((KMh < 5) || (_mediumRPM > 2 && CurrentGear < 2))
                {
                    CurrentGear = 1;
                }
            }

            if (CurrentGear > 0)
            {
                if (KMh > (_vehicleTorque.idealVelocityGears[CurrentGear - 1] * _vehicleTorque.speedOfGear + 7 * _vehicleTorque.speedOfGear))
                {
                    if (CurrentGear < _vehicleTorque.numberOfGears && !_changinGearsAuto && CurrentGear != -1)
                    {
                        _timeAutoGear = 1.5f;
                        StartCoroutine("TimeAutoGears", CurrentGear + 1);
                    }
                }
                else if (KMh < (_vehicleTorque.idealVelocityGears[CurrentGear - 1] * _vehicleTorque.speedOfGear - 15 * _vehicleTorque.speedOfGear))
                {
                    if (CurrentGear > 1 && !_changinGearsAuto)
                    {
                        _timeAutoGear = 0;
                        StartCoroutine("TimeAutoGears", CurrentGear - 1);
                    }
                }
                if (_verticalInput > 0.1f && KMh > (_vehicleTorque.idealVelocityGears[CurrentGear - 1] * _vehicleTorque.speedOfGear + 7 * _vehicleTorque.speedOfGear))
                {
                    if (CurrentGear < _vehicleTorque.numberOfGears && CurrentGear != -1)
                    {
                        _timeAutoGear = 0.0f;
                        StartCoroutine("TimeAutoGears", CurrentGear + 1);
                    }
                }
            }
        }
        
        IEnumerator TimeAutoGears(int gear)
        {
            _changinGearsAuto = true;
            yield return new WaitForSeconds(0.4f);
            CurrentGear = gear;
            yield return new WaitForSeconds(_timeAutoGear);
            _changinGearsAuto = false;
        }

        #endregion

        #region Torque

        public float VehicleTorque(WheelCollider wheelCollider)
        {
            _torqueM = 0;
            _rpmTempTorque = Mathf.Abs(wheelCollider.rpm);

            if ((Mathf.Abs(_verticalInput) < 0.5f) || KMh > _vehicleTorque.maxVelocityKMh)
            {
                return 0;
            }

            if ((_rpmTempTorque * wheelCollider.radius) > (50.0f * _vehicleTorque.numberOfGears * _vehicleTorque.speedOfGear))
            {
                return 0;
            }

            if (KMh < 0.5f)
            {
                if (_rpmTempTorque > (25.0f / wheelCollider.radius))
                {
                    return 0;
                }
            }

            if (!_theEngineIsRunning)
            {
                return 0;
            }

            if (handBrakeTrue)
            {
                return 0;
            }

            if (_isBraking)
            {
                return 0;
            }

            if (_currentBrakeValue > 0.1f)
            {
                return 0;
            }

            if (_brakeInput)
            {
                return 0;
            }

            if (CurrentGear < 0)
            {
                _clampInputTorque = Mathf.Abs(Mathf.Clamp(_verticalInput, -1f, 0f));
                _torqueM = (500.0f * _vehicleTorque.engineTorque) * _clampInputTorque * (_vehicleTorque.gears[0].Evaluate((KMh / _vehicleTorque.speedOfGear))) * -0.8f;
            }
            else if (CurrentGear == 0)
            {
                return 0;
            }
            else
            {
                _torqueM = (500.0f * _vehicleTorque.engineTorque) * (Mathf.Clamp(_engineInput, 0f, 1f)) * _vehicleTorque.gears[CurrentGear - 1].Evaluate((KMh / _vehicleTorque.speedOfGear));
            }
            
            _adjustTorque = 1;
            if (CurrentGear < _vehicleTorque.manualAdjustmentOfTorques.Length && CurrentGear > 0)
            {
                if (CurrentGear == -1)
                {
                    _adjustTorque = _vehicleTorque.manualAdjustmentOfTorques[0];
                }
                else if (CurrentGear == 0)
                {
                    _adjustTorque = 0;
                }
                else if (CurrentGear > 0)
                {
                    _adjustTorque = _vehicleTorque.manualAdjustmentOfTorques[CurrentGear - 1];
                }
            }
            else
            {
                _adjustTorque = 1;
            }

            return _torqueM * _adjustTorque * _vehicleScale;
        }

        void ApplyTorque()
        {
            _leftDifferential = 1 + Mathf.Abs((0.2f * Mathf.Abs(Mathf.Clamp(_horizontalInput, 0, 1))) * (_angleRefVolant / 60));
            _rightDifferential = 1 + Mathf.Abs((0.2f * Mathf.Abs(Mathf.Clamp(_horizontalInput, -1, 0))) * (_angleRefVolant / 60));

            if (_theEngineIsRunning)
            {
                if (_wheels.rightFrontWheel.wheelDrive)
                {
                    _wheels.rightFrontWheel.wheelCollider.motorTorque = VehicleTorque(_wheels.rightFrontWheel.wheelCollider) * _rightDifferential;
                }
                if (_wheels.leftFrontWheel.wheelDrive)
                {
                    _wheels.leftFrontWheel.wheelCollider.motorTorque = VehicleTorque(_wheels.leftFrontWheel.wheelCollider) * _leftDifferential;
                }
                if (_wheels.rightRearWheel.wheelDrive)
                {
                    _wheels.rightRearWheel.wheelCollider.motorTorque = VehicleTorque(_wheels.rightRearWheel.wheelCollider) * _rightDifferential;
                }
                if (_wheels.leftRearWheel.wheelDrive)
                {
                    _wheels.leftRearWheel.wheelCollider.motorTorque = VehicleTorque(_wheels.leftRearWheel.wheelCollider) * _leftDifferential;
                }
            }
            else
            {
                if (_wheels.rightFrontWheel.wheelDrive)
                {
                    _wheels.rightFrontWheel.wheelCollider.motorTorque = 0;
                }
                if (_wheels.leftFrontWheel.wheelDrive)
                {
                    _wheels.leftFrontWheel.wheelCollider.motorTorque = 0;
                }
                if (_wheels.rightRearWheel.wheelDrive)
                {
                    _wheels.rightRearWheel.wheelCollider.motorTorque = 0;
                }
                if (_wheels.leftRearWheel.wheelDrive)
                {
                    _wheels.leftRearWheel.wheelCollider.motorTorque = 0;
                }
            }
        }

        #endregion

        #region Brakes

        void Brakes()
        {
            _brakeVerticalInput = 0.0f;

            _brakeVerticalInput = _verticalInput;

            if (CurrentGear > 0)
            {
                _currentBrakeValue = Mathf.Abs(Mathf.Clamp(_brakeVerticalInput, -1.0f, 0.0f)) * 1.5f;
            }
            else if (CurrentGear < 0)
            {
                _currentBrakeValue = Mathf.Abs(Mathf.Clamp(_brakeVerticalInput, 0.0f, 1.0f)) * 1.5f;
            }
            else if (CurrentGear == 0)
            {
                if (_mediumRPM > 0)
                {
                    _currentBrakeValue = Mathf.Abs(Mathf.Clamp(_brakeVerticalInput, -1.0f, 0.0f)) * 1.5f;
                }
                else
                {
                    _currentBrakeValue = Mathf.Abs(Mathf.Clamp(_brakeVerticalInput, 0.0f, 1.0f)) * 1.5f;
                }
            }

            _handBrake_Input = 0.0f;
            if (handBrakeTrue)
            {
                if (Mathf.Abs(_brakeVerticalInput) < 0.9f)
                {
                    _handBrake_Input = 2;
                }
                else
                {
                    _handBrake_Input = 0;
                    handBrakeTrue = false;
                }
            }
            else
            {
                _handBrake_Input = 0;
            }

            if (_brakeInput)
            {
                _handBrake_Input = 2;
            }

            _handBrake_Input = _handBrake_Input * 1000;
            _totalFootBrake = _currentBrakeValue * 0.5f * _vehicleSettings.vehicleMass;
            _totalHandBrake = _handBrake_Input * 0.5f * _vehicleSettings.vehicleMass;

            if (isInsideTheCar)
            {
                if (Mathf.Abs(_mediumRPM) < 15 && Mathf.Abs(_brakeVerticalInput) < 0.05f && !handBrakeTrue && (_totalFootBrake + _totalHandBrake) < 100)
                {
                    _brakingAuto = true;
                    _totalFootBrake = 1.5f * _vehicleSettings.vehicleMass;
                }
                else
                {
                    _brakingAuto = false;
                }
            }
            else
            {
                _brakingAuto = false;
            }

            if (_totalFootBrake > 10)
            {
                _isBraking = true;
            }
            else
            {
                _isBraking = false;
            }

            if (!_brakingAuto)
            {
                if (_isBraking && Mathf.Abs(KMh) > 1.2f)
                {
                    _totalFootBrake = 0;
                }
            }

            ApplyBrakeInWheels(_wheels.rightFrontWheel.wheelCollider, _wheels.rightFrontWheel.wheelHandBrake);
            ApplyBrakeInWheels(_wheels.leftFrontWheel.wheelCollider, _wheels.leftFrontWheel.wheelHandBrake);
            ApplyBrakeInWheels(_wheels.rightRearWheel.wheelCollider, _wheels.rightRearWheel.wheelHandBrake);
            ApplyBrakeInWheels(_wheels.leftRearWheel.wheelCollider, _wheels.leftRearWheel.wheelHandBrake);
        }

        void ApplyBrakeInWheels(WheelCollider wheelCollider, bool handBrake)
        {
            if (handBrake)
            {
                wheelCollider.brakeTorque = _totalFootBrake + _totalHandBrake;
            }
            else
            {
                wheelCollider.brakeTorque = _totalFootBrake;
            }

            if (!wheelCollider.isGrounded && Mathf.Abs(wheelCollider.rpm) > 0.5f && Mathf.Abs(_verticalInput) < 0.05f && wheelCollider.motorTorque < 5.0f)
            {
                wheelCollider.brakeTorque += _vehicleSettings.vehicleMass * Time.deltaTime * 50;
            }
            if (KMh < 0.5f && Mathf.Abs(_verticalInput) < 0.05f)
            {
                if (wheelCollider.rpm > (25 / wheelCollider.radius))
                {
                    wheelCollider.brakeTorque += 0.5f * _vehicleSettings.vehicleMass * Mathf.Abs(wheelCollider.rpm) * Time.deltaTime;
                }
            }
        }

        #endregion

        #region Volant

        void Volant()
        {
            _angle1Ref = Mathf.MoveTowards(_angle1Ref, _horizontalInput, 2 * Time.deltaTime);
            _angle2Volant = Mathf.MoveTowards(_angle2Volant, _horizontalInput, 2 * Time.deltaTime);
            _maxAngleVolant = 27.0f * _vehicleSettings.angle_x_Velocity.Evaluate(KMh);
            _angleRefVolant = Mathf.Clamp(_angle1Ref * _maxAngleVolant, -_maxAngleVolant, _maxAngleVolant);

            if (_angle1Ref > 0.2f)
            {
                if (_wheels.rightFrontWheel.wheelTurn)
                {
                    _wheels.rightFrontWheel.wheelCollider.steerAngle = _angleRefVolant * 1.2f;
                }
                if (_wheels.leftFrontWheel.wheelTurn)
                {
                    _wheels.leftFrontWheel.wheelCollider.steerAngle = _angleRefVolant;
                }
                if (_wheels.rightRearWheel.wheelTurn)
                {
                    _wheels.rightRearWheel.wheelCollider.steerAngle = _angleRefVolant * 1.2f;
                }
                if (_wheels.leftRearWheel.wheelTurn)
                {
                    _wheels.leftRearWheel.wheelCollider.steerAngle = _angleRefVolant;
                }
            }
            else if (_angle1Ref < -0.2f)
            {
                if (_wheels.rightFrontWheel.wheelTurn)
                {
                    _wheels.rightFrontWheel.wheelCollider.steerAngle = _angleRefVolant;
                }
                if (_wheels.leftFrontWheel.wheelTurn)
                {
                    _wheels.leftFrontWheel.wheelCollider.steerAngle = _angleRefVolant * 1.2f;
                }
                if (_wheels.rightRearWheel.wheelTurn)
                {
                    _wheels.rightRearWheel.wheelCollider.steerAngle = _angleRefVolant;
                }
                if (_wheels.leftRearWheel.wheelTurn)
                {
                    _wheels.leftRearWheel.wheelCollider.steerAngle = _angleRefVolant * 1.2f;
                }
            }
            else
            {
                if (_wheels.rightFrontWheel.wheelTurn)
                {
                    _wheels.rightFrontWheel.wheelCollider.steerAngle = _angleRefVolant;
                }
                if (_wheels.leftFrontWheel.wheelTurn)
                {
                    _wheels.leftFrontWheel.wheelCollider.steerAngle = _angleRefVolant;
                }
                if (_wheels.rightRearWheel.wheelTurn)
                {
                    _wheels.rightRearWheel.wheelCollider.steerAngle = _angleRefVolant;
                }
                if (_wheels.leftRearWheel.wheelTurn)
                {
                    _wheels.leftRearWheel.wheelCollider.steerAngle = _angleRefVolant;
                }
            }

            if (_vehicleSettings.volant)
            {
                if (_vehicleSettings.volant)
                {
                    _vehicleSettings.volant.transform.localEulerAngles = new Vector3(_vehicleSettings.volant.transform.localEulerAngles.x, _vehicleSettings.volant.transform.localEulerAngles.y, _volantStartRotation + (_angle2Volant * 320.0f));
                }
            }
        }
        #endregion

#endregion
    }
}
