using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class WrongAreaChild : MonoBehaviour
    {
        private void Awake()
        {
            GameObject parent = transform.parent.gameObject;

            parent.TryGetComponent(out WrongAreaTrigger wrong);

            if (wrong != null)
                return;

            parent.AddComponent<WrongAreaTrigger>();
        }
    }
}
