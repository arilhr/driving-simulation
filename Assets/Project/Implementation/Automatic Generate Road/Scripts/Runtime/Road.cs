using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrivingSimulation
{
    public class Road : MonoBehaviour
    {
        [Header("Road Area")]
        GameObject leftAreaColliderObj;
        GameObject rightAreaColliderObj;

        public GameObject LeftAreaColliderObj
        {
            get { return leftAreaColliderObj; }
            set
            {
                leftAreaColliderObj = value;
            }
        }

        public GameObject RightAreaColliderObj
        {
            get { return rightAreaColliderObj; }
            set
            {
                rightAreaColliderObj = value;

                if (!rightAreaColliderObj.TryGetComponent(out WrongAreaTrigger trigger))
                {
                    rightAreaColliderObj.AddComponent<WrongAreaTrigger>();
                }
            }
        }
    }
}

