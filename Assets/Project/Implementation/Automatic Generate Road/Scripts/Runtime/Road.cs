using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class Road : MonoBehaviour
    {
        [Header("Road Area")]
        public GameObject LeftAreaColliderObj;
        public GameObject RightAreaColliderObj;

        private void Awake()
        {
            if (RightAreaColliderObj != null)
            {
                RightAreaColliderObj.AddComponent<WrongAreaTrigger>();
            }
        }
    }
}

