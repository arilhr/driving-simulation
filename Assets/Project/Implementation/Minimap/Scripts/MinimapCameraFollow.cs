using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField]
    private Transform _objectToFollow = null;
    [SerializeField]
    private Vector3 _offset = Vector3.zero;

    private void Update()
    {
        UpdateCameraPosition();
        UpdateCameraRotation();
    }

    void UpdateCameraPosition()
    {
        transform.position = _objectToFollow.position + _offset;
    }

    void UpdateCameraRotation()
    {
        Vector3 euler = new Vector3(transform.rotation.eulerAngles.x, _objectToFollow.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler(euler);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_objectToFollow == null) return;

        UpdateCameraPosition();
    }
#endif
}
