using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class WrongAreaTrigger : MonoBehaviour
    {
        private const string PLAYER_LAYER_NAME = "Player";

        private void Awake()
        {
            transform.TryGetComponent(out Rigidbody rb);

            if (rb == null)
            {
                Rigidbody newRb = gameObject.AddComponent<Rigidbody>();
                newRb.isKinematic = true;   
            }
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer(PLAYER_LAYER_NAME)) return;

            Debug.Log($"You in the wrong lane!");
        }
    }
}
