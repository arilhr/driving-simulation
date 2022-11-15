using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Range(1, 30)]
    [Tooltip("The speed at which the camera rotates as it follows and looks at the player.")]
    public float SpinSpeed = 15.0f;
    [Range(1, 20)]
    [Tooltip("The speed at which the camera can follow the player.")]
    public float DisplacementSpeed = 5.0f;
    [Tooltip("The object that will follow by the camera.")]
    public GameObject ObjectToFollow = null;

    private GameObject _startCameraPosition = null;
    private Quaternion _newRotationCameras = Quaternion.identity;

    void Awake()
    {
        _startCameraPosition = new GameObject("Start Camera Position");
        _startCameraPosition.layer = gameObject.layer;
        _startCameraPosition.transform.parent = ObjectToFollow.transform;
        _startCameraPosition.transform.position = transform.position;
    }

    void LateUpdate()
    {
        if (!Physics.Linecast(ObjectToFollow.transform.position, _startCameraPosition.transform.position))
        {
            transform.position = Vector3.Lerp(transform.position, _startCameraPosition.transform.position, Time.deltaTime * DisplacementSpeed);
        }
        else if (Physics.Linecast(ObjectToFollow.transform.position, _startCameraPosition.transform.position, out var hitCameras))
        {
            transform.position = Vector3.Lerp(transform.position, hitCameras.point, Time.deltaTime * DisplacementSpeed);
        }

        _newRotationCameras = Quaternion.LookRotation(ObjectToFollow.transform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, _newRotationCameras, Time.deltaTime * SpinSpeed);
    }
}
