using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLookAt : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 rotation = transform.localRotation.eulerAngles;
        Quaternion offset = Quaternion.Euler(rotation.x, rotation.y + 90f, rotation.z);

        Gizmos.DrawLine(transform.position, transform.position + offset * transform.forward);
    }
}
