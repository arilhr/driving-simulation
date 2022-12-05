using Sirenix.OdinInspector;
using SOGameEvents;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnSignalInput : MonoBehaviour
{
    #region Variables

    [BoxGroup("Right Signal")]
    [SerializeField]
    private GameEventNoParam _toogleTurnSignalRightCallback = null;

    [BoxGroup("Right Signal")]
    [SerializeField]
    private GameEventBool _onTurnRightSignalChanged = null;

    [BoxGroup("Left Signal")]
    [SerializeField]
    private GameEventNoParam _toogleTurnSignalLeftCallback = null;

    [BoxGroup("Left Signal")]
    [SerializeField]
    private GameEventBool _onTurnLeftSignalChanged = null;

    private bool _isLeftActive = false;
    private bool _isRightActive = false;

    private CarInput _carInput = null;

    #endregion

    #region Mono

    private void Awake()
    {
        _carInput = new CarInput();

        Initialize();

        _carInput.Enable();
    }

    private void Initialize()
    {
        _carInput.Controller.R_Signal.started += InputTurnRight;
        _carInput.Controller.L_Signal.started += InputTurnLeft;
        _onTurnLeftSignalChanged.AddListener(OnTurnLeftSignal);
        _onTurnRightSignalChanged.AddListener(OnTurnRightSignal);
    }

    private void Dispose()
    {
        _carInput.Controller.R_Signal.started -= InputTurnRight;
        _carInput.Controller.L_Signal.started -= InputTurnLeft;
        _onTurnLeftSignalChanged.RemoveListener(OnTurnLeftSignal);
        _onTurnRightSignalChanged.RemoveListener(OnTurnRightSignal);
    }

    private void InputTurnRight(InputAction.CallbackContext ctx)
    {
        ToogleRightSignal();
    }

    private void ToogleRightSignal()
    {
        _toogleTurnSignalRightCallback.Invoke();
    }

    private void InputTurnLeft(InputAction.CallbackContext ctx)
    {
        ToogleLeftSignal();
    }

    private void ToogleLeftSignal()
    {
        _toogleTurnSignalLeftCallback.Invoke();
    }

    private void OnTurnLeftSignal(bool isActive)
    {
        _isLeftActive = isActive;

        if (_isRightActive && _isLeftActive)
        {
            ToogleRightSignal();
        }
    }

    private void OnTurnRightSignal(bool isActive)
    {
        _isRightActive = isActive;

        if (_isRightActive && _isLeftActive)
        {
            ToogleLeftSignal();
        }
    }

    private void OnDestroy()
    {
        Dispose();
        _carInput.Disable();
    }

    #endregion
}
