using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

namespace DrivingSimulation
{
    public class InitTrackRoad : MonoBehaviour
    {
        private NavMeshSurface _navMeshSurface;

        private void Awake()
        {
            if (!TryGetComponent(out _navMeshSurface))
            {
                _navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
                _navMeshSurface.layerMask = LayerMask.NameToLayer(ConstantVariable.ROAD_LAYER_NAME);
            }

            _navMeshSurface.BuildNavMesh();
        }
    }
}
