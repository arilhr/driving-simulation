using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMotor : MonoBehaviour
{
    WheelCollider _wheelCol = null;

    private void Awake()
    {
        _wheelCol = GetComponent<WheelCollider>();
    }

    private void Update()
    {
        Debug.Log($"Motor: {_wheelCol.motorTorque}");
    }
}
